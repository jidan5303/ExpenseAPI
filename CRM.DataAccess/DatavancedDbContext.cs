using CRM.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.DataAccess;

#pragma warning disable CS8618
public class DatavancedDbContext : DbContext
{
    public DatavancedDbContext(DbContextOptions<DatavancedDbContext> options) : base(options)
    {

    }


    public DbSet<Employee> Employees { get; set; }
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Designation>().HasData(new Designation { Id = 1, Title = "Software Engineer" });
        modelBuilder.Entity<Designation>().HasData(new Designation { Id = 2, Title = "Senior Software Engineer" });
        modelBuilder.Entity<Designation>().HasData(new Designation { Id = 3, Title = "Principal Software Engineer" });
    }

}