﻿using System;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Entity;
using System.Linq;

namespace DAL
{
    public class DAL : IDAL
    {
        private const string CoachRole = "Coach";
        private const string ParticipantRole = "Participant";
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DAL(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        /// <summary>
        /// Create Coach user
        /// </summary>
        /// <param name="coach">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task CreateCoach(ApplicationUser coach, string password)
        {
            IdentityRole coachRole = await _roleManager.FindByNameAsync(CoachRole);
            if (coachRole == null)
                throw new Exception("Coach Role is missing.");

            ApplicationUser user = await _userManager.FindByEmailAsync(coach.Email);
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
            IdentityRole participantRole = await _roleManager.FindByNameAsync(ParticipantRole);
            if (participantRole == null)
                throw new Exception("Participant Role is missing.");

            ApplicationUser user = await _userManager.FindByEmailAsync(participant.Email);
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
                var appointment_User = new Appointment_User()
                {
                    AppointmentId = appointment.Id,
                    UserId = userId,
                    IsOwner = (creatorId == userId)
                };
                _context.Add(appointment_User);
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
            foreach(var appointment_User in appointment.Appointment_Users)
            {
                if (!attendeesIds.Contains(appointment_User.UserId))
                    _context.Remove(appointment_User);
            }
            
            //Add users
            foreach (var userId in attendeesIds)
            {
                if (!appointment.Appointment_Users.Select(a => a.UserId).Contains(userId))
                {
                    var appointment_User = new Appointment_User()
                    {
                        AppointmentId = appointment.Id,
                        UserId = userId
                    };
                    _context.Add(appointment_User);
                }
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
            Appointment appointment = await _context.Appointments.SingleAsync(m => m.Id == id);
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
            return await _context.Appointments.Where(a => a.Appointment_Users.Any(au => au.UserId == userId)).ToListAsync();
        }

        /// <summary>
        /// Get appointment by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _context.Appointments.Include(a => a.Appointment_Users).ThenInclude(au => au.User).SingleAsync(a => a.Id == id);
        }

        public async Task<Appointment_User> ValidateAppointment(DateTime start, DateTime end, IEnumerable<string> users)
        {
            return await _context.Appointment_Users.Include(a => a.Appointment).Include(a => a.User).FirstOrDefaultAsync(a => (a.Appointment.Start < end && a.Appointment.End > start) && users.Contains(a.UserId));
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
        
        public async Task UpdateTaskAsync(ApplicationTask appTask)
        {
            var taskWithSameName = _context.Tasks.FirstOrDefault(x => x.Name == appTask.Name && x.Id != appTask.Id);
            if (taskWithSameName!=null)
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
            if (user == null || task == null)
                throw new Exception("There is no such User or Task");

            var usertask =
                _context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask != null)
                throw new Exception($"User {usertask.UserId} already has task {usertask.TaskId}");
            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateUserTaskAsync(UserTask userTask)
        {
            var usertask =
                _context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask == null)
                throw new Exception($"User {userTask.UserId} doesn't have task {userTask.TaskId}");
            
            _context.Entry(usertask).State = EntityState.Detached;
            _context.Entry(userTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public List<UserTask> GetUserTasks(ApplicationUser user)
        {

            var userTasks = _context.UserTasks.Where(x => x.UserId == user.Id).Include(x=>x.ApplicationTask).ToList();
            return userTasks;

        }

        public List<UserTask> GetUserGroupTasks(UserGroup group)
        {
            var users = _context.Users.SelectMany(x =>x.ApplicationUser_UserGroups.Where(e=>e.UserGroupId == group.UserGroupId).Select(s=>s.ApplicationUser)).Distinct();
            
            List<UserTask>tasks = new List<UserTask>();
            foreach (ApplicationUser user in users)
            {
                var t = GetUserTasks(user);
                if(t.Count>0)
                    tasks.AddRange(t);
            }
            return tasks;
        }

        public List<UserGroup> GetUsersUserGroups(string userId)
        {
            var userGroups =
                _context.UserGroups.SelectMany(
                    x => x.ApplicationUser_UserGroups.Where(e => e.ApplicationUser.Id == userId)
                            .Select(e => e.UserGroup)).Distinct().ToList();
            return userGroups;
        } 

        public UserTask FindUserTaskById(int taskId, string userId)
        {
            var userTask =
                _context.UserTasks.Where(x => x.UserId == userId && x.TaskId == taskId)
                    .Include(x => x.ApplicationTask).Include(x=>x.User)
                    .FirstOrDefault();
            return userTask;
        }

        public async Task<List<UserTask>> GetUserTasksAsync(ApplicationUser user)
        {
            List<UserTask> userTasks = await _context.UserTasks.Where(x => x.UserId == user.Id).ToListAsync();
            return userTasks;
        }

        public Avatar GetUserAvatarFromContext(ApplicationUser user)
        {
            try
            {
                Avatar avatar = _userManager.Users.First(applicationUser => applicationUser == user).Avatar;
                return avatar;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User {user.Name} doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Avatar GetUserAvatarByUserId(string userId)
        {
            try
            {
                Avatar avatar = _context.Users.First(user => user.Id == userId).Avatar;
                return avatar;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User with {userId} Id doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetAvatarPathByUserId(string userId)
        {
            try
            {
                string path = _context.Users.First(user => user.Id == userId).Avatar.Media.Path;
                return path;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"User with {userId} Id doesn't have an avatar or such user doesn't exist!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> UpdateUserAvatar(Avatar avatar, ApplicationUser appUser)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(appUser.Id);
                user.Avatar = avatar;
                IdentityResult result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception)
            {
                throw new Exception($"There is no such User in the system");
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
                string path = _context.Avatars.First(av => av == avatar).Media.Path;
                return path;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"There is no such Avatar in the system");
            }
        }

        public async Task<int> UpdateAvatarPath(Avatar avatar, string path)
        {
            try
            {
                Avatar userAvatar = _context.Avatars.First(av => av == avatar);
                userAvatar.Media.Path = path;
                int result = await _context.SaveChangesAsync();
                return result;
            }
            catch (NullReferenceException)
            {
                throw new Exception($"There is no such Avatar in the system");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}