using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ComicBookShopCore.Data
{
    public class Order : ValidationClass
    {
        public int Id { get; private set; }


        [Required]
        public DateTime Date { get; set; }


        [Required]
        public virtual ObservableCollection<OrderItem> OrderItems { get; set; }


        [Required]
        public virtual User User { get; set; }

        internal Order()
        {

        }
        public double TotalPrice => OrderItems.Sum(x => x.Price);

    }
}
