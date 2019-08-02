using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class AddEditComicBookViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private IRepository<ComicBook> _comicBookRepository;
        private IRepository<Artist> _artistRepository;
        private IRepository<Series> _seriesRepository;
        private IRepository<ComicBookArtist> _comicBookArtistRepository;

        public DelegateCommand AddArtistCommand { get; set; }
        public DelegateCommand RemoveArtistCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SaveComicBookCommand { get; set; }

        private ComicBook _comicBook;

        public ComicBook ComicBook
        {
            get => _comicBook;
            set => SetProperty(ref _comicBook, value);
        }

        private List<Artist> _artists;

        public List<Artist> Artists
        {
            get => _artists;
            set => SetProperty(ref _artists, value);
        }

        private Artist _selectedArtist;

        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set => SetProperty(ref _selectedArtist, value);
        }

        private ComicBookArtist _selectedComicBookArtist;

        public ComicBookArtist SelectedComicBookArtist
        {
            get => _selectedComicBookArtist;
            set => SetProperty(ref _selectedComicBookArtist, value);
        }

        private List<Series> _seriesList;

        public List<Series> SeriesList
        {
            get => _seriesList;
            set => SetProperty(ref _seriesList, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private string _titleErrorMessage;

        public string TitleErrorMessage
        {
            get => _titleErrorMessage;
            set => SetProperty(ref _titleErrorMessage, value);
        }

        private string _priceErrorMessage;

        public string PriceErrorMessage
        {
            get => _priceErrorMessage;
            set => SetProperty(ref _priceErrorMessage, value);
        }

        private string _quantityErrorMessage;

        public string QuantityErrorMessage
        {
            get => _quantityErrorMessage;
            set => SetProperty(ref _quantityErrorMessage, value);
        }




        public AddEditComicBookViewModel(IRegionManager manager, IRepository<ComicBook> comicBookRepository, IRepository<Artist> artistRepository, IRepository<Series> seriesRepository, IRepository<ComicBookArtist> comicBookArtistRepository)
        {

            _comicBookRepository = comicBookRepository;
            _artistRepository = artistRepository;
            _seriesRepository = seriesRepository;
            _comicBookArtistRepository = comicBookArtistRepository;

            _regionManager = manager;

            AddArtistCommand = new DelegateCommand(AddArtist);
            RemoveArtistCommand = new DelegateCommand(RemoveArtist);
            GoBackCommand = new DelegateCommand(GoBack);
            SaveComicBookCommand = new DelegateCommand(SaveComicBook);

            CanSave = false;

        }

        public void ComicBook_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(ComicBook.Title) && ComicBook.Price > 0 && ComicBook.Series != null && ComicBook.ComicBookArtists.Count() != 0)
                CanSave = !ComicBook.HasErrors;
            else
                CanSave = false;

        }

        private void AddArtist()
        {

            if (SelectedArtist != null && !ComicBook.ComicBookArtists.Any(x => x.Artist.Id.Equals(SelectedArtist.Id)))
            {

                //ComicBook.ComicBookArtists.Add(new ComicBookArtist()
                //{
                //    Artist = SelectedArtist,
                //    Type = String.Empty,
                //    ComicBook = ComicBook
                //});

                ComicBook_PropertyChanged(null, null);

            }

        }

        private void RemoveArtist()
        {
            if (SelectedComicBookArtist != null)
            {
                ComicBook.ComicBookArtists.Remove(SelectedComicBookArtist);
                ComicBook_PropertyChanged(null, null);

            }
        }

        private void SaveComicBook()
        {

            if (ComicBook.Id <= 0)
            {
                _comicBookRepository.Add(ComicBook);
            }
            else
            {
                _comicBookRepository.Update(ComicBook);
            }

            _regionManager.RequestNavigate("content", "ComicBookList");

        }


        private void GoBack()
        {

            _comicBookRepository.Reload(ComicBook);
            foreach(var comArtist in ComicBook.ComicBookArtists)
            {
                _comicBookArtistRepository.Reload(comArtist);
            }
            _regionManager.RequestNavigate("content", "ComicBookList");

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

            GetComicBook(navigationContext);

            GetData();
            ResetErrorMessages();

            /*ComicBook.PropertyChanged += ComicBook_PropertyChanged*/;

            foreach (var comicBookArtist in ComicBook.ComicBookArtists)
            {

                /*comicBookArtist.PropertyChanged += ComicBook_PropertyChanged*/;

            }

            ComicBook.ErrorsChanged += ComicBook_ErrorsChanged;
        }

        public void ComicBook_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            TitleErrorMessage = ComicBook.GetFirstError("Title");
            PriceErrorMessage = ComicBook.GetFirstError("Price");
            QuantityErrorMessage = ComicBook.GetFirstError("Quantity");
        }

        public void ResetErrorMessages()
        {
            PriceErrorMessage = string.Empty;
            QuantityErrorMessage = string.Empty;
            TitleErrorMessage = string.Empty;
        }

        public void GetData()
        {
            Artists = _artistRepository.GetAll().ToList();

            SeriesList = _seriesRepository.GetAll().ToList();
        }

        public void GetComicBook(NavigationContext navigationContext)
        {
            if(navigationContext != null)
                ComicBook = (ComicBook)navigationContext.Parameters["ComicBook"];

            CanSave = false;

            //ComicBook = ComicBook ?? new ComicBook()
            //{
            //    OnSaleDate = DateTime.Now,
            //    ComicBookArtists = new ObservableCollection<ComicBookArtist>()
            //};
        }
    }
}
