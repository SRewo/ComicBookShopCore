using System.Collections.Generic;
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
    public class PublishersListViewModel : BindableBase, INavigationAware
    {
        private List<Publisher> _allPublishers;
        private SqlRepository<Publisher> _publisherRepository;
        private IRegionManager _regionManager;

        private string _searchWord;
        private List<Publisher> _viewList;
        private Publisher _selectedPublisher;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        public List<Publisher> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
        }

        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set => SetProperty(ref _selectedPublisher, value);
        }

        public DelegateCommand SearchWordChanged { get; set; }
        public DelegateCommand AddPublisherCommand { get; set; }
        public DelegateCommand EditPublisherCommand { get; set; }

        public PublishersListViewModel(IRegionManager regionManager)
        {
            
            SearchWordChanged = new DelegateCommand(Search);
            EditPublisherCommand = new DelegateCommand(OpenEdit);
            AddPublisherCommand = new DelegateCommand(OpenAdd);

            _regionManager = regionManager;

        }

        private void Search()
        {

            if (string.IsNullOrEmpty(_searchWord))
            {

                ViewList = _allPublishers;

            }
            else
            {

                var query = from publisher in _allPublishers
                    where publisher.Name.ToLower().Contains(_searchWord.ToLower())
                            select publisher;
                ViewList = query.ToList();

            }
        }

        private void OpenEdit()
        {

            if (_selectedPublisher == null)
            {

                MessageBox.Show("You have to choose publisher.");

            }
            else
            {

                var parameters = new NavigationParameters
                {
                    { "publisher", _selectedPublisher }
                };
                _regionManager.RequestNavigate("content","AddEditPublisher",parameters);

            }
        }

        private void OpenAdd()
        {
            _regionManager.RequestNavigate("content","AddEditPublisher");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            GetTable();

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
            using (var datacontext = new ShopDbEntities())
            {

                _publisherRepository = new SqlRepository<Publisher>(datacontext);
                _allPublishers = await _publisherRepository.GetAll().ToListAsync();
                ViewList = _allPublishers;
            }
        }
    }
}