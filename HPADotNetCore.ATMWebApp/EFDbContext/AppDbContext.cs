using HPADotNetCore.ATMWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HPADotNetCore.ATMWebApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ATMDataModel> ATMDatas { get; set; }
    }
}
