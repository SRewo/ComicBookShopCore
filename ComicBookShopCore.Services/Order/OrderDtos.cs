using System;
using System.Collections.Generic;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.Services.User;

namespace ComicBookShopCore.Services.Order
{
    public class OrderBasicDto
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime OrderDate { get; set; }
        public int NumberOfItems { get; set; }
        public double TotalPrice { get; set; }
    }

    public class OrderDetailsDto
    {
        public int Id { get; private set; }
        public UserBasicDto User { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderInputDto
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public IEnumerable<OrderItemInputDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; private set; }
        public ComicBookBasicDto Comic { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public double Price { get; private set; }
    }

    public class OrderItemInputDto
    {
        public int ComicBookId { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
    }
}