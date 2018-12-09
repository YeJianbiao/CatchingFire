

namespace CatchingFire.Area.Account.View
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login 
    {
        public Login()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                txtAccount.Focus();
            };
        }
    }
}
