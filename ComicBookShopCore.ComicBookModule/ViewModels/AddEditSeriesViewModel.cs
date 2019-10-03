using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.ComicBookModule.ViewModels
{
    public class SeriesInputModel : ValidableBase
    {
        private string _name;

        [Required]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Publisher _publisher;

        [Required]
        public Publisher Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }

    public class AddEditSeriesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IRepository<Series> _seriesRepository;
        private SeriesBuilder _seriesBuilder;

        public DelegateCommand SaveSeriesCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public bool IsEditing { get; private set; }
        

        private SeriesInputModel _inputModel;

        public SeriesInputModel InputModel
        {
            get => _inputModel;
            set => SetProperty(ref _inputModel, value);
        }

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

    public AddEditSeriesViewModel(IRegionManager manager, IRepository<Publisher> publisherRepository, IRepository<Series> seriesRepository)
        {

            _regionManager = manager;
            _publisherRepository = publisherRepository;
            _seriesRepository = seriesRepository;

            GoBackCommand = new DelegateCommand(GoBack);
            SaveSeriesCommand = new DelegateCommand((() => SaveSeriesAsync()));
            InputModel = new SeriesInputModel();
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

            ResetFormAsync();

            Series = (Series) navigationContext.Parameters["series"];

            CheckPassedSeriesAsync(Series);

            GetPublishersFromRepositoryAsync();
            SetErrorsChangedEventAsync();

        }

        public virtual Task SetErrorsChangedEventAsync()
        {
            InputModel.PropertyChanged += InputModel_PropertyChanged;
            return Task.CompletedTask;
        }

        private void InputModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NameErrorMessage = InputModel.GetFirstError("Name");
            CanSave = !InputModel.HasErrors;
          
            if (string.IsNullOrWhiteSpace(InputModel.Name) || InputModel.Publisher == null)
            {
                CanSave = false;
                return;
            }

            if (IsEditing && (Series.Name == InputModel.Name && Equals(Series.Publisher, InputModel.Publisher) &&
                InputModel.Description == Series.Description))
            {
                CanSave = false;
            }

        }

        public virtual Task CheckPassedSeriesAsync(Series series)
        {
            IsEditing = series != null;

            if (IsEditing)
            {
                InputModel.Name = series.Name;
                InputModel.Publisher = series.Publisher;
                InputModel.Description = series.Description;
                Series = series;
                return Task.CompletedTask;
            }

            _seriesBuilder = new SeriesBuilder();

            return Task.CompletedTask;
        }

        public virtual Task ResetFormAsync()
        {

            Series = null;
            InputModel = new SeriesInputModel();
            CanSave = false;
            NameErrorMessage = string.Empty;
            Publishers = null;

            return Task.CompletedTask;
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("content","SeriesList");

        }

        public virtual  Task GetPublishersFromRepositoryAsync()
        {
            Publishers =  _publisherRepository.GetAll().ToList();
            return Task.CompletedTask;
        }

        private Task SaveSeriesAsync()
        {

            if(!IsEditing)
            {
                try
                {
                    Series = _seriesBuilder
                        .Details
                            .Name(InputModel.Name)
                            .Publisher(InputModel.Publisher)
                            .Description(InputModel.Description)
                        .Build();
                    _seriesRepository.Add(Series);
                }
                catch (ValidationException e)
                {
                    MessageBox.Show(e.Message);
                    return Task.CompletedTask;
                }

                GoBackCommand.Execute();
                return Task.CompletedTask;
            }

            Series.Name = InputModel.Name;
            Series.Publisher = InputModel.Publisher;
            Series.Description = InputModel.Description;
            Series.Validate();
            if (Series.HasErrors)
            {
                MessageBox.Show(Series.GetFirstError());
                return Task.CompletedTask;
            }

            _seriesRepository.Update(Series);
            GoBack();
            return Task.CompletedTask;
        }
    }
}
