using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Web.Models
{
    public class ComicBookListViewModel
    {
        private IRepository<ComicBook> _comicBookRepository;
        
        private List<ComicBook> _comicBooks;

        public List<ComicBook> ViewList { get; set; }
        public int Page { get; set; }
        public int NumberOfPages { get; set; }
        public int PublisherId { get; set; }

        public ComicBookListViewModel()
        {

        }

        public ComicBookListViewModel(IRepository<ComicBook> repository, int publisherId, int page)
        {
            _comicBookRepository = repository;
            PublisherId = publisherId;
            Page = page;
        }

        public void GetData()
        {
            _comicBooks = _comicBookRepository.GetAll().Include(x=>x.Series).Include(x => x.Series.Publisher).ToList();
            if (PublisherId == 0)
            {
                ViewList = _comicBooks;
                
            }
            else if(PublisherId >=1 && PublisherId <= 3)
            {
                ViewList = _comicBooks.Where(x => x.Series.Publisher.Id == PublisherId).ToList();
            }
            else
            {
                ViewList = _comicBooks.Where(x => !Enumerable.Range(1, 3).Contains(x.Series.Publisher.Id)).ToList();
            }
            NumberOfPages = (ViewList.Count() / 18) +1;
        }
    }
}
