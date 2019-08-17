using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class OrderDetailsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _manager;

        private Order _order;

        public OrderDetailsViewModel(IRegionManager manager)
        {
            _manager = manager;
            GoBackCommand = new DelegateCommand(() => GoBackAsync());
        }

        public DelegateCommand GoBackCommand { get; set; }

        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Order ??= (Order) navigationContext.Parameters["Order"];
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public Task GoBackAsync()
        {
            _manager.RequestNavigate("content", "OrderList");

            return Task.CompletedTask;
        }
    }
}