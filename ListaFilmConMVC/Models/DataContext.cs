using Microsoft.EntityFrameworkCore;

namespace ListaFilmConMVC.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Film> Films { get; set; }

        public virtual DbSet<Picture> Pictures { get; set; }

    }
}
