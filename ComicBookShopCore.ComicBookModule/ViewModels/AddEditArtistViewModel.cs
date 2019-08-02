using System.CodeDom;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using ComicBookShopCore.Data.Builders;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class AddEditArtistViewModel : ValidableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<Artist> _artistRepository;
        private ArtistBuilder _artistBuilder;
        private bool _isEditing;
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand SaveArtistCommand { get; private set; }

        private Artist _artist;

        public Artist Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        private string _firstName;
        [Required]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        [Required]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _description;
        [Required]
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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

        private void GoBack()
        {

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

            Artist = (Artist) navigationContext.Parameters["Artist"];
            _isEditing = Artist != null;

            if (!_isEditing)
            {
                _artistBuilder = new ArtistBuilder();
                return;
            }

            FirstName = Artist.FirstName;
            LastName = Artist.LastName;
            Description = Artist.Description;
        }

        public void Artist_ErrorsChanged()
        {

            FirstNameErrorMessage = Artist.GetFirstError("FirstName");
            LastNameErrorMessage = Artist.GetFirstError("LastName");

        }

        private void SaveArtist()
        {
            if (!_isEditing)
            {
                try
                {
                    Artist = _artistBuilder
                        .Details
                            .FirstName(FirstName)
                            .LastName(LastName)
                            .Description(Description)
                        .Build();
                }
                catch (ValidationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                _artistRepository.Add(Artist);

            }
            else
            {
                _artist.FirstName = FirstName;
                _artist.LastName = LastName;
                _artist.Description = Description;

                if (Artist.HasErrors)
                {
                    MessageBox.Show(Artist.GetFirstError());
                    return;
                }

                _artistRepository.Update(Artist);
            }

            GoBack();

        }
    }
}
