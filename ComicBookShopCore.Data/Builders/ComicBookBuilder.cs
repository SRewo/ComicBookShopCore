using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ComicBookShopCore.Data.Builders
{
    public class ComicBookBuilder
    {
        protected ComicBook ComicBook = new ComicBook(){ComicBookArtists = new ObservableCollection<ComicBookArtist>()};

        public virtual ComicBookStatusBuilder Status => new ComicBookStatusBuilder(ComicBook);
        public virtual ComicBookDescriptionBuilder Description => new ComicBookDescriptionBuilder(ComicBook);
        public virtual ComicBookDetailsBuilder Details => new ComicBookDetailsBuilder(ComicBook);
        public virtual ComicBookArtistBuilder AddArtist => new ComicBookArtistBuilder(ComicBook);

        public ComicBook Build()
        {
            ComicBook.Validate();
            if (ComicBook.HasErrors)
            {
                throw new ValidationException(ComicBook.GetFirstError());
            }

            return ComicBook;
        }
    }

    public class ComicBookStatusBuilder : ComicBookBuilder
    {

        public ComicBookStatusBuilder(ComicBook comicBook)
        {
            this.ComicBook = comicBook;
        }

        public ComicBookStatusBuilder Price(double price)
        {

            ComicBook.Price = price;
            return this;

        }

        public ComicBookStatusBuilder Quantity(int quantity)
        {

            ComicBook.Quantity = quantity;
            return this;

        }

    }

    public class ComicBookDescriptionBuilder : ComicBookBuilder
    {

        public ComicBookDescriptionBuilder(ComicBook comicBook)
        {
            this.ComicBook = comicBook;
        }

        public ComicBookDescriptionBuilder ShortDesc(string desc)
        {
            ComicBook.ShortDescription = desc;
            return this;
        }

        public ComicBookDescriptionBuilder LongDesc(string desc)
        {
            ComicBook.Description = desc;
            return this;
        }
    }

    public class ComicBookDetailsBuilder : ComicBookBuilder
    {

        public ComicBookDetailsBuilder(ComicBook comicBook)
        {
            this.ComicBook = comicBook;
        }

        public ComicBookDetailsBuilder Title(string title)
        {
            ComicBook.Title = title;
            return this;
        }

        public ComicBookDetailsBuilder OnSaleDate(DateTime date)
        {
            ComicBook.OnSaleDate = date;
            return this;
        }

        public ComicBookDetailsBuilder Series(Series series)
        {
            ComicBook.Series = series;
            return this;
        }

        public ComicBookDetailsBuilder ArtistList(ObservableCollection<ComicBookArtist> list)
        {
            ComicBook.ComicBookArtists = list;
            return this;
        }
    }

    public class ComicBookArtistBuilder : ComicBookBuilder
    {
        private ComicBookArtist _comicArtist;

        public ComicBookArtistBuilder(ComicBook comicBook)
        {
            ComicBook = comicBook;
            _comicArtist = new ComicBookArtist();
        }

        public ComicBookArtistBuilder Artist(Artist artist)
        {
            _comicArtist.Artist = artist;
            return this;
        }

        public ComicBookArtistBuilder Role(string role)
        {
            _comicArtist.Type = role;
            return this;
        }

        public ComicBookArtistBuilder Artist(ComicBookArtist artist)
        {
            _comicArtist = artist;
            return this;
        }

        public ComicBookBuilder Add()
        {
            _comicArtist.Validate();
            if (_comicArtist.HasErrors)
                throw new ValidationException(_comicArtist.GetFirstError());

            ComicBook.ComicBookArtists.Add(_comicArtist);

            return this;
        }

        public ComicBookArtist BuildComicBookArtist()
        {
            _comicArtist.Validate();
            if (_comicArtist.HasErrors)
                throw new ValidationException(_comicArtist.GetFirstError());

            return _comicArtist;
        }

    }

}