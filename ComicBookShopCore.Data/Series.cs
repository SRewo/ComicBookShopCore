﻿using System.ComponentModel.DataAnnotations;

namespace ComicBookShopCore.Data
{
    public class Series : ValidationClass
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "Series name cannot be empty")]
        public string Name { get; set; }

        [Required]
        public virtual Publisher Publisher { get; set; }

        public string Description { get; set; }

        internal Series()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
