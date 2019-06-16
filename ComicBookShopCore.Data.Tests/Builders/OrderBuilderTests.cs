using System;
using System.ComponentModel.DataAnnotations;
using ComicBookShopCore.Data.Builders;
using Xunit;

namespace ComicBookShopCore.Data.Tests.Builders
{
    public class OrderBuilderTests
    {
        [Fact]
        public void Build_MultipleOrderItems_ValidCall()
        {
            var builder = new OrderBuilder();
            var comicBook = new ComicBook();
            var employee = new User();
            var bc = builder
                .Details
                    .Date(new DateTime(1999, 01, 01))
                    .Employee(employee)
                .AddItem
                    .ComicBook(comicBook)
                    .Quantity(2)
                    .Discount(0)
                    .Add()
                .AddItem
                    .ComicBook(comicBook)
                    .Quantity(3)
                    .Discount(12)
                    .Add()
                .Build();

            Assert.NotNull(bc);
            Assert.NotNull(bc.OrderItems);
            Assert.NotEmpty(bc.OrderItems);
            Assert.Equal(2, bc.OrderItems.Count);
            Assert.Equal(3,bc.OrderItems[1].Quantity);
        }

        [Fact]
        public void Build_MissingPropertyOrder_ThrowsException()
        {
            var builder = new OrderBuilder();
            var bc = builder.Details.Date(new DateTime(1999, 01, 01));

            Assert.Throws<ValidationException>((() => bc.Build()));
        }

        [Fact]
        public void Build_MissingPropertyOrderItem_ThrowsException()
        {
            var builder = new OrderBuilder();
            var comicBook = new ComicBook();
            var employee = new User();
            var bc = builder
                .Details
                .Date(new DateTime(1999, 01, 01))
                .Employee(employee)
                .AddItem
                .ComicBook(comicBook)
                .Discount(0);

            Assert.Throws<ValidationException>((() => bc.Add()));
        }

        [Fact]
        public void Build_InvalidOrderItemQuantity_ThrowsException()
        {
            var builder = new OrderBuilder();
            var comicBook = new ComicBook();
            var employee = new User();
            var bc = builder
                .Details
                .Date(new DateTime(1999, 01, 01))
                .Employee(employee)
                .AddItem
                .ComicBook(comicBook)
                .Quantity(-1)
                .Discount(0);

            Assert.Throws<ValidationException>((() => bc.Add()));
        }
    }
}