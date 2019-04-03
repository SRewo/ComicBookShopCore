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

        private bool _isAddingSeries;

        public bool IsAddingSeries
        {
            get => _isAddingSeries;
            set => SetProperty(ref _isAddingSeries, value);
        }

    public AddEditSeriesViewModel(IRegionManager manager)
        {

            _regionManager = manager;

            GoBackCommand = new DelegateCommand(GoBack);
            SaveSeriesCommand = new DelegateCommand(SaveSeries);


            using (var context = new ShopDbEntities())
            {
                
                _publisherRepository = new SqlRepository<Publisher>(context);
                Publishers = _publisherRepository.GetAll().ToList();

            }

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
            Series = null;
            CanSave = false;
            NameErrorMessage = string.Empty;
            Series = (Series) navigationContext.Parameters["series"];
            if (Series == null)
            {
           
                Series = new Series();
                IsAddingSeries = true;

            }
            else
            {
                IsAddingSeries = false;
            }


            Series.PropertyChanged += CanSaveChanged;
            Series.ErrorsChanged += Series_ErrorsChanged;
        }

        private void Series_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            NameErrorMessage = Series.GetFirstError("Name");
        }

        private void CanSaveChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Series.Name) && Series.Publisher != null)
                CanSave = !Series.HasErrors;
            else
                CanSave = false;
            
        }

        private void GoBack()
        {

            _regionManager.RequestNavigate("content","SeriesList");

        }

        private void SaveSeries()
        {

            using (var context = new ShopDbEntities())
            {
                
                _seriesRepository = new SqlRepository<Series>(context);
                context.Publishers.Attach(Series.Publisher);
                if(Series.Id == 0)
                {
                    _seriesRepository.Add(Series);
                }
                else
                {
                    _seriesRepository.Update(Series);
                }
                context.SaveChanges();

            }

            _regionManager.RequestNavigate("content","SeriesList");

        }
    }
}
