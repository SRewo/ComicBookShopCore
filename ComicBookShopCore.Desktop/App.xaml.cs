﻿using Prism.Ioc;
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
using Microsoft.AspNetCore.Identity;

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
            containerRegistry.RegisterInstance<IRepository<ComicBook>>(new SqlRepository<ComicBook>(context));
            containerRegistry.RegisterInstance<IRepository<ComicBookArtist>>(new SqlRepository<ComicBookArtist>(context));
            containerRegistry.RegisterInstance<IRepository<User>>(new SqlRepository<User>(context));
            containerRegistry.RegisterInstance<IRepository<Order>>(new SqlRepository<Order>(context));
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
