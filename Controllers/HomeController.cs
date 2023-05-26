using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OWNA_Test.Models;
using OWNA_Test.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OWNA_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookDataRepository _repository;

        public HomeController(BookDataRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            var result = _repository.FindBooks(search);

            var viewModel = new AuthorBookData
            {
                Authors = _repository.Authors,
                Books = result
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult CreateBook(string title, List<int> authorIDs, string description, string imageUrl)
        {
            var book = new Book
            {
                Title = title,
                Authors = new List<Author>(),
                Description = description,
                CoverImageUrl = imageUrl
            };

            foreach(var authorID in authorIDs)
            {
                var author = _repository.Authors.FirstOrDefault(a => a.Id == authorID);
                if (author != null)
                    book.Authors.Add(author);
            }

            _repository.AddBook(book);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateAuthor(string authorName)
        {
            var author = new Author
            {
                name = authorName
            };

            _repository.AddAuthor(author);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {

            var ViewModel = new AuthorBookData
            {
                Authors = _repository.Authors,
                Books = _repository.Books
            };

            return View(ViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
