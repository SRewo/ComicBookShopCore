using ComicBookShopCore.Services.Artist;
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
            CreateMap<Data.Series, SeriesBasicDto>();
            CreateMap<PublisherDto, Data.Publisher>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}