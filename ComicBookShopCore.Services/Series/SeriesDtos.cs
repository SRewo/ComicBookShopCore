using System.Collections.Generic;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.Services.Publisher;

namespace ComicBookShopCore.Services.Series
{
    public class SeriesBasicDto
    {
        public int Id { get; private set; }
        public string Name { get; set; }
    }

    public class SeriesDto
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string PublisherName { get; set; }
    }

    public class SeriesDetailsDto
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public PublisherBasicDto Publisher { get; private set; }
        public IEnumerable<ComicBookBasicDto> ComicBooks { get; private set; }
    }
}