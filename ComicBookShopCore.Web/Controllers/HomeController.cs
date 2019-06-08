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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ComicBookShopCore.Web.Controllers
{
    public class HomeController : Controller
    {

        private IRepository<ComicBook> _comicBookRepository;
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        public IActionResult Index()
        {
            var model = new IndexViewModel(_comicBookRepository);
            return View(model);
        }

        [HttpGet("comics/{id}/{page}", Name = "ComicBookList")]
        public IActionResult ComicBookList(int? id, int? page, string searchWord)
        {

            if (!id.HasValue | !page.HasValue)
                return RedirectToAction("Index");

            var model = new ComicBookListViewModel(_comicBookRepository, id.Value, page.Value, searchWord);

            return View(model);
        }

        [HttpGet("/comic/{id}", Name = "ComicBookDetails")]
        public IActionResult ComicBookDetails(int? id)
        {
            if (!id.HasValue )
                return RedirectToAction("Index");

            var comic = _comicBookRepository.GetById(id.Value);
            if (comic == null)
                return RedirectToAction("Index");

            comic = _comicBookRepository.GetAll().Include(x => x.Series).ThenInclude(x => x.Publisher)
                .Include(x => x.ComicBookArtists).ThenInclude(x => x.Artist).First(x=> x.Id == id.Value);

            var model = new ComicBookDetailsViewModel(comic);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public HomeController(UserManager<User> userManager, SignInManager<User> signManager, IRepository<ComicBook> comicBookRepository)
        {
            _comicBookRepository = comicBookRepository;
            _userManager = userManager;
            _signManager = signManager;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
