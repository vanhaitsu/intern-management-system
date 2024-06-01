using IMS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace IMS.Models
{
    public class AppDbContext : DbContext
    {
        //public class AppDbContext() : base("ConnectionStrings") { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<EmailSend> EmailSends { get; set; }
        public DbSet<EmailSendTrainee> EmailSendTrainees { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<ProgramCampaign> ProgramCampaigns { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

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
            modelBuilder.Entity<Assignment>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Campaign>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Document>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<EmailSend>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<EmailSendTrainee>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<EmailTemplate>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Lesson>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ProgramCampaign>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Role>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Score>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Trainee>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<TrainingProgram>().Property(e => e.Id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Lesson)
                .WithMany(l => l.Documents)
                .HasForeignKey(d => d.LessonId);

            modelBuilder.Entity<EmailSend>()
                .HasOne(es => es.Account)
                .WithMany(a => a.EmailSends)
                .HasForeignKey(es => es.SenderId);

            modelBuilder.Entity<EmailSend>()
                .HasOne(es => es.EmailTemplate)
                .WithMany(et => et.EmailSends)
                .HasForeignKey(es => es.TemplateId);

            modelBuilder.Entity<EmailSendTrainee>()
                .HasOne(est => est.Trainee)
                .WithMany(t => t.EmailSendTrainees)
                .HasForeignKey(est => est.ReceiveId);

            modelBuilder.Entity<EmailSendTrainee>()
                .HasOne(est => est.EmailSend)
                .WithMany(es => es.EmailSendTrainee)
                .HasForeignKey(est => est.EmailSendId);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Campaign)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CampaignId);

            modelBuilder.Entity<ProgramCampaign>()
                .HasOne(pc => pc.TrainingProgram)
                .WithMany(tp => tp.ProgramCampaigns)
                .HasForeignKey(pc => pc.ProgramId);

            modelBuilder.Entity<ProgramCampaign>()
                .HasOne(pc => pc.Campaign)
                .WithMany(c => c.ProgramCampaigns)
                .HasForeignKey(pc => pc.CampaignId);

            modelBuilder.Entity<Score>()
                .HasOne(s => s.Trainee)
                .WithMany(t => t.Scores)
                .HasForeignKey(s => s.TraineeId);

            modelBuilder.Entity<Trainee>()
                .HasOne(t => t.TrainingProgram)
                .WithMany(p => p.Trainees)
                .HasForeignKey(t => t.ProgramId);

            modelBuilder.Entity<TrainingProgram>()
                .HasOne(tp => tp.Account)
                .WithMany(a => a.TrainingPrograms)
                .HasForeignKey(tp => tp.AccountId);

            modelBuilder.Entity<TrainingProgram>()
                .HasOne(tp => tp.Assignment)
                .WithMany(a => a.TrainingPrograms)
                .HasForeignKey(tp => tp.AssignmentId);
        }

    }
}
