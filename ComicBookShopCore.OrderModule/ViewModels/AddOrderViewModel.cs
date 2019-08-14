using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unity;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class AddOrderViewModel : BindableBase, INavigationAware
    {
        private IRepository<Publisher> _publisherRepository;
        private IRepository<ComicBook> _comicBookRepository;
        private IRegionManager _regionManager;
        private IRepository<Order> _orderRepository;
        private List<ComicBook> _allComicBooks;
        private OrderBuilder _orderBuilder = new OrderBuilder();
        private User _user;

        public DelegateCommand AddItemCommand { get; set; }
        public DelegateCommand RemoveItemCommand { get; set; }
        public DelegateCommand SaveOrderCommand { get; set; }
        public DelegateCommand SelectedPublisherChangedCommand { get; set; }
        public DelegateCommand SearchWordChangedCommand { get; set; }
        public DelegateCommand ResetSearchCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }

        private List<Publisher> _publishers;

        public List<Publisher> Publishers
        {
            get => _publishers;
            set => SetProperty(ref _publishers, value);
        }


        private Publisher _selectedPublisher;

        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set => SetProperty(ref _selectedPublisher, value);
        }


        private List<ComicBook> _comicBooks;

        public List<ComicBook> ComicBooks
        {
            get => _comicBooks;
            set => SetProperty(ref _comicBooks, value);
        }


        private ComicBook _selectedComicBook;

        public ComicBook SelectedComicBook
        {
            get => _selectedComicBook;
            set => SetProperty(ref _selectedComicBook, value);
        }


        private ObservableCollection<OrderItem> _orderItems;

        public ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set => SetProperty(ref _orderItems, value);
        }

        private double _totalPrice;

        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }


        private OrderItem _selectedOrderItem;

        public OrderItem SelectedOrderItem
        {
            get => _selectedOrderItem;
            set => SetProperty(ref _selectedOrderItem, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private string _searchWord;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }





        public AddOrderViewModel(IRegionManager manager, IRepository<ComicBook> comicBookRepository, IRepository<Publisher> publisherRepository, IRepository<Order> orderRepository, User[] users)
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

        public void AddItem()
        {
            if (SelectedComicBook == null || OrderItems.Any(x => x.ComicBook.Id == SelectedComicBook.Id)) return;

            var addedOrderItem = _orderBuilder
                .Item
                    .ComicBook(SelectedComicBook)
                    .Quantity(1)
                    .Discount(0)
                .BuildItem();

            this.PropertyChanged += AddedOrderItem_PropertyChanged;

            OrderItems.Add(addedOrderItem);
            AddedOrderItem_PropertyChanged(null,null);
        }

        public void AddedOrderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TotalPrice = OrderItems.Sum(x => x.Price);
            CanSave = OrderItems.All(x => !x.HasErrors) && OrderItems.Count != 0;
            ErrorMessage = string.Empty;
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

            _allComicBooks = _comicBookRepository.GetAll().Include(x => x.Series).Include(x => x.Series.Publisher).Include(x => x.ComicBookArtists).ThenInclude(z => z.Artist)
                .Where(x => x.Quantity > 0).ToList();
            ComicBooks = _allComicBooks;

            OrderItems = new ObservableCollection<OrderItem>();
        }

        public void Search()
        {
            ComicBooks = SelectedPublisher == null ? _allComicBooks.Where(x => x.Title.Contains(SearchWord)).ToList()
                : _allComicBooks.Where(x => x.Series.Publisher.Id == SelectedPublisher.Id).Where(x => x.Title.Contains(SearchWord)).ToList();
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
                {
                    if (item.Quantity <= item.ComicBook.Quantity)
                    {
                        item.ComicBook.Quantity -= item.Quantity;
                    }
                    else
                    {
                        throw new InvalidOperationException("You are trying to add an order with one of its items above the available quantity limit. Item: " + item.ComicBook.Title);
                    }
                }
                var order = _orderBuilder
                    .Details
                    .Date(DateTime.Now)
                    .User(_user)
                    .OrderItemList(OrderItems)
                    .Build();

                _orderRepository.Add(order);
                GoBack();
            }catch(InvalidOperationException ex)
            {
                CanSave = false;
                ErrorMessage = ex.Message;
            }
        }

        public void ResetSearch()
        {
            SearchWord = string.Empty;
            ComicBooks = _allComicBooks;
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
    }
}
