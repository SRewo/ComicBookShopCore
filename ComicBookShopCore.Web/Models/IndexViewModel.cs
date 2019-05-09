using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Web.Models
{
    public class IndexViewModel
    {
        IRepository<ComicBook> _comicBookRepository;

        public List<ComicBook> ViewList { get; set; }

        public IndexViewModel(IRepository<ComicBook> comicBookRepository)
        {
            _comicBookRepository = comicBookRepository;
        }

        public void GetData()
        {
            ViewList = _comicBookRepository.GetAll().Where(x => x.OnSaleDate <= DateTime.Today).OrderBy(x => x.OnSaleDate).ToList();
        }
    }
}
