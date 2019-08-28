﻿using ComicBookShopCore.Services.Artist;

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
        }
    }
}