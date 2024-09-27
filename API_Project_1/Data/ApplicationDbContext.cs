using API_Project_1.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Project_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {

        }
        public DbSet<Brand>  Product { get; set; }
        public DbSet<FileUpload> Upload { get; set; }
        public DbSet<Registration> Regster { get; set; }


    }
}
