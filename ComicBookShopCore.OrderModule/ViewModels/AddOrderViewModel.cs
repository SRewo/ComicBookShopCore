using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.OrderModule.ViewModels
{
    public class AddOrderViewModel : BindableBase, INavigationAware
    {
        private SqlRepository<Publisher> _publisherRepository;
        private SqlRepository<ComicBook> _comicBookRepository;

        public DelegateCommand AddItemCommand { get; set; }
        public DelegateCommand RemoveItemCommand { get; set; }


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




        public AddOrderViewModel()
        {
            AddItemCommand = new DelegateCommand(AddItem);
            RemoveItemCommand = new DelegateCommand(RemoveItem);
        }

        private void AddItem()
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

        private void AddedOrderItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Order.TotalPrice = Order.OrderItems.Sum(x => x.Price);
            CanSave = Order.OrderItems.All(x => !x.HasErrors) && !Order.HasErrors && Order.OrderItems.Count != 0;
        }

        private void RemoveItem()
        {
            if (SelectedOrderItem != null)
            {
                Order.OrderItems.Remove(SelectedOrderItem);
                AddedOrderItem_PropertyChanged(null, null);
            }
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

            using (var context = new ShopDbEntities())
            {

                _publisherRepository = new SqlRepository<Publisher>(context);
                Publishers = _publisherRepository.GetAll().ToList();

                _comicBookRepository =new SqlRepository<ComicBook>(context);
                if (_comicBookRepository.GetAll().Count() != 0)
                {
                    ComicBooks = _comicBookRepository.GetAll().Include(x => x.Series).Include(x => x.Series.Publisher).Include(x => x.ComicBookArtists.Select(y => y.Artist))
                        .ToList();
                }
            }

            Order = new Order()
            {
                Employee = GlobalVariables.LoggedEmployee,
                OrderItems = new ObservableCollection<OrderItem>()
            };
            CanSave = false;
        }
    }
}
