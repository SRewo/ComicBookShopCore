using System;
using System.Collections.Generic;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class AddEditSeriesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<Publisher> _publisherRepository;
        private IRepository<Series> _seriesRepository;
        public DelegateCommand SaveSeriesCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }

        private List<Publisher> _publishers;

        public List<Publisher> Publishers
        {
            get => _publishers;
            set => SetProperty(ref _publishers, value);
        }

        private Series _series;

        public Series Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private string _nameErrorMessage;

        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
        }

        private bool _canSave;

        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

    public AddEditSeriesViewModel(IRegionManager manager, IRepository<Publisher> publisherRepostory, IRepository<Series> seriesRepository)
        {

            _regionManager = manager;
            _publisherRepository = publisherRepostory;
            _seriesRepository = seriesRepository;

            GoBackCommand = new DelegateCommand(GoBack);
            SaveSeriesCommand = new DelegateCommand(SaveSeries);

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

            ResetForm();

            Series = (Series)navigationContext.Parameters["series"];
            Series ??= new Series();

            GetPublishersFromRepository();

            Series.PropertyChanged += CanSaveChanged;
            Series.ErrorsChanged += Series_ErrorsChanged;
        }

        public void Series_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            NameErrorMessage = Series.GetFirstError("Name");
        }

        public void CanSaveChanged(object sender, EventArgs e)
        {

            CanSave = (!string.IsNullOrEmpty(Series.Name) && Series.Publisher != null) ? !Series.HasErrors : false;
            
        }

        public void ResetForm()
        {
            Series = null;
            CanSave = false;
            NameErrorMessage = string.Empty;
            Publishers = null;
        }

        private void GoBack()
        {
            _seriesRepository.Reload(Series);
            _regionManager.RequestNavigate("content","SeriesList");

        }

        public void GetPublishersFromRepository()
        {
            Publishers = _publisherRepository.GetAll().ToList();
        }

        private void SaveSeries()
        {

            if(Series.Id <= 0)
            {
                _seriesRepository.Add(Series);
            }
            else
            {
                _seriesRepository.Update(Series);
            }


            GoBack();

        }
    }
}
