using BookStoreAppCore.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreAppCore.Repository
{
    public interface IBookRepository
    {
        Task<int> AddNewBookAsync(BookModel model);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> EditBookAsync(BookModel model);
        Task<List<BookModel>> GetAllBooksAsync();
        Task<BookModel> GetBookByIdAsync(int id);
        Task<List<BookModel>> GetTopBooksAsync(int count);
        Task<List<LanguageModel>> GetAllLanguage();
        Task<string> UploadFile(string folder,IFormFile file,string existFilePath);
    }
}