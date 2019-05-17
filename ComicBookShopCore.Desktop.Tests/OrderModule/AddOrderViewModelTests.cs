using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.OrderModule.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.OrderModule
{
    public class AddOrderViewModelTests
    {
        [Fact]
        public void CreateOrder_ValidCall()
        {
            GlobalVariables.LoggedEmployee = new Employee()
            {
                FirstName = "John",
                LastName = "Kent"
            };
            var model = new AddOrderViewModel(null, null, null, null);
            model.CreateOrder();

            Assert.NotNull(model.Order);
            Assert.NotNull(model.Order.OrderItems);
            Assert.Equal(GlobalVariables.LoggedEmployee.FirstName, model.Order.Employee.FirstName);
            Assert.Equal(GlobalVariables.LoggedEmployee.LastName, model.Order.Employee.LastName);
        }

        [Fact]
        public void GetData_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(GetComicBooksSample());
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample());
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();

            Assert.NotNull(model.ComicBooks);
            Assert.NotNull(model.Publishers);
            Assert.Equal(GetComicBooksSample().First().Title, model.ComicBooks.First().Title);
            Assert.Equal(GetPublishersSample().First().Name, model.Publishers.First().Name);
        }

        [Fact]
        public void AddItemCommand_WithoutErrors_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null);
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            model.SelectedComicBook = GetComicBooksSample().First();
            model.AddItemCommand.Execute();

            var actualItem = model.Order.OrderItems.First();

            Assert.NotNull(actualItem);
            Assert.Equal(model.SelectedComicBook, actualItem.ComicBook);
            Assert.Equal(0, actualItem.Discount);
            Assert.Equal(1, actualItem.Quantity);
        }

        [Fact]
        public void AddItemCommand_SelectedComicBookIsNull_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null);
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            model.AddItemCommand.Execute();

            Assert.Empty(model.Order.OrderItems);
        }

        [Fact]
        public void AddItemCommand_SelectedComicBookAlreadyInCollection_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null);
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            model.Order.OrderItems.Add(new OrderItem()
            {
                ComicBook = GetComicBooksSample().First(),
                Discount = 0,
                Quantity = 1
            });
            model.SelectedComicBook = GetComicBooksSample().First();
            model.AddItemCommand.Execute();



            Assert.Single(model.Order.OrderItems);
        }

        [Fact]
        public void RemoveItemCommand_WithoutErrors_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null);
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            model.Order.OrderItems.Add(new OrderItem()
            {
                ComicBook = GetComicBooksSample().First(),
                Discount = 0,
                Quantity = 1
            });
            model.SelectedOrderItem = model.Order.OrderItems.First();
            model.RemoveItemCommand.Execute();

            Assert.Empty(model.Order.OrderItems);
        }

        [Fact]
        public void RemoveItemCommand_SelectedOrderItemIsNull_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null);
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            model.Order.OrderItems.Add(new OrderItem()
            {
                ComicBook = GetComicBooksSample().First(),
                Discount = 0,
                Quantity = 1
            });
            model.SelectedOrderItem = null;
            model.RemoveItemCommand.Execute();

            Assert.NotEmpty(model.Order.OrderItems);
        }

        [Fact]
        public void SelectedPublisherChangedCommand_WithoutSearchWord_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.SelectedPublisher = GetPublishersSample().First();
            model.SearchWord = string.Empty;
            model.GetData();
            model.SelectedPublisherChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Dark Nights Metal: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SelectedPublisherChanged_WithSearchWord_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.SelectedPublisher = GetPublishersSample().First();
            model.GetData();
            model.SearchWord = "Dark";
            model.SelectedPublisherChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Dark Nights Metal: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SearchWordChanged_WithoutPublisher_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();
            model.SearchWord = "Ant";
            model.SearchWordChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Ant Man Last Days: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SearchWordChanged_WithPublisher_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();
            model.SearchWord = "Ant";
            model.SelectedPublisher = GetPublishersSample().ToList()[1];
            model.SearchWordChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Ant Man Last Days: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SaveOrder_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddOrderViewModel>();
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            var comicBook = GetComicBooksSample().First();
            model.Order.OrderItems.Add(new OrderItem()
            {
                ComicBook = comicBook,
                Discount = 0,
                Quantity = 1
            }); ;
            model.SaveOrderCommand.Execute();

            mock.Mock<IRepository<Order>>().Verify(x => x.Add(model.Order), Times.Once);
            Assert.Equal(9, comicBook.Quantity);
        }

        [Fact]
        public void SaveOrder_QuantityIsOverLimit_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddOrderViewModel>();
            model.Order = new Order();
            model.Order.OrderItems = new ObservableCollection<OrderItem>();
            var comicBook = GetComicBooksSample().First();
            model.Order.OrderItems.Add(new OrderItem()
            {
                ComicBook = comicBook,
                Discount = 0,
                Quantity = 11
            }); ;
            model.SaveOrderCommand.Execute();

            mock.Mock<IRepository<Order>>().Verify(x => x.Add(model.Order), Times.Never);
            Assert.False(model.CanSave);
        }


        private IQueryable<ComicBook> GetComicBooksSample()
        {
            var series = GetSeriesSample().ToList();
            var artists = GetSampleArtists().ToList();

            var comicBooks = new List<ComicBook>()
            {
                new ComicBook(0)
                {
                    Title = "Dark Nights Metal: #1",
                    Series = series[0],
                    Price = 9.99,
                    Quantity = 10,
                    OnSaleDate = new DateTime(2010,12,10),
                    ComicBookArtists = new ObservableCollection<ComicBookArtist>()
                    {
                        new ComicBookArtist()
                        {
                            Artist = artists[0],
                            Type = "Writer"
                        },
                        new ComicBookArtist()
                        {
                            Artist = artists[1],
                            Type = "Cover Variant"
                        },
                    }
                },
                new ComicBook(1)
                {
                    Title = "Ant Man Last Days: #1",
                    Series = series[1],
                    Price = 5.99,
                    Quantity = 15,
                    OnSaleDate = new DateTime(2011,10,20),
                    ComicBookArtists = new ObservableCollection<ComicBookArtist>()
                    {
                        new ComicBookArtist()
                        {
                            Artist = artists[2],
                            Type = "Writer"
                        },
                        new ComicBookArtist()
                        {
                            Artist = artists[1],
                            Type = "Cover Variant"
                        },
                    }
                }
            };
            return comicBooks.AsQueryable();
        }

        private IQueryable<Series> GetSeriesSample()
        {
            var publishers = GetPublishersSample().ToList();
            var series = new List<Series>()
            {
                new Series(1)
                {
                    Name = "Dark Nights Metal",
                    Description = "Series one",
                    Publisher = publishers[0]
                },
                new Series(2)
                {
                    Name = "Ant-Man: Last Days",
                    Description = "Ant-Man",
                    Publisher = publishers[1]
                }
            };

            return series.AsQueryable();

        }

        private IQueryable<Publisher> GetPublishersSample()
        {
            var tmp = new List<Publisher>()
            {
                new Publisher(1)
                {
                    Name = "DC Comics",
                    CreationDateTime = DateTime.Parse("01.01.1934"),
                    Description = "Some random description."
                },
                new Publisher(2)
                {
                    Name = "Marvel Comics",
                    CreationDateTime = DateTime.Parse("01.01.1939"),
                    Description = "Another description"
                },
                new Publisher(3)
                {
                    Name = "Dark Horse Comics",
                    CreationDateTime = DateTime.Parse("01.01.1986"),
                    Description = " American comic book and manga publisher."
                }
            };

            return tmp.AsEnumerable().AsQueryable();
        }

        private IQueryable<Artist> GetSampleArtists()
        {

            var testsArtists = new List<Artist>
            {
                new Artist
                {
                    FirstName = "John",
                    LastName =  "Mick"
                },
                new Artist
                {
                    FirstName = "Mark",
                    LastName = "Well"
                },
                new Artist
                {
                    FirstName = "John",
                    LastName = "River"
                }
            };

            return testsArtists.AsQueryable();

        }
    }
}
