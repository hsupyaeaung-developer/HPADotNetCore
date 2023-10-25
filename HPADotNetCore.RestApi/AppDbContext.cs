
using HPADotNetCore.RestApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HPADotNetCore.RestApi
{
    public class AppDbContext : DbContext
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = ".",
            InitialCatalog = "AHMTZDotNetCore",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true
        };
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);
            }
        }
        public DbSet<BlogDataModel> Blogs { get; set; } 
    }
}
