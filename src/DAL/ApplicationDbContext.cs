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

            builder.Entity<Appointment_User>()
              .HasKey(t => new { t.AppointmentId, t.UserId });

            builder.Entity<ApplicationUser_UserGroup>()
              .HasKey(t => new { t.Id, t.UserGroupId });

            builder.Entity<ApplicationUser>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser_UserGroup>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(ug => ug.ApplicationUser_UserGroups)
                .HasForeignKey(u => u.Id);

            builder.Entity<ApplicationUser_UserGroup>()
                .HasOne(ug => ug.UserGroup)
                .WithMany(u => u.ApplicationUser_UserGroups)
                .HasForeignKey(ug => ug.UserGroupId);

            builder.Entity<Appointment_User>()
                .HasOne(au => au.User)
                .WithMany(u => u.User_Appointments)
                .HasForeignKey(au => au.UserId);

            builder.Entity<Appointment_User>()
                .HasOne(au => au.Appointment)
                .WithMany(a => a.Appointment_Users)
                .HasForeignKey(au => au.AppointmentId);

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

            builder.Entity<UserTask>()
                .Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<UserTask>()
                .HasOne(x => x.ApplicationTask);

            builder.Entity<UserTask>()
                .HasOne(x => x.User);

            builder.Entity<Media>()
                .HasKey(media => media.Id);

            builder.Entity<Avatar>()
                .HasKey(avatar => avatar.AvatarId);

            builder.Entity<ApplicationUser_Avatar>()
                .HasKey(avatar => new { avatar.Id, avatar.AvatarId });

            builder.Entity<ApplicationUser_Avatar>()
                .HasOne(u => u.ApplicationUser)
                .WithMany(ug => ug.ApplicationUser_Avatars)
                .HasForeignKey(u => u.Id);

            builder.Entity<ApplicationUser_Avatar>()
                .HasOne(ug => ug.ApplicationUser)
                .WithMany(u => u.ApplicationUser_Avatars)
                .HasForeignKey(ug => ug.AvatarId);

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
        }

        public DbSet<ApplicationUser_Avatar> ApplicationUser_Avatars { get; set; }
        public DbSet<ApplicationUser_UserGroup> ApplicationUser_UserGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Appointment_User> AppointmentUsers { get; set; }
        public DbSet<ApplicationTask> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
    }
}