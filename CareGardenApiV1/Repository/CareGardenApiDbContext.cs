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
        public DbSet<Services> Services { get; set; }
        public DbSet<ConfirmationInfo> ConfirmationInfos { get; set; }
    }
}
