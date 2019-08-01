using System.ComponentModel.DataAnnotations;
using static ComicBookShopCore.Data.CustomValidation;

namespace ComicBookShopCore.Data
{
    public class Artist : ValidationClass
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "First name cannot be empty.")]
        [NameValidation(ErrorMessage ="First name cannot contain special characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be empty.")]
        [NameValidation(ErrorMessage = "Last name cannot contain special characters.")]
        public string LastName { get; set; }

        public string Description { get; set; }

        public string Name => FirstName + " " + LastName;

        public override string ToString()
        {
            return Name;
        }

        internal Artist()
        {
        }
    }
}
