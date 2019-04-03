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
    public class ArtistListViewModel : BindableBase, INavigationAware
    {

        private IRepository<Artist> _artistRepository;
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

        public ArtistListViewModel(IRegionManager manager)
        {

            _regionManager = manager;
            AddArtistCommand = new DelegateCommand(OpenAdd);
            EditArtistCommand = new DelegateCommand(OpenEdit);
            SearchWordChanged = new DelegateCommand(Search);

        }

        private void OpenAdd()
        {
            
            _regionManager.RequestNavigate("content","AddEditArtist");

        }

        private void OpenEdit()
        {
            if (SelectedArtist == null)
                MessageBox.Show("You have to choose artist.");
            else
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "Artist", SelectedArtist }
                };
                _regionManager.RequestNavigate("content","AddEditArtist",parameters);
            }
        }

        private void Search()
        {

            if (string.IsNullOrEmpty(SearchWord))
            {

                ViewList = _allArtists;

            }
            else
            {

                ViewList = _allArtists.Where(c => c.Name.Contains(SearchWord)).ToList();

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

            GetTable();
            
        }

        private async void GetTable()
        {
            using (var context = new ShopDbEntities())
            {

                _artistRepository = new SqlRepository<Artist>(context);
                _allArtists = await _artistRepository.GetAll().ToListAsync();
                ViewList = _allArtists;
            }
        }
    }
}
