using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class SeriesListViewModel : BindableBase , INavigationAware
    {
        private List<Series> _allSeries;
        private readonly IRepository<Series> _seriesRepository;
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IRegionManager _regionManager;
        public DelegateCommand EditSeriesCommand { get; set; }
        public DelegateCommand AddSeriesCommand { get; set; }
        public DelegateCommand SearchWordChanged { get; set; }
        public DelegateCommand SelectedPublisherChanged { get; set; }
        public DelegateCommand ResetSearchCommand { get; set; }   

        private string _searchWord;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }


        private List<Series> _viewList;

        public List<Series> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
        }


        private Series _selectedSeries;

        public Series SelectedSeries
        {
            get => _selectedSeries;
            set => SetProperty(ref _selectedSeries, value);
        }


        private List<Publisher> _publishers;

        public List<Publisher> Publishers
        {
            get => _publishers;
            set => SetProperty(ref _publishers, value);
        }


        private Publisher _selectedPublisher;
           
        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set => SetProperty(ref _selectedPublisher, value);
        }


        private bool _isEditEnabled;

        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set => SetProperty(ref _isEditEnabled, value);
        }


        public SeriesListViewModel(IRegionManager manager, IRepository<Publisher> publisherRepository, IRepository<Series> seriesRepository)
        {

            SearchWordChanged = new DelegateCommand(Search);
            SelectedPublisherChanged = new DelegateCommand(Search);
            ResetSearchCommand = new DelegateCommand(ResetSearch);
            AddSeriesCommand = new DelegateCommand(OpenAdd);
            EditSeriesCommand = new DelegateCommand(OpenEdit);

            _regionManager = manager;
            _publisherRepository = publisherRepository;
            _seriesRepository = seriesRepository;
        }

        public void Search()
        {

                var series = SelectedPublisher == null ? _allSeries.Where(c => c.Name.ToLower().Contains(SearchWord.Trim().ToLower())) : 
                    _allSeries.Where(c => c.Name.ToLower().Contains(SearchWord.Trim().ToLower()) && c.Publisher.Id.Equals(SelectedPublisher.Id));

                ViewList = series.ToList();

        }

        public void ResetSearch()
        {

            SearchWord = string.Empty;
            SelectedPublisher = null;
            ViewList = _allSeries;

        }

        public void OpenAdd()
        {

            _regionManager.RequestNavigate("content","AddEditSeries");

        }

        public void OpenEdit()
        {

                var parameters = new NavigationParameters()
                {
                    {"series", SelectedSeries }
                };

                _regionManager.RequestNavigate("content","AddEditSeries",parameters);

        }

        public void CanEditCheck()
        {
            IsEditEnabled = SelectedSeries != null;
        }

        public void GetSeriesData()
        {

            _allSeries = _seriesRepository.GetAll().Include(m => m.Publisher).ToList();
            ViewList = _allSeries;

        }

        public void GetPublisherData()
        {

            Publishers = _publisherRepository.GetAll().ToList();

        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            CanEditCheck();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
            GetSeriesData();
            ResetSearch();
            GetPublisherData();
            _isEditEnabled = false;

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

    }
}
