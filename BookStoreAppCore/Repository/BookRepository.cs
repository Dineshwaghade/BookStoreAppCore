using BookStoreAppCore.Data;
using BookStoreAppCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;

        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
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
    }
}
