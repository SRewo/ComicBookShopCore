using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Publisher : ValidableBase
    {
        
        public int Id { get; private set; }

        private string _name;
        [Required]
        public string Name {
            get => _name; 
            set => SetProperty(ref _name, value); 
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _creationDateTime;

        [CustomDate]
        public DateTime CreationDateTime
        {
            get => _creationDateTime;
            set => SetProperty(ref _creationDateTime, value);
        }
    }

    public class CustomDateAttribute : RangeAttribute
    {
        public CustomDateAttribute()
            : base(typeof(DateTime),
                "1900-01-01",
                DateTime.Now.AddDays(1).ToShortDateString())
        {
            ErrorMessage = "You have to choose a date between 01.01.1900 and today";
        }
    }
}
