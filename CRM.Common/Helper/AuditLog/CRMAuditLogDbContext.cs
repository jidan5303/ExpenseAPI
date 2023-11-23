
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Common.Helper.AuditLog
{
#pragma warning disable CS8618
    public class CRMAuditLogDbContext : DbContext
    {
        public CRMAuditLogDbContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = DBConnection.Configuration.GetConnectionString("CRMAuditLog");

            optionsBuilder.UseSqlServer(conn);
        }

        public virtual DbSet<AuditLogMain> AuditLogMain { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLogMain>(entity =>
            {
                entity.HasKey(x => x.AuditLogMainID);
                entity.ToTable("AuditLogMain");
            });
            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.HasKey(x => x.ExceptionLogID);
                entity.ToTable("ExceptionLog");
            });

        }


    }

    public static class DBConnection
    {
        private static IConfiguration config;
        public static IConfiguration Configuration
        {
            get
            {
                if (config == null)
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                    config = builder.Build();

                }                
                return config;
            }
        }
    }

#pragma warning restore CS8618
}
