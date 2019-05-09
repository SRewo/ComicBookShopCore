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
using Prism.Ioc;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class ArtistListViewModel : BindableBase, INavigationAware
    {

        private readonly IRepository<Artist> _artistRepository;
        private List<Artist> _allArtists;
        private readonly IRegionManager _regionManager;

        public DelegateCommand AddArtistCommand { get; private set; }
        public DelegateCommand EditArtistCommand { get; private set; }
        public DelegateCommand SearchWordChanged { get; private set; }

        private List<Artist> _viewList;

        public List<Artist> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
        }

        private string _searchWord;

        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        private Artist _selectedArtist;

        public Artist SelectedArtist
        {
            get => _selectedArtist;
            set => SetProperty(ref _selectedArtist, value);
        }

        private bool _isSearchEnabled;

        public bool IsSearchEnabled
        {
            get => _isSearchEnabled;
            set => SetProperty(ref _isSearchEnabled, value);
        }

        private bool _isEditEnabled;

        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set => SetProperty(ref _isEditEnabled, value);
        }


        public ArtistListViewModel(IRegionManager manager, IRepository<Artist> repository)
        {

            _regionManager = manager;
            AddArtistCommand = new DelegateCommand(OpenAdd);
            EditArtistCommand = new DelegateCommand(OpenEdit);
            SearchWordChanged = new DelegateCommand(Search);
            _artistRepository = repository;

        }
            

        public void OpenAdd()
        {
            
            _regionManager.RequestNavigate("content","AddEditArtist");

        }

        public void OpenEdit()
        {
            var parameters = new NavigationParameters
            {
                { "Artist", SelectedArtist }
            };
            _regionManager.RequestNavigate("content","AddEditArtist",parameters);
        }

        public void Search()
        {

            ViewList = string.IsNullOrEmpty(SearchWord) ? _allArtists : _allArtists.Where(c => c.Name.Contains(SearchWord)).ToList();

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

            ViewList = null;
            SelectedArtist = null;
            GetTable();
            CanSearchCheck();

        }

        public void GetTable()
        {

           _allArtists = _artistRepository.GetAll().ToList();
            ViewList = _allArtists;

        }

        public void CanSearchCheck()
        {
            IsSearchEnabled = (ViewList != null);
        }

        public void CanEditCheck()
        {
            IsEditEnabled = SelectedArtist != null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            CanEditCheck();
        }
    }
}
