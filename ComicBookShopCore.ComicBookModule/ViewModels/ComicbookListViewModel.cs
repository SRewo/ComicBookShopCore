using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class ComicBookListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IRepository<ComicBook> _comicBookRepository;
        private IRepository<Publisher> _publisherRepository;
        private List<ComicBook> _allComicBooks;
        public DelegateCommand SelectedPublisherChanged { get; private set; }
        public DelegateCommand SearchWordChanged { get; private set; }
        public DelegateCommand ResetSearchCommand { get; private set; }
        public DelegateCommand AddComicBookCommand { get; private set; }
        public DelegateCommand EditComicBookCommand { get; private set; }

        private List<ComicBook> _viewList;

        public List<ComicBook> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
        }

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

        private string _searchWord;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        private ComicBook _selectedComicBook;

        public ComicBook SelectedComicBook
        {
            get => _selectedComicBook;
            set => SetProperty(ref _selectedComicBook, value);
        }

        private bool _isEditEnabled;

        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set => SetProperty(ref _isEditEnabled, value);
        }

        private bool _isSearchEnabled;

        public bool IsSearchEnabled
        {
            get => _isSearchEnabled;
            set => SetProperty(ref _isSearchEnabled, value);
        }



        public ComicBookListViewModel(IRegionManager manager)
        {

            SelectedPublisherChanged = new DelegateCommand(PublisherChanged);
            SearchWordChanged = new DelegateCommand(Search);
            ResetSearchCommand = new DelegateCommand(ResetSearch);
            AddComicBookCommand = new DelegateCommand(OpenAdd);
            EditComicBookCommand = new DelegateCommand(OpenEdit);

            _regionManager = manager;

        }

        private void PublisherChanged()
        {
            if (string.IsNullOrEmpty(SearchWord))
            {

                ViewList = _allComicBooks.Where(x => x.Series.Publisher.Equals(SelectedPublisher)).ToList();

            }
            else
            {

                ViewList = _allComicBooks.Where(x =>
                    x.Series.Publisher.Equals(SelectedPublisher) && ( CheckStringEquals(x.Title,SearchWord) || CheckStringEquals(x.Series.Name,SearchWord)|| x.ComicBookArtists.Any(z => CheckStringEquals(z.Artist.Name,SearchWord)))).ToList();

            }

        }

        private void Search()
        {

           if (SelectedPublisher == null)
           {

               ViewList = _allComicBooks.Where(x =>
                   CheckStringEquals(x.Title, SearchWord) || CheckStringEquals(x.Series.Name, SearchWord) || x.ComicBookArtists.Any(z => CheckStringEquals(z.Artist.Name, SearchWord))).ToList();

           }
           else
           {

               ViewList = _allComicBooks.Where(x =>
                   x.Series.Publisher.Equals(SelectedPublisher) && (CheckStringEquals(x.Title, SearchWord) || CheckStringEquals(x.Series.Name, SearchWord) || x.ComicBookArtists.Any(z => CheckStringEquals(z.Artist.Name, SearchWord)))).ToList();

           }
            
        }

        private void ResetSearch()
        {

                SearchWord = String.Empty;
                SelectedPublisher = null;
                ViewList = _allComicBooks.ToList();

        }

        private void OpenAdd()
        {

            _regionManager.RequestNavigate("content", "AddEditComicBook");

        }

        private void OpenEdit()
        {

            var parameters = new NavigationParameters()
            {
                { "ComicBook",SelectedComicBook}
            };
            _regionManager.RequestNavigate("content","AddEditComicBook",parameters);

        }

        private static bool CheckStringEquals(string first, string second)
        {
            return first.IndexOf(second, StringComparison.OrdinalIgnoreCase) != -1;
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
            SelectedComicBook = null;
            ViewList = null;
            GetData();
            CanSearchCheck();
        }

        public void GetData()
        {
            using (var context = new ShopDbEntities())
            {

                _comicBookRepository = new SqlRepository<ComicBook>(context);
                if (_comicBookRepository.GetAll().Count() != 0)
                {
                    _allComicBooks = _comicBookRepository.GetAll().Include(x => x.ComicBookArtists).Include(x => x.Series).Include(x => x.Series.Publisher).Include(x => x.ComicBookArtists.Select(z => z.Artist)).ToList();
                }
                ViewList = _allComicBooks;

                _publisherRepository = new SqlRepository<Publisher>(context);
                Publishers = _publisherRepository.GetAll().ToList();

            }
        }

        public void CanSearchCheck()
        {
            IsSearchEnabled = ViewList != null;
        }

        public void CanEditCheck()
        {
            IsEditEnabled = SelectedComicBook != null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            CanEditCheck();
        }
    }
}
