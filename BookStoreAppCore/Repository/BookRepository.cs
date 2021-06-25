using BookStoreAppCore.Data;
using BookStoreAppCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookRepository(BookStoreDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<int> AddNewBookAsync(BookModel model)
        {
            Book book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                TotalPages = model.TotalPages,
                Category = model.Category,
                LanguageId=model.LanguageId,
                Price = model.Price,
                CoverImageURL = model.CoverImageURL,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }
        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            var book = await _context.Books.Select(model =>
            new BookModel()
            {
                Id=model.Id,
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                TotalPages = model.TotalPages,
                Category = model.Category,
                LanguageId=model.LanguageId,
                Language=model.Language.Name,
                Price = model.Price,
                CoverImageURL = model.CoverImageURL,
                CreatedOn = model.CreatedOn,
                UpdatedOn = model.UpdatedOn
            }).ToListAsync();
            return book;
        }
        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.Select(model =>
                        new BookModel()
                        {
                            Id=model.Id,
                            Title = model.Title,
                            Author = model.Author,
                            Description = model.Description,
                            TotalPages = model.TotalPages,
                            Category = model.Category,
                            LanguageId = model.LanguageId,
                            Language=model.Language.Name,
                            Price = model.Price,
                            CoverImageURL = model.CoverImageURL,
                            CreatedOn = model.CreatedOn,
                            UpdatedOn = model.UpdatedOn
                        })
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return book;
        }
        public async Task<bool> EditBookAsync(BookModel model)
        {
            Book book = new Book()
            {
                Id = model.Id,
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                TotalPages = model.TotalPages,
                Category = model.Category,
                LanguageId = model.LanguageId,
                Price = model.Price,
                CoverImageURL = model.CoverImageURL,
                CreatedOn = model.CreatedOn,
                UpdatedOn = DateTime.UtcNow
            };
            var result = _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = _context.Books.Find(id);
            var result = _context.Entry(book).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<List<BookModel>> GetTopBooksAsync(int count)
        {
            List<BookModel> topBooks = new List<BookModel>();
            topBooks = await _context.Books.Select(model =>
              new BookModel()
              {
                  Id = model.Id,
                  Title = model.Title,
                  Author = model.Author,
                  Description = model.Description,
                  TotalPages = model.TotalPages,
                  Category = model.Category,
                  //Language=model.Language,
                  Price = model.Price,
                  CoverImageURL = model.CoverImageURL,
                  CreatedOn = model.CreatedOn,
                  UpdatedOn = model.UpdatedOn,

              }).Take(count).ToListAsync();
            return topBooks;
        }
        public async Task<List<LanguageModel>> GetAllLanguage()
        {
            List<LanguageModel> languages = new List<LanguageModel>();
            languages = await _context.Languages.Select(x=>
            new LanguageModel()
            {
                Id=x.Id,
                Name=x.Name,
                Description=x.Description
            })
                .ToListAsync();
            return languages;
        }
        public async Task<string> UploadFile(string folder,IFormFile file,string existFilePath)
        {
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;
            string FolderPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            await file.CopyToAsync(new FileStream(FolderPath, FileMode.Create));

            //to remove existing file
            //if(!string.IsNullOrEmpty(existFilePath))
            //{
            //    existFilePath = existFilePath.Substring(1);
            //    string FolderPath2 = Path.Combine(_webHostEnvironment.WebRootPath, existFilePath);
            //    if (System.IO.File.Exists(FolderPath2))
            //    {
            //        System.IO.File.Delete(FolderPath2);
            //    }
            //}
            return "/" + folder;
        }
    }
}
