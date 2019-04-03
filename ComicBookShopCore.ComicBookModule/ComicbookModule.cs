using ComicBookShopCore.ComicBookModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ComicBookShopCore.ComicBookModule
{
    public class ComicBookModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterForNavigation<PublishersListView>("PublisherList");
            containerRegistry.RegisterForNavigation<AddEditPublisherView>("AddEditPublisher");
            containerRegistry.RegisterForNavigation<SeriesListView>("SeriesList");
            containerRegistry.RegisterForNavigation<AddEditSeriesView>("AddEditSeries");
            containerRegistry.RegisterForNavigation<ArtistListView>("ArtistList");
            containerRegistry.RegisterForNavigation<AddEditArtistView>("AddEditArtist");
            containerRegistry.RegisterForNavigation<ComicBookListView>("ComicBookList");
            containerRegistry.RegisterForNavigation<AddEditComicBookView>("AddEditComicBook");

        }
    }
}
