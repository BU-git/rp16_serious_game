using System;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;

namespace DAL
{
    // ReSharper disable once InconsistentNaming
    public class DAL : IDAL
    {
        private const string CoachRole = "Coach";
        private const string ParticipantRole = "Participant";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DAL(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Create Coach user
        /// </summary>
        /// <param name="coach">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task CreateCoach(ApplicationUser coach, string password)
        {
            var coachRole = await _roleManager.FindByNameAsync(CoachRole);
            if (coachRole == null)
                throw new Exception("Coach Role is missing.");

            var user = await _userManager.FindByEmailAsync(coach.Email);
            if (user != null)
                throw new Exception("User already exists.");

            await _userManager.CreateAsync(coach, password);
            await _userManager.AddToRoleAsync(coach, CoachRole);
        }

        /// <summary>
        /// Create Participant user
        /// </summary>
        /// <param name="participant">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task CreateParticipant(ApplicationUser participant, string password)
        {
            var participantRole = await _roleManager.FindByNameAsync(ParticipantRole);
            if (participantRole == null)
                throw new Exception("Participant Role is missing.");

            var user = await _userManager.FindByEmailAsync(participant.Email);
            if (user != null)
                throw new Exception("User already exists.");

            await _userManager.CreateAsync(participant, password);
            await _userManager.AddToRoleAsync(participant, ParticipantRole);
        }

        /// <summary>
        /// Create UserGroup
        /// </summary>
        /// <param name="userGroup">UserGroup model</param>
        /// <returns></returns>
        public async Task CreateUserGroup(UserGroup userGroup)
        {
            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add existing user to existing group
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="group">User Group</param>
        /// <returns></returns>
        public async Task AddUserToGroup(ApplicationUser user, UserGroup group)
        {
            var userToGroup = new ApplicationUser_UserGroup()
            {
                ApplicationUser = user,
                UserGroup = group
            };

            _context.Add(userToGroup);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds and returns user with specified id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Finds and returns user with specified email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <summary>
        /// Finds and returns UserGroup with specified id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public async Task<UserGroup> GetUserGroupById(int id)
        {
            return await _context.UserGroups.FirstOrDefaultAsync(g => g.UserGroupId == id);
        }

        /// <summary>
        /// Returns list of roles for specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Applies changes made to user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public async Task EditUser(ApplicationUser user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Applies changes made to User Group
        /// </summary>
        /// <param name="userGroup">User Group</param>
        /// <returns></returns>
        public async Task EditUserGroup(UserGroup userGroup)
        {
            _context.Update(userGroup);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Create Appointment and assign it's owner
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="creatorId">Creator which automatically assigned as owner</param>
        /// <param name="attendeesIds">List of participants UserIds</param>
        /// <returns></returns>
        public async Task CreateAppointment(Appointment appointment, string creatorId, IEnumerable<string> attendeesIds)
        {
            _context.Appointments.Add(appointment);

            foreach (var userId in attendeesIds)
            {
                var appointmentUser = new AppointmentUser()
                {
                    AppointmentId = appointment.Id,
                    UserId = userId,
                    IsOwner = creatorId == userId
                };
                _context.Add(appointmentUser);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edit appointment
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="attendeesIds">List of participants UserIds</param>
        /// <returns></returns>
        public async Task EditAppointment(Appointment appointment, IEnumerable<string> attendeesIds)
        {
            //Remove users
            var userIds = attendeesIds as IList<string> ?? attendeesIds.ToList();
            foreach (var appointmentUser in appointment.AppointmentUsers.Where(appointmentUser => !userIds.Contains(appointmentUser.UserId)))
            {
                _context.Remove(appointmentUser);
            }

            //Add users
            foreach (var appointmentUser in from userId in userIds
                                            where !appointment.AppointmentUsers.Select(a => a.UserId).Contains(userId)
                                            select new AppointmentUser()
                                            {
                                                AppointmentId = appointment.Id,
                                                UserId = userId
                                            })
            {
                _context.Add(appointmentUser);
            }

            _context.Update(appointment);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.SingleAsync(m => m.Id == id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get all future appointments where specified user participates
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public async Task<List<Appointment>> GetUserAppointments(string userId)
        {
            return await _context.Appointments.Where(a => a.AppointmentUsers.Any(au => au.UserId == userId)).ToListAsync();
        }

        /// <summary>
        /// Get appointment by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _context.Appointments.Include(a => a.AppointmentUsers).ThenInclude(au => au.User).SingleAsync(a => a.Id == id);
        }

        public async Task<AppointmentUser> ValidateAppointment(DateTime start, DateTime end, IEnumerable<string> users)
        {
            return await _context.AppointmentUsers.Include(a => a.Appointment).Include(a => a.User).FirstOrDefaultAsync(a => a.Appointment.Start < end && a.Appointment.End > start && users.Contains(a.UserId));
        }

        public async Task AddTaskAsync(ApplicationTask appTask)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Name == appTask.Name);
            if (task != null)
                throw new Exception($"Task {appTask.Name} already exists in database.Task Id: {task.Id}");

            _context.Tasks.Add(appTask);
            await _context.SaveChangesAsync();


        }

        public ApplicationTask FindTaskbyName(string name)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Name == name);
            return task;

        }

        public ApplicationTask FindTaskbyId(int taskId)
        {

            var task = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            return task;

        }
        
        public List<ApplicationTask> GetAllApplicationTasks()
        {
            var tasks = _context.Tasks.ToList();
            return tasks;
        }
        
        public async Task UpdateTaskAsync(ApplicationTask appTask)
        {
            var taskWithSameName = _context.Tasks.FirstOrDefault(x => x.Name == appTask.Name && x.Id != appTask.Id);
            if (taskWithSameName != null)
                throw new Exception($"Task {taskWithSameName.Id} already has this name.");
            var task = _context.Tasks.FirstOrDefault(x => x.Id == appTask.Id);
            if (task == null)
                throw new Exception($"There is no task: {appTask.Name} in database.");

            _context.Entry(task).State = EntityState.Detached;
            _context.Entry(appTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();


        }

        public async Task AssignTaskAsync(UserTask userTask)
        {
            var user = await _userManager.FindByIdAsync(userTask.UserId);
            var task = _context.Tasks.FirstOrDefault(x => x.Id == userTask.TaskId);
            if (user != null && task != null)
            {
            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();
            }
            else 
                throw  new Exception($"No such User {userTask.UserId} or Task {userTask.TaskId} ");

        }

        public async Task UpdateUserTaskAsync(UserTask userTask)
        {
            var usertask =
                _context.UserTasks.FirstOrDefault(x =>x.Id == userTask.Id);

            if (usertask != null)
            {
            _context.Entry(usertask).State = EntityState.Detached;
            _context.Entry(userTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            }
            else 
                throw new Exception($"No task {userTask.Id}");

        }

        public List<UserTask> GetUserTasks(ApplicationUser user)
        {

            var userTasks = _context.UserTasks.Where(x => x.UserId == user.Id).Include(x => x.ApplicationTask).ToList();
            return userTasks;

        }

        public List<UserTask> GetUserTasksForRegion(ApplicationUser user, Region region)
        {
            var userTasks = GetUserTasks(user)
                    .Where(x => x.Region == region)
                    .ToList();
            return userTasks;
        }

        public List<UserTask> GetUserGroupTasks(UserGroup group)
        {
            var users = _context.Users.SelectMany(x => x.ApplicationUserUserGroups.Where(e => e.UserGroupId == group.UserGroupId).Select(s => s.ApplicationUser)).Distinct();
            
            var tasks = new List<UserTask>();
            foreach (var user in users)
            {
                var t = GetUserTasks(user);
                if (t.Count > 0)
                    tasks.AddRange(t);
            }
            return tasks;
        }

        public List<UserGroup> GetUsersUserGroups(string userId)
        {
            var userGroups =
                _context.UserGroups.SelectMany(
                    x => x.ApplicationUserUserGroups.Where(e => e.ApplicationUser.Id == userId)
                            .Select(e => e.UserGroup)).Distinct().ToList();
            return userGroups;
        } 

        public List<ApplicationUser> GetUserGroupUsers(UserGroup group)
        {
            var users =
                _context.Users.SelectMany(
                    x =>
                        x.ApplicationUserUserGroups.Where(e => e.UserGroupId == group.UserGroupId)
                            .Select(u => u.ApplicationUser)).Distinct().ToList();
            return users;
        }

        public UserTask FindUserTaskById(int id)
        {
            var userTask =
                _context.UserTasks.Where(x => x.Id == id )
                    .Include(x => x.ApplicationTask).Include(x=>x.User)
                    .FirstOrDefault();
            return userTask;
        }

        public async Task<List<UserTask>> GetUserTasksAsync(ApplicationUser user)
        {
            var userTasks = await _context.UserTasks.Where(x => x.UserId == user.Id).ToListAsync();
            return userTasks;
        }

        public Avatar GetUserAvatarFromContext(ApplicationUser user)
        {
            try
            {
                var avatar = _userManager.Users.First(applicationUser => applicationUser == user).Avatar;
                return avatar;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User {user.Name} doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong:{ex.Message} ");
            }
        }

        public Avatar GetUserAvatarByUserId(string userId)
        {
            try
            {
                var avatar = _context.Users.First(user => user.Id == userId).Avatar;
                return avatar;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User with {userId} Id doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong:{ex.Message} ");
            }
        }

        public string GetAvatarPathByUserId(string userId)
        {
            try
            {
                var path = _context.Users.First(user => user.Id == userId).Avatar.Media.Path; //TODO: add path to default avatar if not found
                return path;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User with {userId} Id doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong:{ex.Message} "); 
            }
        }

        public async Task<IdentityResult> UpdateUserAvatar(Avatar avatar, ApplicationUser appUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(appUser.Id);
                user.Avatar = avatar;
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception)
            {
                throw new Exception("There is no such User in the system");
            }
        }

        public async Task<List<Avatar>> GetAllAvatarsAsync()
        {
            return await _context.Avatars.ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersWithAvatarsAsync()
        {
            return await _userManager.Users.Where(user => user.Avatar != null).ToListAsync();
        }

        public string GetAvatarPath(Avatar avatar)
        {
            try
            {
                var path = _context.Avatars.First(av => av == avatar).Media.Path;
                return path;
            }
            catch (NullReferenceException)
            {
                throw new Exception("There is no such Avatar in the system");
            }
        }

        public async Task<int> UpdateAvatarPath(Avatar avatar, string path)
        {
            try
            {
                var userAvatar = _context.Avatars.First(av => av == avatar);
                userAvatar.Media.Path = path;
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (NullReferenceException)
            {
                throw new Exception("There is no such Avatar in the system");
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong:{ex.Message} ");
            }
        }
    }
}
