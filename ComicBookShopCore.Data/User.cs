using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;
using Microsoft.AspNetCore.Identity;

namespace ComicBookShopCore.Data
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [CustomValidation.NameValidation(ErrorMessage = "First name cannot contain special characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required] public Address Address { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set;}

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Name => FirstName + " " + LastName;

        protected bool Equals(User other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FirstName != null ? FirstName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateOfBirth.GetHashCode();
                return hashCode;
            }
        }


        public bool CheckPasswords(SecureString secureString)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(this, PasswordHash,
                       new NetworkCredential(string.Empty, secureString).Password) ==
                   PasswordVerificationResult.Success;
        }
    }
}