using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Domain.Entities;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserUserGroup>()
              .HasKey(t => new { t.Id, t.UserGoupId });

            builder.Entity<ApplicationUser>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<ApplicationUserUserGroup>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(ug => ug.ApplicationUserUserGroups)
                .HasForeignKey(u => u.Id);

            builder.Entity<ApplicationUserUserGroup>()
                .HasOne(ug => ug.UserGroup)
                .WithMany(u => u.ApplicationUserUserGroups)
                .HasForeignKey(ug => ug.UserGoupId);

            builder.Entity<Customer>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(c => c.Customer)
                .HasForeignKey(x => x.Id);

            builder.Entity<ApplicationTask>()
                .HasMany(ut => ut.UserTasks)
                .WithOne(t => t.ApplicationTask)
                .HasForeignKey(t => t.TaskId);

            builder.Entity<ApplicationUser>()
                .HasMany(ut => ut.UserTasks)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Avatar)
                .WithMany(avatar => avatar.ApplicationUsers)
                .HasForeignKey(user => user.AvatarId);

            builder.Entity<UserTask>()
                .HasKey(ut => new {ut.TaskId, ut.UserId});

            builder.Entity<Media>()
                .HasKey(media => media.Id);

            builder.Entity<Avatar>()
                .HasKey(avatar => new {avatar.Type, avatar.Level});

            builder.Entity<Media>()
                .HasMany(media => media.Avatars)
                .WithOne(avatar => avatar.Media)
                .HasForeignKey(avatar => avatar.MediaId);

            builder.Entity<ApplicationUser>().Ignore(x => x.AccessFailedCount);
            builder.Entity<ApplicationUser>().Ignore(x => x.EmailConfirmed);
            builder.Entity<ApplicationUser>().Ignore(x => x.AccessFailedCount);
            builder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumberConfirmed);
            builder.Entity<ApplicationUser>().Ignore(x => x.LockoutEnabled);
            builder.Entity<ApplicationUser>().Ignore(x => x.LockoutEnd);
            builder.Entity<ApplicationUser>().Ignore(x => x.TwoFactorEnabled);
            builder.Entity<ApplicationUser>().Ignore(x => x.SecurityStamp);

            
        }


        public DbSet<Customer> Customers {get;set;}
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<ApplicationTask> Tasks { get; set; } 
        public DbSet<UserTask> UserTasks { get; set; } 
        public DbSet<Media> Medias { get; set; }
        public DbSet<Avatar> Avatars { get; set; }  

    }
}
