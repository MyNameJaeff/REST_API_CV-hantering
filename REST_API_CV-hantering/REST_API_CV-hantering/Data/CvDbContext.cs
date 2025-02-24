using Microsoft.EntityFrameworkCore;
using REST_API_CV_hantering.Models;

namespace REST_API_CV_hantering.Data
{
    public class CVDbContext : DbContext
    {
        public CVDbContext(DbContextOptions<CVDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Person>().ToTable("Persons");
        //    modelBuilder.Entity<Education>().ToTable("Educations");
        //    modelBuilder.Entity<WorkExperience>().ToTable("WorkExperiences");
        //}
    }
}
