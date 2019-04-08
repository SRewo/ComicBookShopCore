using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using ComicBookShopCore.Desktop.Views;

namespace ComicBookShopCore.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var context = new ShopDbEntities();

            containerRegistry.RegisterInstance<IRepository<Artist>>(new SqlRepository<Artist>(context));
            containerRegistry.RegisterInstance<IRepository<Publisher>>(
                new SqlRepository<Publisher>(context));
            containerRegistry.RegisterInstance<IRepository<Series>>(new SqlRepository<Series>(context));

        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

            moduleCatalog.AddModule<ComicBookModule.ComicBookModule>();
            moduleCatalog.AddModule<EmployeeModule.EmployeeModule>();
            moduleCatalog.AddModule<OrderModule.OrderModule>();

        }
    }
}
