using Microsoft.EntityFrameworkCore;
using Student_Society_Management.Models;

namespace Student_Society_Management.Data
{
    public class SocietyDbContext : DbContext
    {
        public SocietyDbContext(DbContextOptions<SocietyDbContext> options) : base(options) { }

        public DbSet<Member> Members => Set<Member>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EventId, a.MemberId })
                .IsUnique();
        }
    }
}
