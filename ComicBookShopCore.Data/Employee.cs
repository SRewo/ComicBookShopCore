using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Employee
    {
        public int Id { get; private set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Job { get; set; }
        public Address Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
