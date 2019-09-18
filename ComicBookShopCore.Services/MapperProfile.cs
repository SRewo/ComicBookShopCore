using ComicBookShopCore.Services.Artist;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.Services.Publisher;
using ComicBookShopCore.Services.Series;

namespace ComicBookShopCore.Services
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Data.Artist, ArtistDto>();
            CreateMap<Data.Artist, ArtistDetailsDto>();
	    CreateMap<ArtistDto, Data.Artist>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ArtistDetailsDto, Data.Artist>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Data.Publisher, PublisherBasicDto>();
            CreateMap<Data.Publisher, PublisherDetailsDto>();
            CreateMap<Data.Publisher, PublisherDto>();
            CreateMap<PublisherDto, Data.Publisher>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Data.Series, SeriesBasicDto>();
            CreateMap<Data.Series, SeriesDto>();
            CreateMap<Data.Series, SeriesDetailsDto>();	
	    CreateMap<SeriesInputDto, Data.Series>();
            CreateMap<Data.Series, SeriesInputDto>();

            CreateMap<Data.ComicBook, ComicBookBasicDto>();
        }
    }
}