﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ComicBookShopCore.Services.Artist;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.Services.Order;
using ComicBookShopCore.Services.Publisher;
using ComicBookShopCore.Services.Series;
using ComicBookShopCore.Services.User;

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
            CreateMap<Data.ComicBook, ComicBookListDto>()
                .ForMember(x => x.PublisherId, opt => opt.MapFrom(z => z.Series.Publisher.Id))
                .ForMember(x => x.PublisherName, opt => opt.MapFrom(z => z.Series.Publisher.Name));
            CreateMap<Data.ComicBook, ComicBookDetailsDto>()
                .ForMember(x => x.PublisherId, opt => opt.MapFrom(z => z.Series.Publisher.Id))
                .ForMember(x => x.PublisherName, opt => opt.MapFrom(z => z.Series.Publisher.Name))
                .ForMember(x => x.ArtistList, opt => opt.MapFrom(z => z.ComicBookArtists));
            CreateMap<Data.ComicBook, ComicBookInputDto>()
                .ForMember(x => x.ArtistList, opt => opt.MapFrom(z => z.ComicBookArtists));
            CreateMap<ComicBookInputDto, Data.ComicBook>()
                .ForMember(x => x.ComicBookArtists, opt => opt.MapFrom(z => z.ArtistList))
                .ForMember(x => x.ShortArtistDetail, opt => opt.Ignore());
            CreateMap<Data.ComicBookArtist, ComicBookArtistDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(z => z.Artist.Name))
                .ForMember(x => x.Role, opt => opt.MapFrom(z => z.Type));
            CreateMap<Data.ComicBookArtist, ComicBookArtistInputDto>()
                .ForMember(x => x.Role, opt => opt.MapFrom(z => z.Type));
            CreateMap<ComicBookArtistInputDto, Data.ComicBookArtist>()
		.ForMember(x => x.Type, opt => opt.MapFrom(z => z.Role))
                .EqualityComparison((x,z) => x.ArtistId == z.ArtistId);

            CreateMap<Data.Order, OrderBasicDto>();
            CreateMap<Data.OrderItem, OrderItemDto>();
            CreateMap<Data.Order, OrderDetailsDto>();
            CreateMap<OrderInputDto, Data.Order>();
            CreateMap<OrderItemInputDto, Data.OrderItem>()
                .EqualityComparison((x, z) => x.ComicBookId == z.ComicBookId);

            CreateMap<Data.User, UserTokenDto>().ForMember(x => x.Login, opt => opt.MapFrom(z => z.UserName)).ForPath(x => x.Role, opt => opt.Ignore());
            CreateMap<UserAddressDto, Data.Address>();
            CreateMap<Data.Address, UserAddressDto>();
            CreateMap<Data.User, UserUpdateDto>();
            CreateMap<UserUpdateDto, Data.User>();
            CreateMap<UserRegisterDto, Data.User>();
            CreateMap<Data.User, UserDto>().ForMember(x => x.Name, opt => opt.MapFrom(z => z.Name)).ForMember(x => x.Address, opt => opt.MapFrom(z => z.Address));
        }
    }
}