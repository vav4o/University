using Microsoft.EntityFrameworkCore;
using University.Entities;

namespace University.Repositories
{
    public class UniversityDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserToSubject> UserToSubjects { get; set; }
        public DbSet<Exam> Exams { get; set; }

        public UniversityDBContext() 
        {
            this.Users = this.Set<User>();
            this.Subjects = this.Set<Subject>();
            this.UserToSubjects = this.Set<UserToSubject>();
            this.Exams = this.Set<Exam>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-D1647C2\SQLEXPRESS;Database=UniversityDB;Trusted_Connection=True;TrustServerCertificate=true;").UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Username = "vav4o",
                    Password = "555",
                    FirstName = "Vasil",
                    LastName = "Nikolov"
                });
        }
    }
}
