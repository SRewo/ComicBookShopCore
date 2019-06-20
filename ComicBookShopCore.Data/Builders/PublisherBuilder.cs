using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data.Builders
{
    public class PublisherBuilder
    {
        protected Publisher Publisher = new Publisher();

        public virtual PublisherDetailsBuilder Details => new PublisherDetailsBuilder(Publisher);

        public Publisher Build()
        {
            Publisher.Validate();
            if (Publisher.HasErrors)
            {
                throw new ValidationException(Publisher.GetFirstError());
            }

            return Publisher;
        }
    }

    public class PublisherDetailsBuilder : PublisherBuilder
    {

        public PublisherDetailsBuilder(Publisher publisher)
        {
            this.Publisher = publisher;
        }

        public PublisherDetailsBuilder Name(string name)
        {
            Publisher.Name = name;
            return this;
        }

        public PublisherDetailsBuilder Description(string desc)
        {
            Publisher.Description = desc;
            return this;
        }

        public PublisherDetailsBuilder Created(DateTime created)
        {
            Publisher.CreationDateTime = created;
            return this;
        }

    }
}