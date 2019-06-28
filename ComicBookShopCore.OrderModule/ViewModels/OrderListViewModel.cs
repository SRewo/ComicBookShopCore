using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Filters;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class OrderListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _manager;
        private readonly IRepository<Order> _orderRepository;
        private IUserEmployeeFilterFactory _factory;

        private DateTime _dateFrom;

        private DateTime _dateTo;

        private bool _isEmployeeSelected;

        private bool _isUserSelected;

        private string _searchWord;

        private Order _selectedOrder;

        private List<Order> _orders;

        public OrderListViewModel(IRepository<Order> orderRepository, IRegionManager manager, IUserEmployeeFilterFactory factory)
        {
            _orderRepository = orderRepository;
            _manager = manager;
            _factory = factory;
            InitializeCommandsAsync();
        }

        public DelegateCommand ResetFormCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set => SetProperty(ref _dateFrom, value);
        }

        public DateTime DateTo
        {
            get => _dateTo;
            set => SetProperty(ref _dateTo, value);
        }

        public bool IsUserSelected
        {
            get => _isUserSelected;
            set => SetProperty(ref _isUserSelected, value);
        }

        public bool IsEmployeeSelected
        {
            get => _isEmployeeSelected;
            set => SetProperty(ref _isEmployeeSelected, value);
        }

        private List<Order> _viewList;

        public List<Order> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
        }


        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var tmp = await GetDataAsync().ConfigureAwait(true);
            _orders = tmp.ToList();
            ResetFormAsync();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public Task ResetFormAsync()
        { 

            IsUserSelected = true; 
            IsEmployeeSelected = true;
            DateTo = DateTime.Now; 
            DateFrom = DateTime.Now.AddDays(-14); 
            SearchWord = String.Empty;
            ViewList = _orders.Where(x => (x.Date <= DateTo && x.Date >= DateFrom)).ToList();

            return Task.CompletedTask;
        }

        public Task SearchAsync()
        {
            var searcher = _factory.CheckEmployeeOrUserAsync(IsEmployeeSelected, IsUserSelected).Result;
            ViewList =_orders
                .RoleSearchAsync(searcher).ToList();

            return Task.CompletedTask;
        }

        public Task InitializeCommandsAsync()
        {

            ResetFormCommand = new DelegateCommand(() =>
            { 
                ResetFormAsync();
            });

            SearchCommand = new DelegateCommand((() => SearchAsync()));

            return Task.CompletedTask;
        }

        

        public async Task<IQueryable<Order>> GetDataAsync() =>
             _orderRepository.GetAll().Include(x => x.OrderItems).ThenInclude(x => x.ComicBook);
    }

}