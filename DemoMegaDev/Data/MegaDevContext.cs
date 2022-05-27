using DemoMegaDev.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMegaDev.Data
{
    public class MegaDevContext : DbContext
    {
        public MegaDevContext(DbContextOptions<MegaDevContext> options) : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Image> Images { get; set; }
    }
}
