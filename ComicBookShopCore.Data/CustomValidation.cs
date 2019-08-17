using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ComicBookShopCore.Data
{
    public class CustomValidation
    {
        public sealed class NameValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var name = value as string;
                var valid = true;
                foreach(char x in " !\"#$%&()*+,-./:;<=>?@[\\]^_{|}~")
                {
                    if (name.Contains(x))
                        valid = false;
                }

                return valid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            }
        }

        public sealed class PublisherDateValidation : RangeAttribute
        {
            public PublisherDateValidation()
                : base(typeof(DateTime),
                    "1900-01-01",
                    DateTime.Now.AddDays(1).ToShortDateString())
            {
                ErrorMessage = "You have to choose a date between 01.01.1900 and today";
            }
        }
    }
}
