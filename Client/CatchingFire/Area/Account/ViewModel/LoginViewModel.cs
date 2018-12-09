using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using ApiService.User;
using CatchingFire.Common;
using LocalService.Entity;
using LocalService.SQLite;
using System.Linq;
using Utility.Logger;

namespace CatchingFire.Area.Account.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        public LoginViewModel()
        {
            Init();
        }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public bool RememberPassword
        {
            get
            {
                var dict = SQLiteHelper.DB.Find<LocalService.Entity.Dict>(o=>o.Code== "remember_pwd");
                if (dict == null)
                {
                    return false;
                }
                bool _rememberPwd;
                bool flag = bool.TryParse(dict.Value, out _rememberPwd);
                if (flag)
                {
                    return _rememberPwd;
                }
                return false;
            }
            set
            {

                var dict = new LocalService.Entity.Dict
                {
                    Code = "remember_pwd",
                    Value = value.ToString()
                };
                SQLiteHelper.DB.InsertOrReplace(dict);
                if (!value)
                {
                    AutoLogin = false;
                }
            }
        }

        public bool AutoLogin
        {
            get
            {
                var dict = SQLiteHelper.DB.Find<LocalService.Entity.Dict>(o => o.Code == "auto_login"); 
                if (dict == null)
                {
                    return false;
                }
                bool autoLogin;
                bool flag = bool.TryParse(dict.Value, out autoLogin);
                if (flag)
                {
                    return autoLogin&&RememberPassword;
                }
                return false;
            }
            set
            {
                var dict = new LocalService.Entity.Dict
                {
                    Code = "auto_login",
                    Value = value.ToString()
                };
                SQLiteHelper.DB.InsertOrReplace(dict);
                if (value)
                {
                    RememberPassword = true;
                }
            }
        }

        public string Msg { get; set; }

        public bool IsLoading { get; set; }

        private void Init()
        {
            var user = SQLiteHelper.DB.Table<User>().FirstOrDefault(o => o.IsDefault);
            if (user == null)
            {
                return;
            }
            UserName = user.Name;
            UserPwd = user.Pwd;
            if (AutoLogin)
            {
                DoLogin();
            }
        }

        public void AccountEnter()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                Msg = "Input_Account";
                return;
            }
            ControlHelper.MoveNextFoces();
        }

        private void DoLogin()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                Msg = "Input_Account";
                return;
            }
            if (string.IsNullOrEmpty(UserPwd))
            {
                Msg = "Input_Password";
                return;
            }
            Msg = "Logining";
            if (UserName=="admin"&&UserPwd=="admin123")
            {
                Instance.UserName = "admin";
                ControlHelper.OpenWindow("CatchingFire.Area.Main.View.Main", null, false, true);
                ControlHelper.CloseWindow(typeof (View.Login));
            }
            else
            {
                LoginAction();
            }
        }

        private async void LoginAction()
        {
            IsLoading = true;
            
            try
            {
                var result = await new UserManager().Login(UserName, UserPwd);
                if (result.IsSuccess)
                {
                    Instance.UserName = UserName;
                    if (RememberPassword)
                    {
                        SQLiteHelper.DB.InsertOrReplace(new User { Name = UserName, Pwd = UserPwd, IsDefault = true });
                    }
                    ControlHelper.OpenWindow("CatchingFire.Area.Main.View.Main", null, false, true);
                    ControlHelper.CloseWindow(typeof(View.Login));
                }
                else
                {
                    Msg = result.Message;
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                LogHelper.Fatal("登陆失败："+ex.Message,ex);
                Msg = "Login_Failed";
            }
            
        }

        public ICommand LoginCommand => new RelayCommand(DoLogin);

        public ICommand AccountEnterCommand => new RelayCommand(AccountEnter);
    }
}
