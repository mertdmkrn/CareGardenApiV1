using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
 
namespace CareGardenApiV1.Repository
{
    public class CareGardenApiDbContext : DbContext
    {
        public CareGardenApiDbContext(DbContextOptions<CareGardenApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            builder.Entity<Appointment>(entity =>
            {

                entity.HasOne(d => d.business)
                .WithMany(p => p.appointments)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Appointment_Business_businessId")
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.user)
                .WithMany(p => p.appointments)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_Appointment_User_userId")
                .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<BusinessGallery>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.galleries)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessGallery_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BusinessProperties>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.properties)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessProperties_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BusinessServiceModel>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.services)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessService_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.service)
                .WithMany(p => p.businessServices)
                .HasForeignKey(d => d.serviceId)
                .HasConstraintName("FK_BusinessService_Services_serviceId")
                .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<BusinessWorkingInfo>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.workingInfos)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessWorkingInfo_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Campaign>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.campaigns)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Campaign_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.comments)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Comment_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.user)
                .WithMany(p => p.comments)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_Comment_User_userId")
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.reply)
                .WithMany()
                .HasForeignKey(d => d.replyId)
                .HasConstraintName("FK_Comment_Comment_replyId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Complain>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.complains)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Complain_Business_businessId")
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.user)
                .WithMany(p => p.complains)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_Complain_User_userId")
                .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<ConfirmationInfo>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.target);
                entity.Property(e => e.code);
                entity.Property(e => e.createDate);
            });

            builder.Entity<Favorite>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.favorites)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Favorite_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.user)
                .WithMany(p => p.favorites)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_Favorite_User_userId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PaymentInfo>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.paymentInfos)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_PaymentInfo_Business_businessId")
                .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Worker>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.workers)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Worker_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Discount>(entity =>
            {
                entity.HasOne(d => d.business)
                .WithMany(p => p.discounts)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Discount_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessGallery> BusinessGalleries { get; set; }
        public DbSet<BusinessProperties> BusinessProperties { get; set; }
        public DbSet<BusinessServiceModel> BusinessServices { get; set; }
        public DbSet<BusinessWorkingInfo> BusinessWorkingInfos { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Complain> Complains { get; set; }
        public DbSet<ConfirmationInfo> ConfirmationInfos { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
