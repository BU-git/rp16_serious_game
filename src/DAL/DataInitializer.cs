using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class DataInitializer
    {
        private ApplicationDbContext context;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public DataInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task InitializeDataAsync()
        {
           
            await CreatRoles();
            await CreateUsersAsync();
            await CreateUserGroup();
        }

        private async Task CreatRoles()
        {
            var role1 = await roleManager.FindByNameAsync("Admin");
            if (role1 == null)
            {
                var AdminRole = new IdentityRole { Name = "Admin" };
                await roleManager.CreateAsync(AdminRole);
            }

            var role2 = await roleManager.FindByNameAsync("Participant");
            if (role2 == null)
            {
                var Participant = new IdentityRole { Name = "Participant" };
                await roleManager.CreateAsync(Participant);
            }

            var role3 = await roleManager.FindByNameAsync("Coach");
            if(role3== null)
            {
                var Coach = new IdentityRole { Name = "Coach" };
                await roleManager.CreateAsync(Coach);
            }

            var role4 = await roleManager.FindByNameAsync("Governor");
            if (role4 == null)
            {
                var Governor = new IdentityRole { Name = "Governor" };
                await roleManager.CreateAsync(Governor);
            }

         
        }

        private async Task CreateUsersAsync()
        {
            var user1 = await userManager.FindByEmailAsync("Admin@admin.com");
            if (user1 == null)
            {
                ApplicationUser admin = new ApplicationUser() { UserName = "Admin@admin.com", Name = "Admin1", Email = "Admin@admin.com", Gender = Gender.MALE };
                await userManager.CreateAsync(admin, "User2016!");
                await userManager.AddToRoleAsync(admin, "Admin");
  
            }

            var user2 = await userManager.FindByEmailAsync("Admin2@admin.com");
            if (user2 == null)
            {
                ApplicationUser admin2 = new ApplicationUser() { UserName = "Admin2@admin.com", Name = "Admin2", Email = "Admin2@admin.com" };
                await userManager.CreateAsync(admin2, "User2016!");
                await userManager.AddToRoleAsync(admin2, "Admin");

            }

            var user3 = await userManager.FindByEmailAsync("FirstCoach@coach.com");
            if (user3 == null)
            {
                ApplicationUser FirstCoach = new ApplicationUser() { UserName = "FirstCoach@coach.com", Name = "Mentor1", Email = "FirstCoach@coach.com" };
                await userManager.CreateAsync(FirstCoach, "User2016!");
                await userManager.AddToRoleAsync(FirstCoach, "Coach");
               
                
            }

            var user4 = await userManager.FindByEmailAsync("SecondCoach@coach.com");
            if (user4 == null)
            {
                ApplicationUser SecondCoach = new ApplicationUser() { UserName = "SecondCoach@coach.com", Name = "Mentor2" , Email = "SecondCoach@coach.com" };              
                await userManager.CreateAsync(SecondCoach, "User2016!");
                await userManager.AddToRoleAsync(SecondCoach, "Coach");

            }

            var user5 = await userManager.FindByEmailAsync("FirstParticipant@participant.com");
            if (user5 == null)
            {
                ApplicationUser Participant = new ApplicationUser() { UserName = "FirstParticipant@participant.com", Name = "Participant1", Email= "FirstParticipant@participant.com" };
                await userManager.CreateAsync(Participant, "User2016!");
                await userManager.AddToRoleAsync(Participant, "Participant");
            }

            var user6 = await userManager.FindByEmailAsync("SecondParticipant@participant.com");
            if (user6 == null)
            {
                ApplicationUser SecondParticipant = new ApplicationUser() { UserName = "SecondParticipant@participant.com", Name = "Participant2", Email = "SecondParticipant@participant.com" };
                await userManager.CreateAsync(SecondParticipant, "User2016!");
                await userManager.AddToRoleAsync(SecondParticipant, "Participant");
            }

            var user7 = await userManager.FindByEmailAsync("Governor@governor.com");
            if (user7 == null)
            {
                ApplicationUser Governor = new ApplicationUser() { UserName = "Governor@governor.com", Name = "Governor", Email = "Governor@governor.com" };
                await userManager.CreateAsync(Governor, "User2016!");
                await userManager.AddToRoleAsync(Governor, "Governor");
            }
            
        }

        private async Task CreateUserGroup()
        {
            if (!context.UserGroups.Any())
            {
                var UserGroup = new UserGroup();
                var user1 = await userManager.FindByEmailAsync("FirstParticipant@participant.com");
                var user2 = await userManager.FindByEmailAsync("SecondParticipant@participant.com");
                var coach = await userManager.FindByEmailAsync("FirstCoach@coach.com");
                

                if (user1!=null && user2!=null && coach!=null)
                {
                    ApplicationUser_UserGourp uug1 = new ApplicationUser_UserGourp();
                    ApplicationUser_UserGourp uug2 = new ApplicationUser_UserGourp();
                    ApplicationUser_UserGourp uug3 = new ApplicationUser_UserGourp();

                    uug1.ApplicationUser = user1;
                    uug1.UserGroup = UserGroup;

                    uug2.ApplicationUser = user2;
                    uug2.UserGroup = UserGroup;

                    uug3.ApplicationUser = coach;
                    uug3.UserGroup = UserGroup;

                    UserGroup.GroupName = "Test Family";
                    UserGroup.Type = UserGroupType.MARRIED;

                    context.Add(UserGroup);
                    context.Add(uug1);
                    context.Add(uug2);
                    context.Add(uug3);

                    context.SaveChanges();
                }
            }
        }
    }
}
