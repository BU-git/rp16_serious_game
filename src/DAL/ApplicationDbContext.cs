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

            builder.Entity<ApplicationUser_UserGroup>()
              .HasKey(t => new { t.Id, t.UserGoupId });

            builder.Entity<ApplicationUser>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser_UserGroup>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(ug => ug.ApplicationUserUserGroups)
                .HasForeignKey(u => u.Id);

            builder.Entity<ApplicationUser_UserGroup>()
                .HasOne(ug => ug.UserGroup)
                .WithMany(u => u.ApplicationUserUserGroups)
                .HasForeignKey(ug => ug.UserGoupId);


            builder.Entity<Customer>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(c => c.Customer)
                .HasForeignKey(x => x.Id);

            builder.Entity<ApplicationUser>().Ignore(x => x.AccessFailedCount);
            builder.Entity<ApplicationUser>().Ignore(x => x.EmailConfirmed);
            builder.Entity<ApplicationUser>().Ignore(x => x.AccessFailedCount);
            builder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumberConfirmed);
            builder.Entity<ApplicationUser>().Ignore(x => x.LockoutEnabled);
            builder.Entity<ApplicationUser>().Ignore(x => x.LockoutEnd);
            builder.Entity<ApplicationUser>().Ignore(x => x.TwoFactorEnabled);
            builder.Entity<ApplicationUser>().Ignore(x => x.NormalizedEmail);
            builder.Entity<ApplicationUser>().Ignore(x => x.NormalizedUserName);
            builder.Entity<ApplicationUser>().Ignore(x => x.SecurityStamp);


        }


        public DbSet<Customer> Customers {get;set;}
        public DbSet<UserGroup> UserGroups { get; set; }
       

    }
}
