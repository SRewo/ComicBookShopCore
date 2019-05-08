using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class AddOrderViewModel : BindableBase, INavigationAware
    {
        private IRepository<Publisher> _publisherRepository;
        private IRepository<ComicBook> _comicBookRepository;
        private IRegionManager _regionManager;
        private IRepository<Order> _orderRepository;
        private List<ComicBook> _allComicBooks;

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


        private Order _order;

        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
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





        public AddOrderViewModel(IRegionManager manager, IRepository<ComicBook> comicBookrepository, IRepository<Publisher> publisherRepository, IRepository<Order> orderRepository)
        {

            _regionManager = manager;
            _comicBookRepository = comicBookrepository;
            _publisherRepository = publisherRepository;
            _orderRepository = orderRepository;

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
            if (SelectedComicBook != null && Order.OrderItems.All(x => x.ComicBook.Id != SelectedComicBook.Id))
            {
                OrderItem addedOrderItem = new OrderItem()
                {
                    ComicBook = SelectedComicBook,
                    Discount = 0,
                    Quantity = 1
                };

                addedOrderItem.PropertyChanged += AddedOrderItem_PropertyChanged;

                Order.OrderItems.Add(addedOrderItem);
                AddedOrderItem_PropertyChanged(null,null);
            }
        }

        public void AddedOrderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Order.TotalPrice = Order.OrderItems.Sum(x => x.Price);
            CanSave = Order.OrderItems.All(x => !x.HasErrors) && !Order.HasErrors && Order.OrderItems.Count != 0;
            ErrorMessage = string.Empty;
        }

        public void RemoveItem()
        {
            if (SelectedOrderItem != null)
            {
                Order.OrderItems.Remove(SelectedOrderItem);
                AddedOrderItem_PropertyChanged(null, null);
            }
        }

        public void GetData()
        {

            Publishers = _publisherRepository.GetAll().ToList();

            if (_comicBookRepository.GetAll().Count() != 0)
            {
                _allComicBooks = _comicBookRepository.GetAll().Include(x => x.Series).Include(x => x.Series.Publisher).Include(x => x.ComicBookArtists).ThenInclude(z => z.Artist)
                    .Where(x => x.Quantity > 0).ToList();
                ComicBooks = _allComicBooks;
            }

        }

        public void CreateOrder()
        {
            Order = new Order()
            {
                Employee = GlobalVariables.LoggedEmployee,
                OrderItems = new ObservableCollection<OrderItem>()
            };
            
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
                Order.Date = DateTime.Now;
                foreach (OrderItem item in Order.OrderItems)
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
                _orderRepository.Add(Order);
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
            return GlobalVariables.LoggedEmployee != null;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            GetData();
            CreateOrder();
            ResetSearch();

            CanSave = false;
        }
    }
}
