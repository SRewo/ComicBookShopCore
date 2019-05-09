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
                bool valid = true;
                foreach(char x in " !\"#$%&()*+,-./:;<=>?@[\\]^_{|}~")
                {
                    if (name.Contains(x))
                        valid = false;
                }
                if (valid)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
        }
    }
}
