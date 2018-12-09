/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CatchingFire"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CatchingFire.Area.Account.ViewModel;
using CatchingFire.Area.Main.ViewModel;
using CatchingFire.Area.Sys.ViewModel;
using CatchingFire.Area.TechnologySummarize.ViewModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace CatchingFire.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ControlsViewModel>();
            SimpleIoc.Default.Register<LogViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<DownloadListViewModel>();
            SimpleIoc.Default.Register<CreateDownloadViewModel>();
            SimpleIoc.Default.Register<OrganizationViewModel>();
            SimpleIoc.Default.Register<RoleViewModel>();
            SimpleIoc.Default.Register<DictViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public ControlsViewModel Controls => ServiceLocator.Current.GetInstance<ControlsViewModel>();

        public LogViewModel LogVM => ServiceLocator.Current.GetInstance<LogViewModel>();

        public MenuViewModel MenuVM => ServiceLocator.Current.GetInstance<MenuViewModel>();

        public UserViewModel UserVM => ServiceLocator.Current.GetInstance<UserViewModel>();

        public OrganizationViewModel OrganizationVM => ServiceLocator.Current.GetInstance<OrganizationViewModel>();

        public RoleViewModel RoleVM => ServiceLocator.Current.GetInstance<RoleViewModel>();

        public DictViewModel DictVM => ServiceLocator.Current.GetInstance<DictViewModel>();


        public DownloadListViewModel DoanlowListVM => ServiceLocator.Current.GetInstance<DownloadListViewModel>();

        public CreateDownloadViewModel CreateDownloadVM => ServiceLocator.Current.GetInstance<CreateDownloadViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}