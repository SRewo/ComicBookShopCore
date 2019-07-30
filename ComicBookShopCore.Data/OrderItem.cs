using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicBookShopCore.Data
{
    public class OrderItem : ValidableBase
    {
        public int Id { get; private set; }


        private ComicBook _comicBook;

        [Required(ErrorMessage = "Comic Book can't be null or empty.")]
        public virtual ComicBook ComicBook
        {
            get => _comicBook;
            set => SetProperty(ref _comicBook,value);
        }


        private int _quantity;

        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Please enter valid value.")]
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }


        private int _discount;

        [Range(0,100, ErrorMessage = "Please enter valid value.")]
        public int Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value);
        }

        private double _price;

        [NotMapped]
        public double Price
        {
            get => ComicBook.Price * Quantity * (1 - Discount * 0.01);
            private set => SetProperty(ref _price, value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(!HasErrors)
                Price = ComicBook.Price * Quantity * (1 - Discount * 0.01);
        }
    }
}
