using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Web.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

namespace ComicBookShopCore.Web.Tests.ViewModels
{
    //public class ComicBookListViewModelTests
    //{
    //    [Fact]
    //    public void GetData_GetAll_ValidCall()
    //    {
    //        var mock = new Mock<IRepository<ComicBook>>();
    //        mock.Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample());
    //        var model = new ComicBookListViewModel(mock.Object, 0, 1);
    //        model.GetData();

    //        Assert.NotEmpty(model.ViewList);
    //        Assert.Equal(1, model.Page);
    //        Assert.Equal(1, model.NumberOfPages);
    //    }

    //    [Fact]
    //    public void GetData_PublisherIsSelected_ValidCall()
    //    {
    //        var mock = new Mock<IRepository<ComicBook>>();
    //        mock.Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample());
    //        var model = new ComicBookListViewModel(mock.Object, 1, 1);
    //        model.GetData();

    //        Assert.NotEmpty(model.ViewList);
    //        Assert.Equal(1, model.NumberOfPages);
    //        Assert.Single(model.ViewList);
    //    }

    //    [Fact]
    //    public void GetData_OtherPublishersSelected_ValidCall()
    //    {
    //        var mock = new Mock<IRepository<ComicBook>>();
    //        mock.Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
    //        var model = new ComicBookListViewModel(mock.Object, 4, 1);
    //        model.GetData();

    //        Assert.Empty(model.ViewList);
    //    }

    //    [Fact]
    //    public void GetData_WithSearchWord_ValidCall()
    //    {
    //        var mock = new Mock<IRepository<ComicBook>>();
    //        mock.Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
    //        var model = new ComicBookListViewModel(mock.Object, 0,1, "Ant");
    //        model.GetData();

    //        Assert.NotEmpty(model.ViewList);
    //        Assert.Single(model.ViewList);
    //        Assert.Equal(1, model.NumberOfPages);
    //        Assert.Equal(TestData.GetComicBooksSample().ToList()[1].Title, model.ViewList.First().Title);
    //    }


}
