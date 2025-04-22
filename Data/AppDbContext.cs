using Microsoft.EntityFrameworkCore;
using SuperNotesBackend.Models;

namespace SuperNotesBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
    }
}