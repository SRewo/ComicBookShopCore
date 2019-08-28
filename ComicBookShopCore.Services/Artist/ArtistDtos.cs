using System.Collections.Generic;
using System.Runtime.Serialization;
using ComicBookShopCore.Data;

namespace ComicBookShopCore.Services.Artist
{
    public class ArtistDto
    {
	[DataMember]
        public int Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ArtistDetailsDto
    {
	[DataMember]
        public int Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }

}