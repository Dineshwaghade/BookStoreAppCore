using BookStoreAppCore.Models;
using BookStoreAppCore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            ViewBag.Languages = new SelectList(await _bookRepository.GetAllLanguage(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookModel model)
        {
            ViewBag.Languages = new SelectList(await _bookRepository.GetAllLanguage(), "Id", "Name");
            if (ModelState.IsValid)
            {
                int result = await _bookRepository.AddNewBookAsync(model);
                if (result > 0)
                {
                    ModelState.Clear();
                    ViewBag.isSuccess = true;
                    ViewBag.BookId = result;
                }

            }
            return View();
        }
        public async Task<IActionResult> GetAllBooks()
        {
            List<BookModel> books = new List<BookModel>();
            books = await _bookRepository.GetAllBooksAsync();
            return View(books);
        }
        [HttpGet]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _bookRepository.GetBookByIdAsync(id);
            ViewBag.SimilarBooks = await _bookRepository.GetAllBooksAsync();
            return View(result);
        }
        public async Task<IActionResult> BookList()
        {
            var result = await _bookRepository.GetAllBooksAsync();
            return View(result);
        }
        public async Task<IActionResult> EditBook(int id)
        {
            ViewBag.Languages = new SelectList(await _bookRepository.GetAllLanguage(), "Id", "Name");
            var result = await _bookRepository.GetBookByIdAsync(id);
            if(result!=null)
            {
                return View(result);
            }
            return RedirectToAction("BookList");
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookModel model)
        {
            ViewBag.Languages = new SelectList(await _bookRepository.GetAllLanguage(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var result = await _bookRepository.EditBookAsync(model);
                if (result)
                {
                    return RedirectToAction("BookList");
                }
            }
            return View();
        }
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result=await _bookRepository.DeleteBookAsync(id);
            return RedirectToAction("BookList");
        }
    }
}
