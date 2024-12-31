using campus.AdditionalServices.SeedingData;
using campus.DBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace campus.DBContext
{
    public class AppDBContext : DbContext
    {
        
        private readonly DataSeeder _seeder;
        public AppDBContext(DbContextOptions<AppDBContext> options, DataSeeder seeder) : base(options)
        {
            _seeder = seeder;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasIndex(acc => acc.Email)
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasIndex(group => group.Id)
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasOne(course => course.Group)
                .WithMany(group => group.Courses)
                .HasForeignKey(course => course.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            
            modelBuilder.Entity<Student>()
                .HasKey(student => new { student.AccountId, student.CourseId });

            modelBuilder.Entity<Student>()
                .HasOne(student => student.Account)
                .WithMany(account => account.MyCourses)
                .HasForeignKey(student => student.AccountId);

            modelBuilder.Entity<Student>()
                .HasOne(student => student.Course)
                .WithMany(course => course.Students)
                .HasForeignKey(student => student.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            modelBuilder.Entity<Teacher>()
                .HasKey(teacher => new { teacher.AccountId, teacher.CourseId });

            modelBuilder.Entity<Teacher>()
                .HasOne(teacher => teacher.Account)
                .WithMany(account => account.TeachingCourses)
                .HasForeignKey(teacher => teacher.AccountId);

            modelBuilder.Entity<Teacher>()
                .HasOne(teacher => teacher.Course)
                .WithMany(course => course.Teachers)
                .HasForeignKey(teacher => teacher.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Course>()
                .HasIndex(course => course.Id)
                .IsUnique();

            modelBuilder.Entity<Notification>()
                .HasOne(notification => notification.Course)
                .WithMany(course => course.Notifications)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Account>().HasData(_seeder.GetAccounts());
            modelBuilder.Entity<Group>().HasData(_seeder.GetGroups());
            modelBuilder.Entity<Course>().HasData(_seeder.GetCourses());
            modelBuilder.Entity<Student>().HasData(_seeder.GetStudents());
            modelBuilder.Entity<Teacher>().HasData(_seeder.GetTeachers());
            modelBuilder.Entity<Notification>().HasData(_seeder.GetNotifications());
        }
    }
}