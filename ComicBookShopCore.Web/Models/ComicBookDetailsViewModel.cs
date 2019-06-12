using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;

namespace ComicBookShopCore.Web.Models
{
    public class ComicBookDetailsViewModel
    {
        public ComicBook ComicBook { get; set; }

        public ComicBookDetailsViewModel(ComicBook comic)
        {
            ComicBook = comic;
        }
    }
}
