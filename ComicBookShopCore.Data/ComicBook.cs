using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ComicBookShopCore.Data
{
    public class ComicBook : ValidableBase
    {
        public int Id { get; private set; }

        private string _title;

        [Required(ErrorMessage = "Comic book title cannot be empty.")]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private DateTime _onSaleDate;

        public DateTime OnSaleDate
        {
            get => _onSaleDate;
            set => SetProperty(ref _onSaleDate, value);
        }

        private double _price;

        [Required]
        [Range(Double.Epsilon, Double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private int _quantity;

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid value.")]
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private Series _series;

        [Required]
        public virtual Series Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private ObservableCollection<ComicBookArtist> _comicBookArtists;

        [Required]
        public ObservableCollection<ComicBookArtist> ComicBookArtists
        {
            get => _comicBookArtists;
            set => SetProperty(ref _comicBookArtists, value);
        }

        public string ShortArtistDetail => GetShortArtistDetail();


        private string GetShortArtistDetail()
        {

            var n = ComicBookArtists.Count;
            var result = string.Empty;
            foreach (var artist in ComicBookArtists)
            {
                result += n == 1 ? artist.Artist.Name : artist.Artist.Name + ", ";
                n--;
            }

            return result;
        }

        public ComicBook()
        {

        }
        
        public ComicBook(int id)
        {
            Id = id;
        }
    }
}
