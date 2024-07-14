using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Connections
{
    public class TContext : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("server=DESKTOP-LAHHDS9;database=eticaret;integrated Security=true;TrustServerCertificate=true;");
            }
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<salesDetails>().HasKey(x => new { x.urunID, x.salesID,x.FeatureID,x.GroupID });

            builder.Entity<salesDetails>().HasOne(sd => sd.urun)
                .WithMany(u => u.SalesDetails).HasForeignKey(u => u.urunID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.Entity<salesDetails>().HasOne(sd => sd.sales)
                .WithMany(s => s.SalesDetails).HasForeignKey(s => s.salesID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

			builder.Entity<salesDetails>().HasOne(sd => sd.Feature)
				.WithMany(s => s.SalesDetails).HasForeignKey(s => s.FeatureID)
				.OnDelete(DeleteBehavior.Restrict).IsRequired();






			builder.Entity<chart>().HasOne(c => c.Urun)
                .WithMany(u => u.Charts).HasForeignKey(u => u.urunID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.Entity<chart>().HasOne(c => c.User)
                .WithMany(u => u.Charts).HasForeignKey(c => c.userID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();





            builder.Entity<UrunFeature>().HasKey(uf => new { uf.urunID, uf.featureID });

            builder.Entity<UrunFeature>().HasOne(uf=>uf.Urun).WithMany(u=>u.UrunFeatures).HasForeignKey(uf=>uf.urunID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.Entity<UrunFeature>().HasOne(uf => uf.Feature).WithMany(f => f.UrunFeatures).HasForeignKey(uf => uf.featureID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();





            builder.Entity<urun>().HasOne(u => u.category).WithMany(c => c.uruns).HasForeignKey(u => u.categoryID)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();





            builder.Entity<CategoryFeature>().HasKey(x => new { x.FeatureID, x.CategoryID });

            builder.Entity<CategoryFeature>().HasOne(cf=>cf.Feature).WithMany(f=>f.CategoryFeatures).HasForeignKey(cf=>cf.FeatureID)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict).IsRequired();

            builder.Entity<CategoryFeature>().HasOne(cf => cf.Category).WithMany(c => c.CategoryFeatures).HasForeignKey(cf => cf.CategoryID)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict).IsRequired();




            builder.Entity<Message>().HasOne(m=>m.SendUser).WithMany(su=>su.SentMessages)
                .HasForeignKey(m=>m.SendUserID).OnDelete(DeleteBehavior.Restrict).IsRequired();

            builder.Entity<Message>().HasOne(m => m.ReceiveUser).WithMany(su => su.ReceivedMessages)
                .HasForeignKey(m => m.ReceiveUserID).OnDelete(DeleteBehavior.Restrict).IsRequired();





            builder.Entity<ChartFeature>().HasKey(x => new { x.featureID, x.chartID });

            builder.Entity<ChartFeature>().HasOne(cf => cf.Feature).WithMany(f => f.ChartFeatures)
                .HasForeignKey(cf => cf.featureID).OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.Entity<ChartFeature>().HasOne(cf => cf.Chart).WithMany(c => c.ChartFeatures)
                .HasForeignKey(cf => cf.chartID).OnDelete(DeleteBehavior.Cascade).IsRequired();



            base.OnModelCreating(builder);

        }

        public DbSet<urun> Uruns { get; set; }
        public DbSet<category> Categories { get; set; }
        public DbSet<sales> Sales { get; set; }
        public DbSet<salesDetails> SalesDetails { get; set; }
        public DbSet<chart> Charts { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Feature> Features { get; set; }
        public DbSet<UrunFeature> UrunFeatures { get; set; }
        public DbSet<urunImage> UrunImages { get; set; }

        public DbSet<CategoryFeature> CategoryFeatures { get; set; }

        public DbSet<ChartFeature> ChartFeatures { get; set; }


    }
}
