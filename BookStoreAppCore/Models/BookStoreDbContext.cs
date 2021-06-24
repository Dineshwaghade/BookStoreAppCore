using BookStoreAppCore.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Models
{
    public class BookStoreDbContext:IdentityDbContext<ApplicationUser>
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options):base(options)
        {
                
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }

    }
}
