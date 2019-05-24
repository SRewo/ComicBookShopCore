using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ComicBookShopCore.Data
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        public bool CheckPasswords(SecureString secureString)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(this, this.PasswordHash, new System.Net.NetworkCredential(string.Empty, secureString).Password) == PasswordVerificationResult.Success;
        }
    }
}
