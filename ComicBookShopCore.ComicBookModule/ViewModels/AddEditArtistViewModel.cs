using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.ComponentModel;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class AddEditArtistViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IRepository<Artist> _artistRepository;
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand SaveArtistCommand { get; private set; }

        private Artist _artist;

        public Artist Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private string _firstNameErrorMessage;

        public string FirstNameErrorMessage
        {
            get => _firstNameErrorMessage;
            set => SetProperty(ref _firstNameErrorMessage, value);
        }

        private string _lastNameErrorMessage;

        public string LastNameErrorMessage
        {
            get => _lastNameErrorMessage;
            set => SetProperty(ref _lastNameErrorMessage, value);
        }

        public AddEditArtistViewModel(IRegionManager manager, IRepository<Artist> artistRepository)
        {

            _regionManager = manager;
            GoBackCommand = new DelegateCommand(GoBack);
            SaveArtistCommand = new DelegateCommand(SaveArtist);
            _artistRepository = artistRepository;
        }

        public void ArtistOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            CanSave = !string.IsNullOrEmpty(Artist.FirstName) && !string.IsNullOrEmpty(Artist.LastName) ? !Artist.HasErrors : false;

        }

        private void GoBack()
        {
            _artistRepository.Reload(Artist);
            _regionManager.RequestNavigate("content","ArtistList");

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

            Artist = null;
            FirstNameErrorMessage = string.Empty;
            LastNameErrorMessage = string.Empty;
            CanSave = false;

            Artist = (Artist) navigationContext.Parameters["Artist"];
            Artist ??= new Artist();

            Artist.PropertyChanged += ArtistOnPropertyChanged;
            Artist.ErrorsChanged += Artist_ErrorsChanged;
        }

        public void Artist_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {

            FirstNameErrorMessage = Artist.GetFirstError("FirstName");
            LastNameErrorMessage = Artist.GetFirstError("LastName");

        }

        private void SaveArtist()
        {
            if (Artist.Id <= 0)
            {
                _artistRepository.Add(Artist);
            }
            else
            {
                _artistRepository.Update(Artist);
            }

            GoBack();

        }
    }
}
