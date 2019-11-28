# ComicBookShopCore

ComicBookShopCore is a group of .net core 3.0 apps for managing a comic book store. At the moment, the project consists of:
* ComicBookShopCore.Data - library used for communication with database
* ComicBookShopCore.Desktop - WPF + Prism app designed for managing data and taking instore orders
* ComicBookShopCore.WebAPI - Asp.net web API 
* ComicBookShopCoreWeb - Angular8 website designed for taking online users orders
* ComicBookShopCore.*.Tests - Xuit tests for listed above apps

Project created is mainly for educational purposes.

## Instalation

Project contains docker containers for db and web projects and also few make commands:
```
make build [APP=container] - for building all/single container
make start [APP=container] - for starting all/single container and its dependencies
make stop [APP=container] - for stopping all/single container
make restart [APP=container] - for restarting all/single container
```

Docker containers:
* ***db*** - mssql database with sample data
  - default port: _1533_
  - default sa password: @Dmin123 (can be changed in .env file)
  - default asp identity credentials for loging in apps:
```
Login: Admin
Password: @Dmin123
```
* ***web*** - container with Angular8 website
  - default port: _4201_
* ***webapi*** - container with asp.net WebAPI
  - default port: _8081_
  
By default, library and WPF app are using a database provided by docker.

## Built With

* [Prism](https://github.com/PrismLibrary/Prism)
* [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore)

## Authors

* [Patryk Szmajduch](https://github.com/SRewo)
