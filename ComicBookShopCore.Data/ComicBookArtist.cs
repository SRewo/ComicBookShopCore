using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class ComicBookArtist : ValidableBase
    {
        public int Id { get; private set; }

        private Artist _artist;

        [Required]
        public Artist Artist
        {
            get => _artist;
            set => SetProperty(ref _artist,value);
        }

        private ComicBook _comicBook;

        [Required]
        public ComicBook ComicBook
        {
            get => _comicBook;
            set => SetProperty(ref _comicBook, value);
        }

        private string _type;

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type,value);
        }
    }
}
