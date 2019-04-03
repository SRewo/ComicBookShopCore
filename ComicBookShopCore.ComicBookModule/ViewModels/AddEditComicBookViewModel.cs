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
    class AddEditComicBookViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private IRepository<ComicBook> _comicBookRepository;
        private IRepository<Artist> _artistRepository;
        private IRepository<Series> _seriesRepository;
        private IRepository<ComicBookArtist> _comicBookArtistRepository;
        private List<ComicBookArtist> _deletedComicBookArtists;

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

        private bool _isEdited;

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




        public AddEditComicBookViewModel(IRegionManager manager)
        {

            _regionManager = manager;

            AddArtistCommand = new DelegateCommand(AddArtist);
            RemoveArtistCommand = new DelegateCommand(RemoveArtist);
            GoBackCommand = new DelegateCommand(GoBack);
            SaveComicBookCommand = new DelegateCommand(SaveComicBook);

            CanSave = false;

        }

        private void ComicBook_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(ComicBook.Title) && ComicBook.Price > 0 && ComicBook.Series != null && ComicBook.ComicBookArtists.Count() != 0)
                CanSave = !ComicBook.HasErrors;
            else
                CanSave = false;

        }

        private void AddArtist()
        {

            if ( SelectedArtist != null && !ComicBook.ComicBookArtists.Any(x => x.Artist.Id.Equals(SelectedArtist.Id)) )
            {

                ComicBook.ComicBookArtists.Add(new ComicBookArtist()
                {
                    Artist = SelectedArtist,
                    Type = String.Empty,
                    ComicBook = ComicBook
                });

                ComicBook_PropertyChanged(null, null);

            }
            
        }

        private void RemoveArtist()
        {
            if (SelectedComicBookArtist != null)
            {
                _deletedComicBookArtists.Add(SelectedComicBookArtist);
                ComicBook.ComicBookArtists.Remove(SelectedComicBookArtist);
                ComicBook_PropertyChanged(null, null);

            }
        }

        private void SaveComicBook()
        {

            if (_isEdited)
            {

                SaveComicBookArtists();

            }

            using (var context = new ShopDbEntities())
            {

                _comicBookRepository = new SqlRepository<ComicBook>(context);
                

                foreach (var comicBookArtist in ComicBook.ComicBookArtists)
                {

                    context.Artists.Attach(comicBookArtist.Artist);

                }

                context.Publishers.Attach(ComicBook.Series.Publisher);
                context.Series.Attach(ComicBook.Series);

                context.SaveChanges();
                if(ComicBook.Id == 0)
                {
                    _comicBookRepository.Add(ComicBook);
                }
                else
                {
                    _comicBookRepository.Update(ComicBook);
                }
                context.SaveChanges();
            }

            _regionManager.RequestNavigate("content","ComicBookList");

        }

        private void SaveComicBookArtists()
        {
            using (var context = new ShopDbEntities())
            {

                _comicBookArtistRepository = new SqlRepository<ComicBookArtist>(context);

                context.ComicBooks.Attach(ComicBook);

                foreach (var comicBookArtist in ComicBook.ComicBookArtists)
                {

                    context.Artists.Attach(comicBookArtist.Artist);
                    context.Entry(comicBookArtist).State = EntityState.Modified;
                    if (comicBookArtist.Id == 0)
                    {
                        _comicBookArtistRepository.Add(comicBookArtist);
                    }
                    else
                    {
                        _comicBookArtistRepository.Update(comicBookArtist);
                    }

                }

                var tmp = _comicBookArtistRepository.GetAll().ToList();

                foreach (var comicBookArtist in _deletedComicBookArtists)
                {

                    if (comicBookArtist != null && tmp.Any(x => x.Id == comicBookArtist.Id))
                    {

                        var tmpArtist = tmp.First(x => x.Id == comicBookArtist.Id);
                        _comicBookArtistRepository.Delete(tmpArtist);
                    }

                }

                context.SaveChanges();

            }
        }


        private void GoBack()
        {

            _regionManager.RequestNavigate("content","ComicBookList");

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

            ComicBook = (ComicBook) navigationContext.Parameters["ComicBook"];
            PriceErrorMessage = string.Empty;
            QuantityErrorMessage = string.Empty;
            TitleErrorMessage = string.Empty;

            _isEdited = ComicBook != null;

            CanSave = false;

            _deletedComicBookArtists = new List<ComicBookArtist>();

            ComicBook = ComicBook ?? new ComicBook()
            {
                OnSaleDate = DateTime.Now,
                ComicBookArtists =  new ObservableCollection<ComicBookArtist>()
            };

            using (var context = new ShopDbEntities())
            {

                _artistRepository = new SqlRepository<Artist>(context);
                Artists = _artistRepository.GetAll().ToList();

                _seriesRepository = new SqlRepository<Series>(context);
                SeriesList = _seriesRepository.GetAll().ToList();

            }

            ComicBook.PropertyChanged += ComicBook_PropertyChanged;

            foreach (var comicBookArtist in ComicBook.ComicBookArtists)
            {

                comicBookArtist.PropertyChanged += ComicBook_PropertyChanged;

            }

            ComicBook.ErrorsChanged += ComicBook_ErrorsChanged;
        }

        private void ComicBook_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            TitleErrorMessage = ComicBook.GetFirstError("Title");
            PriceErrorMessage = ComicBook.GetFirstError("Price");
            QuantityErrorMessage = ComicBook.GetFirstError("Quantity");
        }
    }
}
