using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Type = Domain.Entities.Type;

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
            await CreateAvatars();
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
                    var uug1 = new ApplicationUser_UserGroup();
                    var uug2 = new ApplicationUser_UserGroup();
                    var uug3 = new ApplicationUser_UserGroup();

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
                        Region = Region.Africa,
                        Country = "Algeria",
                        Coins = firstTask.Coins,
                        ExpireDt = new DateTime(2016, 08, 08),
                        Status = Status.Open,
                        TaskId = firstTask.Id,
                        Text =   firstTask.Text,
                        UserId = user.Id,
                    };
                    _context.Add(userTask1);
                }

                if (secondTask != null)
                {
                    var userTask2 = new UserTask()
                    {
                        Region = Region.Europe,
                        Country = "France",
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
                        Region = Region.NearEast,
                        Country = "Armenia",
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
                        Region = Region.NearEast,
                        Country = "Iraq",
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

        private async Task CreateAvatars()
        {
            if (!_context.Avatars.Any())
            {
                var media = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_0.png"
                };
                var avatar = new Avatar
                {
                    Price = 0,
                    Media = media,
                };
                _context.Medias.Add(media);
                _context.Avatars.Add(avatar);
                var media1 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_1.png",
                    AdditionalPath = "~/images/Level_1_blocked.png"
                };
                var avatar1 = new Avatar
                {
                    Price = 10,
                    Media = media1,
                };
                _context.Medias.Add(media1);
                _context.Avatars.Add(avatar1);
                var media2 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_2.png",
                    AdditionalPath = "~/images/Level_2_blocked.png"
                };
                var avatar2 = new Avatar
                {
                    Price = 20,
                    Media = media2,
                };
                _context.Medias.Add(media2);
                _context.Avatars.Add(avatar2);
                var media3 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_3.png",
                    AdditionalPath = "~/images/Level_3_blocked.png"
                };
                var avatar3 = new Avatar
                {
                    Price = 30,
                    Media = media3,
                };
                _context.Medias.Add(media3);
                _context.Avatars.Add(avatar3);
                var media4 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_4.png",
                    AdditionalPath = "~/images/Level_4_blocked.png"
                };
                var avatar4 = new Avatar
                {
                    Price = 40,
                    Media = media4,
                };
                _context.Medias.Add(media4);
                _context.Avatars.Add(avatar4);
                var media5 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_5.png",
                    AdditionalPath = "~/images/Level_5_blocked.png"
                };
                var avatar5 = new Avatar
                {
                    Price = 50,
                    Media = media5,
                };
                _context.Medias.Add(media5);
                _context.Avatars.Add(avatar5);
                var media6 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_6.png",
                    AdditionalPath = "~/images/Level_6_blocked.png"
                };
                var avatar6 = new Avatar
                {
                    Price = 60,
                    Media = media6,
                };
                _context.Medias.Add(media6);
                _context.Avatars.Add(avatar6);
                var media7 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_7.png",
                    AdditionalPath = "~/images/Level_7_blocked.png"
                };
                var avatar7 = new Avatar
                {
                    Price = 70,
                    Media = media7,
                };
                _context.Medias.Add(media7);
                _context.Avatars.Add(avatar7);
                var media8 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_8.png",
                    AdditionalPath = "~/images/Level_8_blocked.png"
                };
                var avatar8 = new Avatar
                {
                    Price = 80,
                    Media = media8,
                };
                _context.Medias.Add(media8);
                _context.Avatars.Add(avatar8);
                var media9 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_9.png",
                    AdditionalPath = "~/images/Level_9_blocked.png"
                };
                var avatar9 = new Avatar
                {
                    Price = 90,
                    Media = media9,
                };
                _context.Medias.Add(media9);
                _context.Avatars.Add(avatar9);
                var media10 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_10.png",
                    AdditionalPath = "~/images/Level_10_blocked.png"
                };
                var avatar10 = new Avatar
                {
                    Price = 100,
                    Media = media10,
                };
                _context.Medias.Add(media10);
                _context.Avatars.Add(avatar10);
                var media6_2 = new Media
                {
                    Type = Type.Image,
                    MainPath = "~/images/Level_6(2).png",
                    AdditionalPath = "~/images/Level_6(2)_blocked.png"
                };
                var avatar6_2 = new Avatar
                {
                    Price = 60,
                    Media = media2,
                };
                _context.Medias.Add(media6_2);
                _context.Avatars.Add(avatar6_2);
                await _context.SaveChangesAsync();
                var a = await _context.Avatars.Where(avatar11 => avatar11.Price == 0).ToListAsync();
            }
        }
    }

}


