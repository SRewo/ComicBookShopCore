using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data.Builders
{
    public class ArtistBuilder
    {
        protected Artist Artist = new Artist();

        public virtual ArtistDetailsBuilder Details => new ArtistDetailsBuilder(this.Artist);

        public Artist Build()
        {
            Artist.Validate();
            if (Artist.HasErrors )
                throw new ValidationException(Artist.GetFirstError());


            return Artist;
        }
    }

    public class ArtistDetailsBuilder : ArtistBuilder
    {
        public ArtistDetailsBuilder(Artist artist)
        {
            this.Artist = artist;
        }

        public ArtistDetailsBuilder FirstName(string name)
        {
            Artist.FirstName = name;
            return this;
        }

        public ArtistDetailsBuilder LastName(string name)
        {
            Artist.LastName = name;
            return this;
        }

        public ArtistDetailsBuilder Description(string desc)
        {
            Artist.Description = desc;
            return this;
        }
    }
}