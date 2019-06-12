using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicBookShopCore.Data
{
    public class Order : ValidableBase
    {
        public int Id { get; private set; }


        private DateTime _date;

        [Required]
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date,value);
        }


        private ObservableCollection<OrderItem> _orderItems;

        [Required]
        public virtual ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set => SetProperty(ref _orderItems, value);
        }


        private User _employee;

        [Required]
        public virtual User Employee
        {
            get => _employee;
            set => SetProperty(ref _employee, value);
        }

        
        private double _totalPrice;

        [NotMapped]
        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }


    }
}
