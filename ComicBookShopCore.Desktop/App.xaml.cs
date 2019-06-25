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
using ComicBookShopCore.Data.Searchers;
using ComicBookShopCore.Desktop.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ComicBookShopCore.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IRepository<Artist>, SqlRepository<Artist>>();
            containerRegistry.RegisterSingleton<IRepository<Publisher>, SqlRepository<Publisher>>();
            containerRegistry.RegisterSingleton<IRepository<Series>, SqlRepository<Series>>();
            containerRegistry.RegisterSingleton<IRepository<ComicBook>, SqlRepository<ComicBook>>();
            containerRegistry.RegisterSingleton<IRepository<ComicBookArtist>, SqlRepository<ComicBookArtist>>();
            containerRegistry.RegisterSingleton<IOpenable<User>, SqlRepository<User>>();
            containerRegistry.RegisterSingleton<IRepository<Order>, SqlRepository<Order>>();
            containerRegistry.RegisterSingleton<DbContext, ShopDbEntities>();
            containerRegistry.Register<IUserEmployeeSearcherFactory, DbRoleSearcherFactory>();

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
