using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Address
    {
        public int Id { get; private set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Region { get; set; }
    }
}
