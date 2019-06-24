using ComicBookShopCore.OrderModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ComicBookShopCore.OrderModule
{
    public class OrderModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AddOrderView>("AddOrder");
            containerRegistry.RegisterForNavigation<OrderListView>("OrderList");
        }
    }
}
