using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class ComicBookInputModel : ValidableBase
    {
        private ObservableCollection<InputComicBookArtist> _comicBookArtists;

        private string _description;

        private DateTime _onSaleDate;

        private double _price;

        private int _quantity;

        private Series _series;

        private string _shortDescription;
        private string _title;

        public ComicBookInputModel()
        {
            OnSaleDate = DateTime.Today;
            ComicBookArtists = new ObservableCollection<InputComicBookArtist>();
        }

        [Required]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DateTime OnSaleDate
        {
            get => _onSaleDate;
            set => SetProperty(ref _onSaleDate, value);
        }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid value.")]
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        [Required]
        public Series Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public ObservableCollection<InputComicBookArtist> ComicBookArtists
        {
            get => _comicBookArtists;
            set => SetProperty(ref _comicBookArtists, value);
        }

        [MaxLength(120)]
        public string ShortDescription
        {
            get => _shortDescription;
            set => SetProperty(ref _shortDescription, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }


    public class InputComicBookArtist : ValidableBase
    {
        public InputComicBookArtist(ComicBookArtist comicBookArtist)
        {
            ComicBookArtist = comicBookArtist;
        }

        public ComicBookArtist ComicBookArtist { get; }
        public int Id => ComicBookArtist.Id;

        public Artist Artist
        {
            get => ComicBookArtist.Artist;
            set => ComicBookArtist.Artist = SetArtist(value);
        }

        public string Type
        {
            get => ComicBookArtist.Type;
            set => ComicBookArtist.Type = SetType(value);
        }

        private string SetType(string value)
        {
            RaisePropertyChanged("Type");
            return value;
        }

        private Artist SetArtist(Artist value)
        {
            RaisePropertyChanged("Artist");
            return value;
        }
    }


    public class AddEditComicBookViewModel : BindableBase, INavigationAware
    {
        private readonly IRepository<Artist> _artistRepository;

        private List<Artist> _artists;

        private bool _canSave;

        private ComicBook _comicBook;
        private ComicBookBuilder _comicBookBuilder;
        private readonly IRepository<ComicBook> _comicBookRepository;

        private ComicBookInputModel _inputModel;

        private string _priceErrorMessage;

        private string _quantityErrorMessage;
        private readonly IRegionManager _regionManager;

        private Artist _selectedArtist;

        private InputComicBookArtist _selectedComicBookArtist;

        private List<Series> _seriesList;
        private readonly IRepository<Series> _seriesRepository;

        private string _titleErrorMessage;


        public AddEditComicBookViewModel(IRegionManager manager, IRepository<ComicBook> comicBookRepository,
            IRepository<Artist> artistRepository, IRepository<Series> seriesRepository
            )
        {
            _comicBookRepository = comicBookRepository;
            _artistRepository = artistRepository;
            _seriesRepository = seriesRepository;

            _regionManager = manager;

            InputModel = new ComicBookInputModel();

            AddArtistCommand = new DelegateCommand(AddArtist);
            RemoveArtistCommand = new DelegateCommand(RemoveArtist);
            GoBackCommand = new DelegateCommand(GoBack);
            SaveComicBookCommand = new DelegateCommand(() => SaveComicBookAsync());

            CanSave = false;
        }

        public DelegateCommand AddArtistCommand { get; set; }
        public DelegateCommand RemoveArtistCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SaveComicBookCommand { get; set; }
        public bool IsEditing { get; private set; }

        public ComicBookInputModel InputModel
        {
            get => _inputModel;
            set => SetProperty(ref _inputModel, value);
        }

        public ComicBook ComicBook
        {
            get => _comicBook;
            set => SetProperty(ref _comicBook, value);
        }

        public List<Artist> Artists
        {
            get => _artists;
            set => SetProperty(ref _artists, value);
        }

        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set => SetProperty(ref _selectedArtist, value);
        }

        public InputComicBookArtist SelectedComicBookArtist
        {
            get => _selectedComicBookArtist;
            set => SetProperty(ref _selectedComicBookArtist, value);
        }

        public List<Series> SeriesList
        {
            get => _seriesList;
            set => SetProperty(ref _seriesList, value);
        }

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        public string TitleErrorMessage
        {
            get => _titleErrorMessage;
            set => SetProperty(ref _titleErrorMessage, value);
        }

        public string PriceErrorMessage
        {
            get => _priceErrorMessage;
            set => SetProperty(ref _priceErrorMessage, value);
        }

        public string QuantityErrorMessage
        {
            get => _quantityErrorMessage;
            set => SetProperty(ref _quantityErrorMessage, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            ComicBook = (ComicBook) navigationContext.Parameters["ComicBook"];

            await GetDataAsync().ConfigureAwait(true);

            CheckPassedComicBookAsync(ComicBook);

            SetErrorMessagesChangesAsync();
        }

        private void AddArtist()
        {
            if (SelectedArtist == null ||
                InputModel.ComicBookArtists.Any(x => x.Artist.Id.Equals(SelectedArtist.Id))) return;

            var artist = _comicBookBuilder.AddArtist.Artist(SelectedArtist).Role("role").BuildComicBookArtist();
            var inputArtist = new InputComicBookArtist(artist);
            inputArtist.PropertyChanged += InputModel_PropertyChanged;

            InputModel.ComicBookArtists.Add(inputArtist);

            InputModel_PropertyChanged(null, null);
        }

        private void RemoveArtist()
        {
            if (SelectedComicBookArtist == null) return;
            InputModel.ComicBookArtists.Remove(SelectedComicBookArtist);
            InputModel_PropertyChanged(null, null);
        }

        private Task SaveComicBookAsync()
        {
            if (!CanSave) return Task.CompletedTask;

            if (IsEditing)
            {
                ComicBook.Title = InputModel.Title;
                ComicBook.Series = InputModel.Series;
                ComicBook.Description = InputModel.Description;
                ComicBook.Quantity = InputModel.Quantity;
                ComicBook.Price = InputModel.Price;
                ComicBook.ShortDescription = InputModel.ShortDescription;

                foreach (var artist in InputModel.ComicBookArtists)
                    if (!artist.HasErrors)
                    {
                        AddOrEditArtistAsync(artist.ComicBookArtist);
                    }
                    else
                    {
                        MessageBox.Show(artist.GetFirstError());
                        return Task.CompletedTask;
                    }


                foreach (var artist in ComicBook.ComicBookArtists.ToList()) CheckAndRemoveArtistsAsync(artist);

                ComicBook.Validate();

                if (ComicBook.HasErrors)
                {
                    MessageBox.Show(ComicBook.GetFirstError());
                    return Task.CompletedTask;
                }

                _comicBookRepository.Update(ComicBook);
                GoBack();
                return Task.CompletedTask;
            }


            try
            {
                var artists = InputModel.ComicBookArtists.Select(x => x.ComicBookArtist);
                var obsArtist = new ObservableCollection<ComicBookArtist>(artists);
                ComicBook = _comicBookBuilder.Details.ArtistList(obsArtist).Title(InputModel.Title).Series(InputModel.Series).OnSaleDate(InputModel.OnSaleDate).Status.Price(InputModel.Price).Quantity(InputModel.Quantity).Description.ShortDesc(InputModel.ShortDescription).LongDesc(InputModel.Description).Build();
                _comicBookRepository.Add(ComicBook);
            }
            catch (ValidationException e)
            {
                MessageBox.Show(e.Message);
                return Task.CompletedTask;
            }

            GoBack();
            return Task.CompletedTask;
        }


        private void GoBack()
        {
            _regionManager.RequestNavigate("content", "ComicBookList");
        }


        public virtual Task SetErrorMessagesChangesAsync()
        {
            InputModel.PropertyChanged += InputModel_PropertyChanged;
            return Task.CompletedTask;
        }

        private void InputModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TitleErrorMessage = InputModel.GetFirstError("Title");
            PriceErrorMessage = InputModel.GetFirstError("Price");
            QuantityErrorMessage = InputModel.GetFirstError("Quantity");
            CanSave = !InputModel.HasErrors;
            if ( string.IsNullOrEmpty(InputModel.Title) || !(InputModel.Price > 0) || InputModel.Series == null ||
                !InputModel.ComicBookArtists.Any())
                CanSave = false;
        }


        public virtual Task GetDataAsync()
        {
            Artists =  _artistRepository.GetAll().ToList();
            SeriesList =  _seriesRepository.GetAll().ToList();
            return Task.CompletedTask;
        }

        public virtual Task CheckPassedComicBookAsync(ComicBook comic)
        {
            InputModel = new ComicBookInputModel();
            IsEditing = comic != null;
            _comicBookBuilder = new ComicBookBuilder();

            if (!IsEditing) return Task.CompletedTask;

            InputModel.Title = comic.Title;
            InputModel.Series = comic.Series;
            InputModel.Quantity = comic.Quantity;
            InputModel.Price = comic.Price;
            InputModel.Description = comic.Description;
            InputModel.ShortDescription = comic.ShortDescription;

            foreach (var c in comic.ComicBookArtists)
            {
                var artist = _comicBookBuilder.AddArtist.Artist(c.Artist).Role(c.Type).BuildComicBookArtist();
                var inputArtist = new InputComicBookArtist(artist);
                inputArtist.PropertyChanged += InputModel_PropertyChanged;
                InputModel.ComicBookArtists.Add(inputArtist);
            }

            ComicBook = comic;

            return Task.CompletedTask;
        }

        public Task AddOrEditArtistAsync(ComicBookArtist artist)
        {
            if(artist == null) return Task.CompletedTask;
            if (ComicBook.ComicBookArtists.Any(x =>  x.Artist.FirstName == artist.Artist.FirstName && x.Artist.LastName == artist.Artist.LastName))
            {
                var comicArtist =
                    ComicBook.ComicBookArtists.Single(x => x.Artist.FirstName == artist.Artist.FirstName && x.Artist.LastName == artist.Artist.LastName);
                if (comicArtist.Type != artist.Type) comicArtist.Type = artist.Type;

                return Task.CompletedTask;
            }

            ComicBook.ComicBookArtists.Add(artist);

            return Task.CompletedTask;
        }

        public Task CheckAndRemoveArtistsAsync(ComicBookArtist artist)
        {
            if (!InputModel.ComicBookArtists.Any(x => x.Artist.FirstName == artist.Artist.FirstName && x.Artist.LastName == artist.Artist.LastName))
                ComicBook.ComicBookArtists.Remove(artist);

            return Task.CompletedTask;
        }
    }
}