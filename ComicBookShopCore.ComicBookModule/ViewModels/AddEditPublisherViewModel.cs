using System;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class AddEditPublisherViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private IRepository<Publisher> _publisherRepository;
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SavePublisherCommand { get; set; }

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
            SavePublisherCommand = new DelegateCommand(SavePublisher);

            _regionManager = manager;
            _publisherRepository = repository;

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

            Publisher = null;
            NameErrorMessage = string.Empty;
            DateErrorMessage = string.Empty;
            CanSave = false;


            Publisher = (Publisher)navigationContext.Parameters["publisher"];
            Publisher ??= new Publisher()
            {
                CreationDateTime = DateTime.Now
            };

            Publisher.PropertyChanged += CanExecuteChanged;
            Publisher.ErrorsChanged += Publisher_ErrorsChanged;
        }

        public void Publisher_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {

            NameErrorMessage = Publisher.GetFirstError("Name");
            DateErrorMessage = Publisher.GetFirstError("CreationDateTime");

        }

        public void CanExecuteChanged(object sender, EventArgs e)
        {

            if(!string.IsNullOrEmpty(Publisher.Name) && Publisher.CreationDateTime != DateTime.MinValue)
                CanSave = !Publisher.HasErrors;
            else
                CanSave = false;

        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("content","PublisherList");
        }

        public void SavePublisher()
        {
            if (Publisher != null)
            {

                if (Publisher.Id == 0)
                {
                    _publisherRepository.Add(Publisher);
                }
                else
                {
                    _publisherRepository.Update(Publisher);
                }

            }

            GoBack();

        }
    }
}
