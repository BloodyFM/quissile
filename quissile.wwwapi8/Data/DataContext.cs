using Microsoft.EntityFrameworkCore;
using quissile.wwwapi8.Models;

namespace quissile.wwwapi8.Data
{
    public class DataContext : DbContext
    {
        private string _connectionString;

        public DataContext(DbContextOptions<DataContext> options) : base(options) {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seeder seeder = new Seeder();
            // Seed some example data
            modelBuilder.Entity<Question>().HasData(seeder.Questions);
            modelBuilder.Entity<Alternative>().HasData(seeder.Alternatives);
            modelBuilder.Entity<Quiz>().HasData(seeder.Quizes);

            modelBuilder.Entity<Question>().Navigation(x => x.Alternatives).AutoInclude();
            modelBuilder.Entity<Quiz>().Navigation(x => x.Questions).AutoInclude();
        }

        public DbSet<Alternative> Alternatives { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
    }
}
