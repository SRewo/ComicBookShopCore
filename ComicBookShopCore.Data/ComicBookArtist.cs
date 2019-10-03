using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class ComicBookArtist : ValidationClass
    {
        public int Id { get; private set; }

	public int ArtistId { get; set; }
        
        public Artist Artist { get; set; }

        public string Type { get; set; }

        internal ComicBookArtist()
        {

        }
    }
}
