using System.Collections.Generic;
using ComicBookShopCore.Services.Series;

namespace ComicBookShopCore.Services.Publisher
{
    public class PublisherBasicDto
    {
        public int Id { get; private set; }
        public string Name { get; set; }
    }

    public class PublisherDto
    {
        public int  Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class PublisherDetailsDto
    {
        public int Id { get; private set; }
        public string Name { get; set; }
	public string Description { get; set; }
        public IEnumerable<SeriesBasicDto> SeriesList { get; private set; }
    }
}