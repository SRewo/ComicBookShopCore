﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
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
        private IRepository<Publisher> _publisherRepository;
        private readonly IRegionManager _regionManager;

        public DelegateCommand SearchWordChanged { get; set; }
        public DelegateCommand AddPublisherCommand { get; set; }
        public DelegateCommand EditPublisherCommand { get; set; }


        private string _searchWord;
        
        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }


        private List<Publisher> _viewList;

        public List<Publisher> ViewList
        {
            get => _viewList;
            set => SetProperty(ref _viewList, value);
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


        private bool _isSearchEnabled;

        public bool IsSearchEnabled
        {
            get => _isSearchEnabled;
            set => SetProperty(ref _isSearchEnabled, value);
        }


        public PublishersListViewModel(IRegionManager regionManager, IRepository<Publisher> repository)
        {
            
            SearchWordChanged = new DelegateCommand(Search);
            EditPublisherCommand = new DelegateCommand(OpenEdit);
            AddPublisherCommand = new DelegateCommand(OpenAdd);

            _regionManager = regionManager;
            _publisherRepository = repository;

        }

        public void Search()
        {

                ViewList = SearchWord == String.Empty ? _allPublishers : _allPublishers.Where(x=> x.Name.Contains(SearchWord)).ToList();

        }

        public void OpenEdit()
        {

            var parameters = new NavigationParameters
            {
                { "publisher", SelectedPublisher }
            };

            _regionManager.RequestNavigate("content","AddEditPublisher",parameters);

        }

        public void OpenAdd()
        {
            _regionManager.RequestNavigate("content","AddEditPublisher");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SearchWord = null;
            ViewList = null;
            GetData();
            CanSearchCheck();

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void GetData()
        {
                _allPublishers = _publisherRepository.GetAll().ToList();
                ViewList = _allPublishers;
        }

        public void CanEditCheck()
        {
            IsEditEnabled = SelectedPublisher != null;
        }

        public void CanSearchCheck()
        {
            IsSearchEnabled = ViewList != null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            CanEditCheck();
        }
    }
}