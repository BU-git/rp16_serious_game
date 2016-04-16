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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task InitializeDataAsync()
        {
           
            await CreatRoles();
            await CreateUsersAsync();
            await CreateUserGroup();
        }

        private async Task CreatRoles()
        {
            var role1 = await _roleManager.FindByNameAsync("Admin");
            if (role1 == null)
            {
                var adminRole = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(adminRole);
            }

            var role2 = await _roleManager.FindByNameAsync("Participant");
            if (role2 == null)
            {
                var participant = new IdentityRole { Name = "Participant" };
                await _roleManager.CreateAsync(participant);
            }

            var role3 = await _roleManager.FindByNameAsync("Coach");
            if(role3== null)
            {
                var coach = new IdentityRole { Name = "Coach" };
                await _roleManager.CreateAsync(coach);
            }

            var role4 = await _roleManager.FindByNameAsync("Governor");
            if (role4 == null)
            {
                var governor = new IdentityRole { Name = "Governor" };
                await _roleManager.CreateAsync(governor);
            }

         
        }

        private async Task CreateUsersAsync()
        {
            var user1 =
                await _userManager.FindByEmailAsync("Admin@admin.com");
            if (user1 == null)
            {
                ApplicationUser admin = new ApplicationUser() { UserName = "Admin@admin.com", Name = "Admin1", Email = "Admin@admin.com", Gender = Gender.Male };
                await _userManager.CreateAsync(admin, "User2016!");
                await _userManager.AddToRoleAsync(admin, "Admin");
  
            }

            var user2 = await _userManager.FindByEmailAsync("Admin2@admin.com");
            if (user2 == null)
            {
                ApplicationUser admin2 = new ApplicationUser() { UserName = "Admin2@admin.com", Name = "Admin2", Email = "Admin2@admin.com" };
                await _userManager.CreateAsync(admin2, "User2016!");
                await _userManager.AddToRoleAsync(admin2, "Admin");
            }

            var user3 = await _userManager.FindByEmailAsync("FirstCoach@coach.com");
            if (user3 == null)
            {
                ApplicationUser firstCoach = new ApplicationUser() { UserName = "FirstCoach@coach.com", Name = "Mentor1", Email = "FirstCoach@coach.com" };
                await _userManager.CreateAsync(firstCoach, "User2016!");
                await _userManager.AddToRoleAsync(firstCoach, "Coach");
               
                
            }

            var user4 = await _userManager.FindByEmailAsync("SecondCoach@coach.com");
            if (user4 == null)
            {
                ApplicationUser secondCoach = new ApplicationUser() { UserName = "SecondCoach@coach.com", Name = "Mentor2" , Email = "SecondCoach@coach.com" };              
                await _userManager.CreateAsync(secondCoach, "User2016!");
                await _userManager.AddToRoleAsync(secondCoach, "Coach");

            }

            var user5 = await _userManager.FindByEmailAsync("FirstParticipant@participant.com");
            if (user5 == null)
            {
                ApplicationUser participant = new ApplicationUser() { UserName = "FirstParticipant@participant.com", Name = "Participant1", Email= "FirstParticipant@participant.com" };
                await _userManager.CreateAsync(participant, "User2016!");
                await _userManager.AddToRoleAsync(participant, "Participant");
            }

            var user6 = await _userManager.FindByEmailAsync("SecondParticipant@participant.com");
            if (user6 == null)
            {
                ApplicationUser secondParticipant = new ApplicationUser() { UserName = "SecondParticipant@participant.com", Name = "Participant2", Email = "SecondParticipant@participant.com" };
                await _userManager.CreateAsync(secondParticipant, "User2016!");
                await _userManager.AddToRoleAsync(secondParticipant, "Participant");
            }

            var user7 = await _userManager.FindByEmailAsync("Governor@governor.com");
            if (user7 == null)
            {
                ApplicationUser governor = new ApplicationUser() { UserName = "Governor@governor.com", Name = "Governor", Email = "Governor@governor.com" };
                await _userManager.CreateAsync(governor, "User2016!");
                await _userManager.AddToRoleAsync(governor, "Governor");
            }
            
        }

        private async Task CreateUserGroup()
        {
            if (!_context.UserGroups.Any())
            {
                var userGroup = new UserGroup();
                var user1 = await _userManager.FindByEmailAsync("FirstParticipant@participant.com");
                var user2 = await _userManager.FindByEmailAsync("SecondParticipant@participant.com");
                var coach = await _userManager.FindByEmailAsync("FirstCoach@coach.com");
                

                if (user1!=null && user2!=null && coach!=null)
                {
                    ApplicationUser_UserGroup uug1 = new ApplicationUser_UserGroup();
                    ApplicationUser_UserGroup uug2 = new ApplicationUser_UserGroup();
                    ApplicationUser_UserGroup uug3 = new ApplicationUser_UserGroup();

                    uug1.ApplicationUser = user1;
                    uug1.UserGroup = userGroup;

                    uug2.ApplicationUser = user2;
                    uug2.UserGroup = userGroup;

                    uug3.ApplicationUser = coach;
                    uug3.UserGroup = userGroup;

                    userGroup.GroupName = "Test Family";
                    userGroup.Type = UserGroupType.Married;

                    _context.Add(userGroup);
                    _context.Add(uug1);
                    _context.Add(uug2);
                    _context.Add(uug3);

                    _context.SaveChanges();
                }
            }
        }
    }
}

