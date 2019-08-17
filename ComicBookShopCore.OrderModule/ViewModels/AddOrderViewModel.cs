using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class OrderItemInputModel : ValidableBase
    {
        private double _price;

        public OrderItemInputModel(OrderItem orderItem)
        {
            OrderItem = orderItem;
            Price = ComicBook.Price;
        }

        public ComicBook ComicBook
        {
            get => OrderItem.ComicBook;
            set
            {
                OrderItem.ComicBook = value;
                RaisePropertyChanged();
            }
        }

        public int Quantity
        {
            get => OrderItem.Quantity;
            set
            {
                if (value < ComicBook.Quantity)
                {
                    OrderItem.Quantity = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Discount
        {
            get => OrderItem.Discount;
            set
            {
                OrderItem.Discount = value;
                RaisePropertyChanged();
            }
        }

        public new bool HasErrors => OrderItem.HasErrors;

        public double Price
        {
            get => _price;
            private set => SetProperty(ref _price, value);
        }

        public OrderItem OrderItem { get; }

        public new event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add => OrderItem.ErrorsChanged += value;
            remove => OrderItem.ErrorsChanged -= value;
        }

        public new IEnumerable GetErrors(string propertyName)
        {
            return OrderItem.GetErrors(propertyName);
        }

        public new string GetFirstError(string propertyName)
        {
            return OrderItem.GetFirstError(propertyName);
        }

        public new string GetFirstError()
        {
            return OrderItem.GetFirstError();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            OrderItem.Validate();
            if (!HasErrors)
                Price = ComicBook.Price * Quantity * (1 - Discount * 0.01);
            else
                Price = 0;

            base.OnPropertyChanged(args);
        }
    }

    public class AddOrderViewModel : BindableBase, INavigationAware
    {
        private List<ComicBook> _allComicBooks;

        private bool _canSave;
        private readonly IRepository<ComicBook> _comicBookRepository;


        private List<ComicBook> _comicBooks;
        private readonly OrderBuilder _orderBuilder = new OrderBuilder();


        private ObservableCollection<OrderItemInputModel> _orderItems;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Publisher> _publisherRepository;

        private List<Publisher> _publishers;
        private readonly IRegionManager _regionManager;

        private string _searchWord;


        private ComicBook _selectedComicBook;


        private OrderItemInputModel _selectedOrderItem;


        private Publisher _selectedPublisher;

        private double _totalPrice;
        private readonly User _user;


        public AddOrderViewModel(IRegionManager manager, IRepository<ComicBook> comicBookRepository,
            IRepository<Publisher> publisherRepository, IRepository<Order> orderRepository, User[] users)
        {
            _regionManager = manager;
            _comicBookRepository = comicBookRepository;
            _publisherRepository = publisherRepository;
            _orderRepository = orderRepository;
            _user = users[0];

            AddItemCommand = new DelegateCommand(AddItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem);
            SaveOrderCommand = new DelegateCommand(SaveOrder);
            SelectedPublisherChangedCommand = new DelegateCommand(Search);
            SearchWordChangedCommand = new DelegateCommand(Search);
            ResetSearchCommand = new DelegateCommand(ResetSearch);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        public DelegateCommand AddItemCommand { get; set; }
        public DelegateCommand RemoveItemCommand { get; set; }
        public DelegateCommand SaveOrderCommand { get; set; }
        public DelegateCommand SelectedPublisherChangedCommand { get; set; }
        public DelegateCommand SearchWordChangedCommand { get; set; }
        public DelegateCommand ResetSearchCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public Order Order { get; private set; }

        public List<Publisher> Publishers
        {
            get => _publishers;
            set => SetProperty(ref _publishers, value);
        }

        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set => SetProperty(ref _selectedPublisher, value);
        }

        public List<ComicBook> ComicBooks
        {
            get => _comicBooks;
            set => SetProperty(ref _comicBooks, value);
        }

        public ComicBook SelectedComicBook
        {
            get => _selectedComicBook;
            set => SetProperty(ref _selectedComicBook, value);
        }

        public ObservableCollection<OrderItemInputModel> OrderItems
        {
            get => _orderItems;
            set => SetProperty(ref _orderItems, value);
        }

        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public OrderItemInputModel SelectedOrderItem
        {
            get => _selectedOrderItem;
            set => SetProperty(ref _selectedOrderItem, value);
        }

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetData();
            ResetSearch();

            CanSave = false;
        }

        public void AddItem()
        {
            if (SelectedComicBook == null || OrderItems.Any(x => x.ComicBook.Id == SelectedComicBook.Id)) return;

            var addedOrderItem = _orderBuilder
                .Item
                .ComicBook(SelectedComicBook)
                .Quantity(1)
                .Discount(0)
                .BuildItem();

            PropertyChanged += AddedOrderItem_PropertyChanged;
            var inputItem = new OrderItemInputModel(addedOrderItem);
            inputItem.PropertyChanged += AddedOrderItem_PropertyChanged;

            OrderItems.Add(inputItem);
            AddedOrderItem_PropertyChanged(null, null);
        }

        public void AddedOrderItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TotalPrice = OrderItems.Sum(x => x.Price);
            CanSave = OrderItems.All(x => !x.HasErrors) && OrderItems.Count != 0;
        }

        public void RemoveItem()
        {
            if (SelectedOrderItem == null) return;

            OrderItems.Remove(SelectedOrderItem);
            AddedOrderItem_PropertyChanged(null, null);
        }

        public void GetData()
        {
            Publishers = _publisherRepository.GetAll().ToList();

            if (!_comicBookRepository.GetAll().Any()) return;

            _allComicBooks = _comicBookRepository.GetAll().Include(x => x.Series).Include(x => x.Series.Publisher)
                .Include(x => x.ComicBookArtists).ThenInclude(z => z.Artist)
                .Where(x => x.Quantity > 0).ToList();
            ComicBooks = _allComicBooks;

            OrderItems = new ObservableCollection<OrderItemInputModel>();
        }

        public void Search()
        {
            ComicBooks = SelectedPublisher == null
                ? _allComicBooks.Where(x => x.Title.Contains(SearchWord)).ToList()
                : _allComicBooks.Where(x => x.Series.Publisher.Name == SelectedPublisher.Name)
                    .Where(x => x.Title.Contains(SearchWord)).ToList();
        }

        public void GoBack()
        {
            _regionManager.RequestNavigate("content", "ComicBookList");
        }

        public void SaveOrder()
        {
            try
            {
                foreach (var item in OrderItems)
                    if (item.Quantity <= item.ComicBook.Quantity)
                        item.ComicBook.Quantity -= item.Quantity;
                    else
                        throw new InvalidOperationException(
                            "You are trying to add an order with one of its items above the available quantity limit. Item: " +
                            item.ComicBook.Title);

                var items = new ObservableCollection<OrderItem>(OrderItems.Select(x => x.OrderItem));
                Order = _orderBuilder
                    .Details
                    .Date(DateTime.Now)
                    .User(_user)
                    .OrderItemList(items)
                    .Build();

                _orderRepository.Add(Order);
                GoBack();
            }
            catch (InvalidOperationException ex)
            {
                CanSave = false;
            }
        }

        public void ResetSearch()
        {
            SearchWord = string.Empty;
            ComicBooks = _allComicBooks;
        }
    }
}