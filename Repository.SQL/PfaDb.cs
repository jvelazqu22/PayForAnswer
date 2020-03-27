using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Repository.SQL
{
    [DbConfigurationType(typeof(CustomDbConfiguration))]
    public class PfaDb : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public PfaDb() : base("name=PfaDb") { }

        public static PfaDb Create() { return new PfaDb(); }
    
        public DbSet<Status> Status { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionPaymentDetail> QuestionPaymentDetails { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<MarketingCampaign> MarketingCampaigns { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<SearchKeyword> SearchKeywords { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
               .HasMany(u => u.Subjects).WithMany(s => s.Users)
               .Map(t => t.MapLeftKey("UserID")
                   .MapRightKey("SubjectID")
                   .ToTable("UserSubject"));

            // Configure the primary key for the OfficeAssignment 
            modelBuilder.Entity<MarketingCampaign>()
                .HasKey(t => t.QuestionPaymentDetailID);

            // Map one-to-zero or one relationship 
            modelBuilder.Entity<MarketingCampaign>()
                .HasRequired(t => t.QuestionPaymentDetail)
                .WithOptional(t => t.MarketingCampaign);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.Questions)
                .WithRequired(c => c.User)
                .WillCascadeOnDelete(false);
        }
    }
}
