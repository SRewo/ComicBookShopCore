using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class ComicBook : ValidationClass
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "Comic book title cannot be empty.")]
        public string Title { get; set; }

        public DateTime OnSaleDate { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter valid price.")]
        public double Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid value.")]
        public int Quantity { get; set; }

        [Required] public virtual Series Series { get; set; }

        [Required] public ObservableCollection<ComicBookArtist> ComicBookArtists { get; set; }

        [MaxLength(120)] public string ShortDescription { get; set; }

        public string Description { get; set; }


        public string ShortArtistDetail => GetShortArtistDetail();

        internal ComicBook()
        {

        }

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
    }
}