using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Repositories;

namespace ComicBookShopCore.Web.Models
{
    public class LayoutViewModel
    {
        private SqlRepository<Publisher> _publisheRepository;


        public List<Publisher> Publishers { get; set; }

        public LayoutViewModel()
        {
            using (var context = new ShopDbEntities())
            {
                _publisheRepository = new SqlRepository<Publisher>(context);
                Publishers = _publisheRepository.GetAll().ToList();
            }
        }
    }
}
