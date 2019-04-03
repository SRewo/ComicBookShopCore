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
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand SavePublisherCommand { get; set; }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private SqlRepository<Publisher> _publisherRepository;

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

        public AddEditPublisherViewModel(IRegionManager manager)
        {

            _regionManager = manager;
            GoBackCommand = new DelegateCommand(GoBack);
            SavePublisherCommand = new DelegateCommand(SavePublisher);

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


            Publisher = (Publisher) navigationContext.Parameters["publisher"];
            if (Publisher == null)
            {
                CanSave = false;
                Publisher = new Publisher()
                {
                    CreationDateTime = DateTime.Now
                };

            }

            Publisher.PropertyChanged += CanExecuteChanged;
            Publisher.ErrorsChanged += Publisher_ErrorsChanged;
        }

        private void Publisher_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {

            NameErrorMessage = Publisher.GetFirstError("Name");
            DateErrorMessage = Publisher.GetFirstError("CreationDateTime");
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {

            if(!string.IsNullOrEmpty(Publisher.Name))
                CanSave = !Publisher.HasErrors;
            else
                CanSave = false;

        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("content","PublisherList");
        }

        private void SavePublisher()
        {
            if (Publisher != null)
            {
                using (var context = new ShopDbEntities())
                {

                    _publisherRepository = new SqlRepository<Publisher>(context);
                    if (Publisher.Id == 0)
                    {
                        _publisherRepository.Add(Publisher);
                    }
                    else
                    {
                        _publisherRepository.Update(Publisher);
                    }
                    context.SaveChanges();

                }

                _regionManager.RequestNavigate("content", "PublisherList");

            }

        }
    }
}
