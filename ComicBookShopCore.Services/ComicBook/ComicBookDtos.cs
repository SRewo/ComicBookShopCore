using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Services.ComicBook
{
    public class ComicBookBasicDto
    {
        public int Id { get; private set; }
        public string Title { get; set; }
    }

    public class ComicBookListDto : ComicBookBasicDto
    {
        public DateTime OnSaleDate { get; set; }     
        public double Price { get; set; }
        public int Quantity { get; set; }  
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string ShortDescription { get; set; }
    }

    public class ComicBookDetailsDto : ComicBookListDto
    {
        public IEnumerable<ComicBookArtistDto> ArtistList { get; set; }
        public string Description { get; set; }
    }

    public class ComicBookInputDto
    {
	public string Title { get; set; }
        public DateTime OnSaleDate { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int SeriesId { get; set; }
        public ObservableCollection<ComicBookArtistInputDto> ArtistList { get; set; }
    }

    public class ComicBookArtistInputDto
    {
	public int ArtistId { get; set; }
        public string Role { get; set; }
    }

    public class ComicBookArtistDto
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}