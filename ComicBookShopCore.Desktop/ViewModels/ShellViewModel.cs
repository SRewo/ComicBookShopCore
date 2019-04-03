using ComicBookShopCore.Desktop.Views;
using ComicBookShopCore.EmployeeModule.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.Desktop.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private IContainerExtension _container;
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigationCommand { get; private set; }
        public DelegateCommand DbCheckCommand { get; private set; }


        public ShellViewModel(IContainerExtension container, IRegionManager manager)
        {

            _container = container;
            _regionManager = manager;
            
            _regionManager.RegisterViewWithRegion("content", typeof(LoginView));
            container.RegisterForNavigation<MenuView>("MenuView");
            NavigationCommand = new DelegateCommand<string>(Navigate);
            ApplicationCommands.NavigateCommand.RegisterCommand(NavigationCommand);

        }

        private void Navigate(string control)
        {
            if (control != null)
            {
                _regionManager.RequestNavigate("content", control);
            }
        }

    }
}
