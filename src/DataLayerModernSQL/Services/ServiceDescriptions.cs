using Microsoft.EntityFrameworkCore;

namespace DataLayerModernSQL.Services
{
    public class DataService : DbContext
    {
        readonly string _connectionString;
        public DataService(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public DbSet<ServiceDescription> Services { get; set; }
        public DbSet<LearningResource> LearningResources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=(localdb)\mssqllocaldb;Database=Blogging;Integrated Security=True");
        }
    }
}
