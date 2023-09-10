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
    }
}
