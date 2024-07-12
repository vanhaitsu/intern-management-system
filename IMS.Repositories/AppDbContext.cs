using IMS.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories
{
    public class AppDbContext : DbContext
    {
        //public class AppDbContext() : base("ConnectionStrings") { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Intern> Interns { get; set; }
        public DbSet<Mentorship> Mentorships { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configurations can be added here if needed.
            // Example:
            // modelBuilder.Entity<Account>()
            //     .HasMany(a => a.AccountRoles)
            //     .WithOne(ar => ar.Account)
            //     .HasForeignKey(ar => ar.AccountId);

            //modelBuilder.Entity<BaseEntity>(entity =>
            //{
            //    entity.Property(e => e.Id)
            //          .ValueGeneratedOnAdd()
            //          .HasDefaultValueSql("NEWID()");
            //});
            modelBuilder.Entity<Account>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Role>().Property(e => e.Id).HasDefaultValueSql("NEWID()");

        }

    }
}
