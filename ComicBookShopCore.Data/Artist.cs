using System.ComponentModel.DataAnnotations;
using static ComicBookShopCore.Data.CustomValidation;

namespace ComicBookShopCore.Data
{
    public class Artist : ValidableBase 
    {
        public int Id { get; private set; }

        private string _firstName;

        [Required(ErrorMessage = "First name cannot be empty.")]
        [NameValidation(ErrorMessage ="First name cannot contain special characters.")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;

        [Required(ErrorMessage = "Last name cannot be empty.")]
        [NameValidation(ErrorMessage = "Last name cannot contain special characters.")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Name => FirstName + " " + LastName;

        public override string ToString()
        {
            return Name;
        }

        public Artist()
        {

        }

        public Artist(int id)
        {
            Id = id;
        }
    }
}
