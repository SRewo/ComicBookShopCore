using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class PublisherInputModel : ValidableBase
    {
        private string _name;

        [Required(ErrorMessage = "Publisher name cannot be empty.")]
        [MinLength(3, ErrorMessage = "Publisher name is too short.")]
        [MaxLength(40, ErrorMessage = "Publisher name is too long")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _creationDateTime;

        [CustomValidation.PublisherDateValidation]
        public DateTime CreationDateTime
        {
            get => _creationDateTime;
            set => SetProperty(ref _creationDateTime, value);
        }

        public PublisherInputModel()
        {
            CreationDateTime = DateTime.Today;
        }
    }

    public class AddEditPublisherViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<Publisher> _publisherRepository;
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SavePublisherCommand { get; set; }
        public bool IsEditing { get; private set; }
        private PublisherBuilder _publisherBuilder;

        private PublisherInputModel _inputModel;

        public PublisherInputModel InputModel
        {
            get => _inputModel;
            set => SetProperty(ref _inputModel, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private Publisher _publisher;

        public Publisher Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        private string _nameErrorMessage;

        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
        }

        private string _dateErrorMessage;

        public string DateErrorMessage
        {
            get => _dateErrorMessage;
            set => SetProperty(ref _dateErrorMessage, value);
        }

        public AddEditPublisherViewModel(IRegionManager manager, IRepository<Publisher> repository)
        {

            GoBackCommand = new DelegateCommand(GoBack);
            SavePublisherCommand = new DelegateCommand((() => SavePublisherAsync()));

            _regionManager = manager;
            _publisherRepository = repository;
            InputModel = new PublisherInputModel();
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
            ResetModelAsync();

            Publisher = (Publisher)navigationContext.Parameters["publisher"];

            CheckPassedPublisherAsync(Publisher);
            SetErrorsChangedEventAsync();

        }

        public virtual Task ResetModelAsync()
        {
            CanSave = false;
            Publisher = null;

            InputModel = new PublisherInputModel();

            return Task.CompletedTask;
        }

        public virtual Task SetErrorsChangedEventAsync()
        {
            InputModel.PropertyChanged += InputModel_PropertyChanged;
            return Task.CompletedTask;
        }

        private void InputModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NameErrorMessage = InputModel.GetFirstError("Name");
            DateErrorMessage = InputModel.GetFirstError("CreationDateTime");
            CanSave = !InputModel.HasErrors;

            if (string.IsNullOrWhiteSpace(InputModel.Name))
            {
                CanSave = false;
                return;
            }

            if (IsEditing && (Publisher.Name == InputModel.Name && Publisher.Description == InputModel.Description &&
                              Publisher.CreationDateTime == InputModel.CreationDateTime))
                CanSave = false;
        }

        public virtual Task CheckPassedPublisherAsync(Publisher publisher)
        {
            IsEditing = publisher != null;

            if (IsEditing)
            {
                Publisher = publisher;
                InputModel.Name = publisher.Name;
                InputModel.Description = publisher.Description;
                InputModel.CreationDateTime = publisher.CreationDateTime;

                return Task.CompletedTask;
            }

            _publisherBuilder = new PublisherBuilder();

            return Task.CompletedTask;
        }


        private void GoBack()
        {
            _regionManager.RequestNavigate("content","PublisherList");
        }

        public Task SavePublisherAsync()
        {
            if(!CanSave) return Task.CompletedTask;

            if (IsEditing)
            {
                Publisher.Name = InputModel.Name;
                Publisher.Description = InputModel.Description;
                Publisher.CreationDateTime = InputModel.CreationDateTime;

                Publisher.Validate();

                if (Publisher.HasErrors)
                {
                    MessageBox.Show(Publisher.GetFirstError());
                    return Task.CompletedTask;
                }

                _publisherRepository.Update(Publisher);
                
                GoBack();

                return Task.CompletedTask;
            }

            try
            {
                Publisher = _publisherBuilder
                    .Details
                    .Name(InputModel.Name)
                    .Description(InputModel.Description)
                    .Created(InputModel.CreationDateTime)
                    .Build();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
                return Task.CompletedTask;
            }

            _publisherRepository.Add(Publisher);

            GoBack();

            return Task.CompletedTask;
        }
    }
}
