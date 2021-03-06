﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicBookShopCore.Data
{
    public class OrderItem : ValidationClass
    {
        public int Id { get; private set; }


        public virtual ComicBook ComicBook { get; set; }

        public int ComicBookId { get; set; }

        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Please enter valid value.")]
        public int Quantity { get; set; }


        [Range(0,100, ErrorMessage = "Please enter valid value.")]
        public int Discount { get; set; }

        public int OrderId { get; set; }

        internal OrderItem()
        {

        }

        [NotMapped]
        public double Price => ComicBook.Price * Quantity * (1 - Discount * 0.01); 
    }
}
