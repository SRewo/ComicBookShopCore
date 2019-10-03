using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ComicBookShopCore.Data
{
    public class Publisher : ValidationClass
    {
        public int Id { get; private set; }


        [Required(ErrorMessage = "Publisher name cannot be empty")]
        [MinLength(3, ErrorMessage = "Publisher name is too short.")]
        [MaxLength(40, ErrorMessage = "Publisher name is too long")]
        public string Name { get; set; }

        public string Description { get; set; }

        [CustomValidation.PublisherDateValidation]
        public DateTime CreationDateTime { get; set; }

        public IEnumerable<Series> SeriesList { get; set; }

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
