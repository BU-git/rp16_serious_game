using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DAL;

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160503195311_Appointments")]
    partial class Appointments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.ApplicationTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Coins");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Recurency");

                    b.Property<string>("Text");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AvatarId");

                    b.Property<DateTime>("BirthDate");

                    b.Property<int>("Bsn");

                    b.Property<string>("BuildingNumber");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("Passport");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Region");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Street");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("Domain.Entities.ApplicationUser_UserGroup", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("UserGroupId");

                    b.HasKey("Id", "UserGroupId");
                });

            modelBuilder.Entity("Domain.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Domain.Entities.Appointment_User", b =>
                {
                    b.Property<int>("AppointmentId");

                    b.Property<string>("UserId");

                    b.Property<bool>("IsOwner");

                    b.HasKey("AppointmentId", "UserId");
                });

            modelBuilder.Entity("Domain.Entities.Avatar", b =>
                {
                    b.Property<int>("AvatarId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Level");

                    b.Property<int>("MediaId");

                    b.Property<int>("Type");

                    b.HasKey("AvatarId");
                });

            modelBuilder.Entity("Domain.Entities.Customer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<DateTime>("ActivationDate");

                    b.Property<DateTime>("BirthDate");

                    b.Property<DateTime>("Birthplace");

                    b.Property<int>("Contact");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DisactivationDate");

                    b.Property<bool>("IsActive");

                    b.Property<int>("RegisteredBy");

                    b.Property<int>("Resident");

                    b.Property<string>("SchoolGrade");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Domain.Entities.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Path");

                    b.Property<int>("Type");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Domain.Entities.UserGroup", b =>
                {
                    b.Property<int>("UserGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupName");

                    b.Property<int>("Type");

                    b.HasKey("UserGroupId");
                });

            modelBuilder.Entity("Domain.Entities.UserTask", b =>
                {
                    b.Property<int>("TaskId");

                    b.Property<string>("UserId");

                    b.Property<int>("Coins");

                    b.Property<DateTime>("ExpireDt");

                    b.Property<int>("Status");

                    b.Property<string>("Text");

                    b.HasKey("TaskId", "UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("Domain.Entities.ApplicationUser", b =>
                {
                    b.HasOne("Domain.Entities.Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");
                });

            modelBuilder.Entity("Domain.Entities.ApplicationUser_UserGroup", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");

                    b.HasOne("Domain.Entities.UserGroup")
                        .WithMany()
                        .HasForeignKey("UserGroupId");
                });

            modelBuilder.Entity("Domain.Entities.Appointment_User", b =>
                {
                    b.HasOne("Domain.Entities.Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId");

                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Domain.Entities.Avatar", b =>
                {
                    b.HasOne("Domain.Entities.Media")
                        .WithMany()
                        .HasForeignKey("MediaId");
                });

            modelBuilder.Entity("Domain.Entities.Customer", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id");
                });

            modelBuilder.Entity("Domain.Entities.UserTask", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationTask")
                        .WithMany()
                        .HasForeignKey("TaskId");

                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("Domain.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
