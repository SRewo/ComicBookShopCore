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
        private IRepository<Series> _seriesRepository;
        private IRepository<Publisher> _publisherRepository;
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


        private bool _canEdit;

        public bool CanEdit
        {
            get => _canEdit;
            set => SetProperty(ref _canEdit, value);
        }


        public SeriesListViewModel(IRegionManager manager)
        {

            SearchWordChanged = new DelegateCommand(SearchByWord);
            SelectedPublisherChanged = new DelegateCommand(SearchByPublisher);
            ResetSearchCommand = new DelegateCommand(ResetSearch);
            AddSeriesCommand = new DelegateCommand(OpenAdd);
            EditSeriesCommand = new DelegateCommand(OpenEdit);

            _regionManager = manager;
        }

        private void SearchByWord()
        {

            if (SelectedPublisher == null)
            {
                var series = _allSeries.Where(c => c.Name.ToLower().Contains(SearchWord.Trim().ToLower()));

                ViewList = series.ToList();
            }
            else
            {
                var series = _allSeries.Where(c => c.Name.ToLower().Contains(SearchWord.Trim().ToLower()) && c.Publisher.Id.Equals(SelectedPublisher.Id));

                ViewList = series.ToList();
            }

        }

        private void SearchByPublisher()
        {

            if (string.IsNullOrEmpty(SearchWord) && SelectedPublisher != null)
            {

                var series = _allSeries.Where(c => c.Publisher.Id.Equals(SelectedPublisher.Id));

                ViewList = series.ToList();

            }
            else if(SelectedPublisher != null)
            {

                var series = _allSeries.Where(c => c.Name.ToLower().Contains(SearchWord.Trim().ToLower()) && c.Publisher.Id.Equals(SelectedPublisher.Id));

                ViewList = series.ToList();

            }

        }

        private void ResetSearch()
        {

            SearchWord = string.Empty;
            SelectedPublisher = null;
            ViewList = _allSeries.ToList();

        }

        private void OpenAdd()
        {

            _regionManager.RequestNavigate("content","AddEditSeries");

        }

        private void OpenEdit()
        {

                var parameters = new NavigationParameters()
                {
                    {"series", SelectedSeries }
                };

                _regionManager.RequestNavigate("content","AddEditSeries",parameters);

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetTable();
            CanEdit = false;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private async void GetTable()
        {
            using (var context = new ShopDbEntities())
            {

                _seriesRepository = new SqlRepository<Series>(context);
                _allSeries = await _seriesRepository.GetAll().Include(m => m.Publisher).ToListAsync();
                ViewList = _allSeries;

                _publisherRepository = new SqlRepository<Publisher>(context);
                Publishers = await _publisherRepository.GetAll().ToListAsync();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            CanEdit = SelectedSeries != null;
        }
    }
}
