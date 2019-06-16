using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data.Builders
{
    public class OrderBuilder
    {
        protected Order Order = new Order(){ OrderItems = new ObservableCollection<OrderItem>()};

        public OrderDetailsBuilder Details => new OrderDetailsBuilder(Order);
        public OrderOrderItemBuilder AddItem => new OrderOrderItemBuilder(Order);

        public Order Build()
        {
            Order.Validate();
            if (Order.HasErrors)
            {
                throw new ValidationException(Order.GetFirstError());
            }

            return Order;
        }
    }

    public class OrderOrderItemBuilder : OrderBuilder
    {
        private readonly OrderItem _item;

        public OrderOrderItemBuilder(Order order)
        {
            this.Order = order;
            _item = new OrderItem();
        }

        public OrderOrderItemBuilder ComicBook(ComicBook comicBook)
        {
            _item.ComicBook = comicBook;
            return this;
        }

        public OrderOrderItemBuilder Quantity(int quantity)
        {
            _item.Quantity = quantity;
            return this;
        }

        public OrderOrderItemBuilder Discount(int discount)
        {
            _item.Discount = discount;
            return this;
        }

        public OrderBuilder Add()
        {
            _item.Validate();
            if (_item.HasErrors)
            {
                throw new ValidationException(_item.GetFirstError());
            }

            Order.OrderItems.Add(_item);
            return this;
        }
    }

    public class OrderDetailsBuilder : OrderBuilder
    {
        public OrderDetailsBuilder(Order order)
        {
            this.Order = order;
        }

        public OrderDetailsBuilder Date(DateTime date)
        {
            Order.Date = date;
            return this;
        }

        public OrderDetailsBuilder Employee(User employee)
        {
            Order.Employee = employee;
            return this;
        }
    }
}