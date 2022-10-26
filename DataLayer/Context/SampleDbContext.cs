using DataLayer.Entities.Access;
using DataLayer.Entities.Account;
using DataLayer.Entities.Order;
using DataLayer.Entities.Product;
using DataLayer.Entities.Site;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options):base(options)
        {
        }

        #region disable cascading delete in database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            
            base.OnModelCreating(modelBuilder);
        }
        #endregion


        #region Db Sets

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Slider> Sliders { get; set; }


        #region product dbsets

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<SpecialProduct> SpecialProducts { get; set; }
        public DbSet<ProductVisit> ProductVisits { get; set; }

        #endregion

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
