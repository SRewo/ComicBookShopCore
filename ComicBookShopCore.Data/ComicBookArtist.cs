using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class ComicBookArtist : ValidationClass
    {
        public int Id { get; private set; }

        [Required]
        public Artist Artist { get; set; }

        [Required]
        public ComicBook ComicBook { get; set; }

        public string Type { get; set; }

        internal ComicBookArtist()
        {

        }
    }
}
