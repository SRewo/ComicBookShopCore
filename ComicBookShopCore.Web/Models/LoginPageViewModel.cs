using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace ComicBookShopCore.Web.Models
{
    public class LoginPageViewModel
    {
        public List<string> Errors { get; set; }
        public LoginPageViewModel()
        {
            Errors = new List<string>();
        }

    }
}
