using ComicBookShopCore.EmployeeModule.ViewModels;
using ComicBookShopCore.EmployeeModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ComicBookShopCore.EmployeeModule
{
    public class EmployeeModule : IModule
    {

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView>("LoginView");
        }
    }
}
