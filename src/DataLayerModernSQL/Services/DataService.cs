using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

// Note that you need to run PM> add-migration initial from the package manager console 

namespace DataLayerModernSQL.Services
{
    public class DataService : DbContext
    {
        readonly string _connectionString;

        //Required by data migrations
        public DataService()
        {

        }

        public DataService(IConfiguration configuration)
        {
            //TODO extract constant
            _connectionString = configuration.GetConnectionString("sqlconnectionstring");
        }

        public DataService(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public DataService(DbContextOptions<DataService> options)
        : base(options)
        {
        }

        public DbSet<ServiceDescription> Services { get; set; }

        public DbSet<LearningResource> LearningResources { get; set; }

        public bool BadConnectionString() => string.IsNullOrEmpty(_connectionString);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _connectionString);
        }
    }
}


