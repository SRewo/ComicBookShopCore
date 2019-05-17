using System;
using System.Collections.Generic;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;

namespace ComicBookShopCore.Web.Models
{
    public class IndexViewModel
    {
        private readonly IRepository<ComicBook> _comicBookRepository;

        public IndexViewModel(IRepository<ComicBook> comicBookRepository)
        {
            _comicBookRepository = comicBookRepository;
        }

        public List<ComicBook> ViewList { get; set; }
        public List<ComicBook> PremieresList { get; set; }

        public void GetData()
        {
            ViewList = _comicBookRepository.GetAll().Where(x => x.OnSaleDate <= DateTime.Today)
                .OrderByDescending(x => x.OnSaleDate).ToList();
            PremieresList = _comicBookRepository.GetAll().Where(x => x.OnSaleDate > DateTime.Today)
                .OrderBy(x => x.OnSaleDate).ToList();
        }
    }
}