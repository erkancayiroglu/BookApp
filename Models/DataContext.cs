using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookApp.Models
{
    public class DataContext:DbContext

    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Book> Books { get; set; } = null!;

    }
}
