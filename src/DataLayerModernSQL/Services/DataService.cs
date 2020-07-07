using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// Note that you need to run PM> add-migration initial from the package manager console 

namespace DataLayerModernSQL.Services
{
    public class DataService : DbContext
    {
        private const string CONNSTRINGKEY = "sqlconnectionstring";
        readonly string _connectionString;

        //Required by data migrations
        public DataService()
        {

        }

        public DataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>(CONNSTRINGKEY);
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
        public string Con { get { return _connectionString; } }

        public void CheckConnectionString(ILogger logger)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                logger.LogError($"Connection string error: {CONNSTRINGKEY} is null");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _connectionString);
        }
    }
}


