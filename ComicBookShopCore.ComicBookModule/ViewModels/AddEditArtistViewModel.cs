using System;
using System.CodeDom;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using ComicBookShopCore.Data.Builders;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class InputModel : ValidableBase
    {
        private string _firstName;
        [Required(ErrorMessage = "First name cannot be empty.")]
        [CustomValidation.NameValidation(ErrorMessage = "First Name cannot contain special characters.")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        [Required(ErrorMessage = "Last name cannot be empty.")]
        [CustomValidation.NameValidation(ErrorMessage = "Last Name cannot contain special characters.")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }


    public class AddEditArtistViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<Artist> _artistRepository;
        private ArtistBuilder _artistBuilder;
        private bool _isEditing;
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand SaveArtistCommand { get; private set; }

        public Artist Artist { get; private set; }
        public InputModel InputModel { get; private set; }

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

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        public AddEditArtistViewModel(IRegionManager manager, IRepository<Artist> artistRepository)
        {

            _regionManager = manager;
            GoBackCommand = new DelegateCommand(GoBack);
            SaveArtistCommand = new DelegateCommand((() => SaveArtistAsync()));
            _artistRepository = artistRepository;
            InputModel = new InputModel();
        }

        private void AddEditArtistViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            if (InputModel.HasErrors)
            {
                CanSave = false;
                FirstNameErrorMessage = InputModel.GetFirstError("FirstName");
                LastNameErrorMessage = InputModel.GetFirstError("LastName");
            }
            else if (Artist != null && (Artist.FirstName == InputModel.FirstName && Artist.LastName == InputModel.LastName && Artist.Description == InputModel.Description))
            {
                CanSave = false;
                FirstNameErrorMessage = InputModel.GetFirstError("FirstName");
                LastNameErrorMessage = InputModel.GetFirstError("LastName");
            }
            else
            {
                CanSave = true;
                FirstNameErrorMessage = string.Empty;
                LastNameErrorMessage = string.Empty;
            }

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

        public virtual Task ResetFormAsync()
        {
            CanSave = false;
            Artist = null;
            FirstNameErrorMessage = string.Empty;
            LastNameErrorMessage = string.Empty;

            return Task.CompletedTask;
        }

        public virtual Task SetErrorMessageChangesAsync()
        {
            InputModel.ErrorsChanged += AddEditArtistViewModel_ErrorsChanged;
            return Task.CompletedTask;
        }

        public virtual Task CheckPassedArtistAsync(Artist artist)
        {
            _isEditing = artist != null;

            if (!_isEditing)
            {
                _artistBuilder = new ArtistBuilder();
                return Task.CompletedTask;
            }

            Artist = artist;
            InputModel.FirstName = artist.FirstName;
            InputModel.LastName = artist.LastName;
            InputModel.Description = artist.Description;

            return Task.CompletedTask;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ResetFormAsync();

            var artist = (Artist) navigationContext.Parameters["Artist"];

            CheckPassedArtistAsync(artist);

            SetErrorMessageChangesAsync();
        }

        private Task SaveArtistAsync()
        {
            if (!_isEditing)
            {
                try
                {
                    Artist = _artistBuilder
                        .Details
                            .FirstName(InputModel.FirstName)
                            .LastName(InputModel.LastName)
                            .Description(InputModel.Description)
                        .Build();
                }
                catch (ValidationException ex)
                {
                    MessageBox.Show(ex.Message);
                    return Task.CompletedTask;
                }

                _artistRepository.Add(Artist);

            }
            else
            {
                Artist.FirstName = InputModel.FirstName;
                Artist.LastName = InputModel.LastName;
                Artist.Description = InputModel.Description;

                if (Artist.HasErrors)
                {
                    MessageBox.Show(Artist.GetFirstError());
                    return Task.CompletedTask;
                }

                _artistRepository.Update(Artist);
            }

            GoBack();
            return Task.CompletedTask;
        }

    }
}
