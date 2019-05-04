using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Series : ValidableBase
    {
        public int Id { get; private set; }
        private string _name;

        [Required(ErrorMessage = "Series name cannot be empty")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name,value);
        }

        private Publisher _publisher;

        [Required]
        public virtual Publisher Publisher
        {
            get => _publisher;
            set => SetProperty(ref _publisher,value);
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Series()
        {
        }

        public Series(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
