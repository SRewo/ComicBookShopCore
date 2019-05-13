using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ComicBookShopCore.Web.Models;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;

namespace ComicBookShopCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private ShopDbEntities _context = new ShopDbEntities();
        private IRepository<ComicBook> _comicBookRepository;
        public IActionResult Index()
        {
            var model = new IndexViewModel(_comicBookRepository);
            return View(model);
        }

        [HttpGet("/comics/{id}/{page}", Name = "ComicBookList")]
        public IActionResult ComicBookList(int? id, int? page)
        {

            if (!id.HasValue && !page.HasValue)
                return RedirectToAction("Index");

            var model1 = new ComicBookListViewModel(_comicBookRepository, id.Value, page.Value);
            return View(model1);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public HomeController()
        {
            _comicBookRepository = new SqlRepository<ComicBook>(_context);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
