using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;

namespace DAL
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDAL _dal;

        public DataInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager , IDAL dal)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _dal = dal;
        }

        public async Task InitializeDataAsync()
        {

            await CreatRoles();
            await CreateUsersAsync();
            await CreateUserGroup();
            await CreateTasks();
        }

        private async Task CreatRoles()
        {
            var role1 = await _roleManager.FindByNameAsync("Admin");
            if (role1 == null)
            {
                var adminRole = new IdentityRole {Name = "Admin"};
                await _roleManager.CreateAsync(adminRole);
            }

            var role2 = await _roleManager.FindByNameAsync("Participant");
            if (role2 == null)
            {
                var participant = new IdentityRole {Name = "Participant"};
                await _roleManager.CreateAsync(participant);
            }

            var role3 = await _roleManager.FindByNameAsync("Coach");
            if (role3 == null)
            {
                var coach = new IdentityRole {Name = "Coach"};
                await _roleManager.CreateAsync(coach);
            }

            var role4 = await _roleManager.FindByNameAsync("Governor");
            if (role4 == null)
            {
                var governor = new IdentityRole {Name = "Governor"};
                await _roleManager.CreateAsync(governor);
            }


        }

        private async Task CreateUsersAsync()
        {
            var user1 = await _userManager.FindByEmailAsync("Admin@admin.com");
            if (user1 == null)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "Admin@admin.com",
                    Name = "Admin1",
                    Email = "Admin@admin.com",
                    Gender = Gender.Male
                };
                await _userManager.CreateAsync(admin, "User2016!");
                await _userManager.AddToRoleAsync(admin, "Admin");

            }

            var user2 = await _userManager.FindByEmailAsync("Admin2@admin.com");
            if (user2 == null)
            {
                var admin2 = new ApplicationUser()
                {
                    UserName = "Admin2@admin.com",
                    Name = "Admin2",
                    Email = "Admin2@admin.com"
                };
                await _userManager.CreateAsync(admin2, "User2016!");
                await _userManager.AddToRoleAsync(admin2, "Admin");

            }

            var user3 = await _userManager.FindByEmailAsync("FirstCoach@coach.com");
            if (user3 == null)
            {
                var firstCoach = new ApplicationUser()
                {
                    UserName = "FirstCoach@coach.com",
                    Name = "Mentor1",
                    Email = "FirstCoach@coach.com"
                };
                await _userManager.CreateAsync(firstCoach, "User2016!");
                await _userManager.AddToRoleAsync(firstCoach, "Coach");


            }

            var user4 = await _userManager.FindByEmailAsync("SecondCoach@coach.com");
            if (user4 == null)
            {
                var secondCoach = new ApplicationUser()
                {
                    UserName = "SecondCoach@coach.com",
                    Name = "Mentor2",
                    Email = "SecondCoach@coach.com"
                };
                await _userManager.CreateAsync(secondCoach, "User2016!");
                await _userManager.AddToRoleAsync(secondCoach, "Coach");

            }

            var user5 = await _userManager.FindByEmailAsync("FirstParticipant@participant.com");
            if (user5 == null)
            {
                var participant = new ApplicationUser()
                {
                    UserName = "FirstParticipant@participant.com",
                    Name = "Participant1",
                    Email = "FirstParticipant@participant.com"
                };
                await _userManager.CreateAsync(participant, "User2016!");
                await _userManager.AddToRoleAsync(participant, "Participant");
            }

            var user6 = await _userManager.FindByEmailAsync("SecondParticipant@participant.com");
            if (user6 == null)
            {
                var secondParticipant = new ApplicationUser()
                {
                    UserName = "SecondParticipant@participant.com",
                    Name = "Participant2",
                    Email = "SecondParticipant@participant.com"
                };
                await _userManager.CreateAsync(secondParticipant, "User2016!");
                await _userManager.AddToRoleAsync(secondParticipant, "Participant");
            }

            var user7 = await _userManager.FindByEmailAsync("Governor@governor.com");
            if (user7 == null)
            {
                var governor = new ApplicationUser()
                {
                    UserName = "Governor@governor.com",
                    Name = "Governor",
                    Email = "Governor@governor.com"
                };
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


                if (user1 != null && user2 != null && coach != null)
                {
                    var uug1 = new ApplicationUserUserGroup();
                    var uug2 = new ApplicationUserUserGroup();
                    var uug3 = new ApplicationUserUserGroup();

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

        private async Task CreateTasks()
        {

            if (!_context.Tasks.Any())
            {
                var task1 = new ApplicationTask()
                {
                    Coins = 1,
                    Name = "First-Task",
                    Recurency = new DateTime(2016, 3, 4),
                    Text = "Please complete first task in order to start participation in program"
                };

                var task2 = new ApplicationTask()
                {
                    Coins = 10,
                    Name = "Second-Task",
                    Recurency = new DateTime(2016, 3, 4),
                    Text = "Please complete Second task in order to continue participation in program"
                };

                var task3 = new ApplicationTask()
                {
                    Coins = 40,
                    Name = "Third-Task",
                    Recurency = new DateTime(2016, 3, 4),
                    Text = "Please complete Third task in order to further participate in program"
                };

                var task4 = new ApplicationTask()
                {
                    Coins = 100,
                    Name = "Fourth-Task",
                    Recurency = new DateTime(2016, 3, 4),
                    Text = "Please complete Third task in order to further participate in program"
                };

                _context.Tasks.Add(task1);
                _context.Tasks.Add(task2);
                _context.Tasks.Add(task3);
                _context.Tasks.Add(task4);

                _context.SaveChanges();
            }

            if (!_context.UserTasks.Any())
            {
                var firstTask = _context.Tasks.FirstOrDefault(x => x.Name == "First-Task");
                var secondTask = _context.Tasks.FirstOrDefault(x => x.Name == "Second-Task");
                var thirdTask = _context.Tasks.FirstOrDefault(x => x.Name == "Third-Task");
                var fourthTask = _context.Tasks.FirstOrDefault(x => x.Name == "Fourth-Task");

                var user = await _dal.GetUserByEmail("FirstParticipant@participant.com");

                if (firstTask != null)
                {
                    var userTask1 = new UserTask()
                    {

                        Coins = firstTask.Coins,
                        ExpireDt = new DateTime(2016, 08, 08),
                        Status = Status.Open,
                        TaskId = firstTask.Id,
                        Text =   firstTask.Text,
                        UserId = user.Id
                    };
                    _context.Add(userTask1);
                }

                if (secondTask != null)
                {
                    var userTask2 = new UserTask()
                    {

                        Coins = secondTask.Coins,
                        ExpireDt = new DateTime(2016, 08, 08),
                        Status = Status.Open,
                        TaskId = secondTask.Id,
                        Text = secondTask.Text,
                        UserId = user.Id
                    };
                    _context.Add(userTask2);
                }

                if (thirdTask != null)
                {
                    var userTask3 = new UserTask()
                    {

                        Coins = thirdTask.Coins,
                        ExpireDt = new DateTime(2016, 08, 08),
                        Status = Status.Reopened,
                        TaskId = thirdTask.Id,
                        Text = thirdTask.Text,
                        UserId = user.Id
                    };
                    _context.Add(userTask3);
                }

                if (fourthTask != null)
                {
                    var userTask4 = new UserTask()
                    {
                        Coins = fourthTask.Coins,
                        ExpireDt = new DateTime(2015, 08, 08),
                        Status = Status.Expired,
                        TaskId = fourthTask.Id,
                        UserId = user.Id
                    };
                    _context.Add(userTask4);
                }


                _context.SaveChanges();

            }
        }
    }

}


