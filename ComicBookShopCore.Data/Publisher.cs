using System;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Publisher : ValidationClass
    {
        
        public int Id { get; }


        [Required(ErrorMessage = "Publisher name cannot be empty")]
        [MinLength(3, ErrorMessage = "Publisher name is too short.")]
        [MaxLength(40, ErrorMessage = "Publisher name is too long")]
        public string Name { get; set; }

        public string Description { get; set; }

        [CustomValidation.PublisherDateValidation]
        public DateTime CreationDateTime { get; set; }

        internal Publisher()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != typeof(Publisher)) return false;

            return obj is Publisher ob && Id == ob.Id;
        }
        public override int GetHashCode()
        {
            return Id == 0 ? base.GetHashCode() : Id.GetHashCode();
        }
    }


}
