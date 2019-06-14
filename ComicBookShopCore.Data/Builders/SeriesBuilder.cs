using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data.Builders
{
    public class SeriesBuilder
    {
        protected Series Series = new Series();

        public virtual SeriesDetailsBuilder Details => new SeriesDetailsBuilder(Series);

        public Series Build()
        {
            Series.Validate();
            if (Series.HasErrors)
            {
                throw new ValidationException(Series.GetFirstError());
            }

            return Series;
        }
    }

    public class SeriesDetailsBuilder : SeriesBuilder
    {

        public SeriesDetailsBuilder(Series series)
        {
            Series = series;
        }

        public SeriesDetailsBuilder Name(string name)
        {
            Series.Name = name;
            return this;
        }

        public SeriesDetailsBuilder Description(string desc)
        {
            Series.Description = desc;
            return this;
        }

        public SeriesDetailsBuilder Publisher(Publisher publisher)
        {
            Series.Publisher = publisher;
            return this;
        }
    }
}