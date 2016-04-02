using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Domain.Entities;
using Microsoft.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser_UserGourp>()
              .HasKey(t => new { t.Id, t.UserGoupId });

            builder.Entity<ApplicationUser>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser_UserGourp>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(ug => ug.ApplicationUser_UserGourps)
                .HasForeignKey(u => u.Id);

            builder.Entity<ApplicationUser_UserGourp>()
                .HasOne(ug => ug.UserGroup)
                .WithMany(u => u.ApplicationUser_UserGourps)
                .HasForeignKey(ug => ug.UserGoupId);


            builder.Entity<Customer>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(c => c.Customer)
                .HasForeignKey(x => x.Id);

        }


        public DbSet<Customer> Customers {get;set;}
        public DbSet<UserGroup> UserGroups { get; set; }
       

    }
}
