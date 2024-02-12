using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
 
namespace CareGardenApiV1.Repository
{
    public class CareGardenApiDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(HelperMethods.GetConfiguration()["ConnectionStrings:AWSPostgreSQL"], x => x.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            builder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.name);
                entity.Property(e => e.description);
                entity.Property(e => e.descriptionEn);
                entity.Property(e => e.city);
                entity.Property(e => e.province);
                entity.Property(e => e.district);
                entity.Property(e => e.address);
                entity.Property(e => e.telephone);
                entity.Property(e => e.email);
                entity.Property(e => e.password);
                entity.Property(e => e.latitude);
                entity.Property(e => e.longitude);
                entity.Property(e => e.location);
                entity.Property(e => e.createDate);
                entity.Property(e => e.updateDate);
                entity.Property(e => e.workingGenderType);
                entity.Property(e => e.appointmentTimeInterval);
                entity.Property(e => e.appointmentPeopleCount);
                entity.Property(e => e.officialHolidayAvailable);
                entity.Property(e => e.isActive);
                entity.Property(e => e.verified);
                entity.Property(e => e.isFeatured);
                entity.Property(e => e.hasPromotion);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.fullName);
                entity.Property(e => e.email);
                entity.Property(e => e.telephone);
                entity.Property(e => e.password);
                entity.Property(e => e.city);
                entity.Property(e => e.gender);
                entity.Property(e => e.createDate);
                entity.Property(e => e.updateDate);
                entity.Property(e => e.birthDate);
                entity.Property(e => e.services);
                entity.Property(e => e.role);
                entity.Property(e => e.imageUrl);
                entity.Property(e => e.isBan);
                entity.Property(e => e.latitude);
                entity.Property(e => e.longitude);
                entity.Property(e => e.location);
            });

            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.createDate);
                entity.Property(e => e.updateDate);
                entity.Property(e => e.startDate);
                entity.Property(e => e.status);
                entity.Property(e => e.description);
                entity.Property(e => e.userId);
                entity.Property(e => e.businessId);

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
                entity.HasKey(e => e.id);

                entity.Property(e => e.imageUrl);
                entity.Property(e => e.size);
                entity.Property(e => e.isProfilePhoto);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.galleries)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessGallery_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BusinessProperties>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.key);
                entity.Property(e => e.value);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.properties)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessProperties_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<BusinessServiceModel>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.name);
                entity.Property(e => e.nameEn);
                entity.Property(e => e.spot);
                entity.Property(e => e.spotEn);
                entity.Property(e => e.maxDuration);
                entity.Property(e => e.minDuration);
                entity.Property(e => e.price);
                entity.Property(e => e.serviceId);
                entity.Property(e => e.businessId);

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
                entity.HasKey(e => e.id);

                entity.Property(e => e.mondayWorkHours);
                entity.Property(e => e.tuesdayWorkHours);
                entity.Property(e => e.wednesdayWorkHours);
                entity.Property(e => e.thursdayWorkHours);
                entity.Property(e => e.fridayWorkHours);
                entity.Property(e => e.saturdayWorkHours);
                entity.Property(e => e.sundayWorkHours);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.workingInfos)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_BusinessWorkingInfo_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.path);
                entity.Property(e => e.url);
                entity.Property(e => e.isActive);
                entity.Property(e => e.createDate);
                entity.Property(e => e.updateDate);
                entity.Property(e => e.sortOrder);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.campaigns)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Campaign_Business_businessId")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.comment);
                entity.Property(e => e.commentType);
                entity.Property(e => e.createDate);
                entity.Property(e => e.updateDate);
                entity.Property(e => e.point);
                entity.Property(e => e.userId);
                entity.Property(e => e.businessId);
                entity.Property(e => e.replyId);

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
                entity.HasKey(e => e.id);

                entity.Property(e => e.date);
                entity.Property(e => e.description);
                entity.Property(e => e.businessId);
                entity.Property(e => e.userId);
 
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
                entity.HasKey(e => e.id);

                entity.Property(e => e.userId);
                entity.Property(e => e.businessId);

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
                entity.HasKey(e => e.id);

                entity.Property(e => e.description);
                entity.Property(e => e.date);
                entity.Property(e => e.payDate);
                entity.Property(e => e.amount);
                entity.Property(e => e.payAmount);
                entity.Property(e => e.receiptFilePath);
                entity.Property(e => e.paidType);
                entity.Property(e => e.isPaid);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.paymentInfos)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_PaymentInfo_Business_businessId")
                .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.name);
                entity.Property(e => e.nameEn);
                entity.Property(e => e.className);
                entity.Property(e => e.colorCode);
                entity.Property(e => e.sortOrder);
            });

            builder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.question);
                entity.Property(e => e.questionEn);
                entity.Property(e => e.answer);
                entity.Property(e => e.answerEn);
                entity.Property(e => e.category);
                entity.Property(e => e.categoryEn);
                entity.Property(e => e.sortOrder);
            });

            builder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.name);
                entity.Property(e => e.title);
                entity.Property(e => e.path);
                entity.Property(e => e.serviceIds);
                entity.Property(e => e.isAvailable);
                entity.Property(e => e.isActive);
                entity.Property(e => e.createdUserId);
                entity.Property(e => e.businessId);

                entity.HasOne(d => d.business)
                .WithMany(p => p.workers)
                .HasForeignKey(d => d.businessId)
                .HasConstraintName("FK_Worker_Business_businessId")
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
