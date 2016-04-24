using System;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Data.Entity;

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
        public async Task<IdentityResult> CreateParticipant(ApplicationUser participant, string password)
        {
            IdentityRole participantRole = await _roleManager.FindByNameAsync(ParticipantRole);
            if (participantRole == null)
                throw new Exception("Participant Role is missing.");

            ApplicationUser user = await _userManager.FindByEmailAsync(participant.Email);
            if (user != null)
                throw new Exception("User already exists.");

            var result = await _userManager.CreateAsync(participant, password);
            await _userManager.AddToRoleAsync(participant, ParticipantRole);
            return result;
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
            var userToGroup = new ApplicationUserUserGroup()
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

        public async Task AddTaskAsync(ApplicationTask appTask)
        {
            ApplicationTask task = _context.Tasks.FirstOrDefault(x => x.Name == appTask.Name);
            if (task != null)
                throw new Exception($"Task {appTask.Name} already exists in database.Task Id: {task.Id}");

            _context.Tasks.Add(appTask);
            await _context.SaveChangesAsync();


        }

        public ApplicationTask FindTaskbyName(string name)
        {
            ApplicationTask task = _context.Tasks.FirstOrDefault(x => x.Name == name);
            return task;

        }

        public ApplicationTask FindTaskbyId(int taskId)
        {

            ApplicationTask task = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            return task;

        }

        public async Task UpdateTaskAsync(ApplicationTask appTask)
        {
            ApplicationTask taskWithSameName = _context.Tasks.FirstOrDefault(x => x.Name == appTask.Name && x.Id != appTask.Id);
            if (taskWithSameName != null)
                throw new Exception($"Task {taskWithSameName.Id} already has this name.");
            ApplicationTask task = _context.Tasks.FirstOrDefault(x => x.Id == appTask.Id);
            if (task == null)
                throw new Exception($"There is no task: {appTask.Name} in database.");

            _context.Entry(task).State = EntityState.Detached;
            _context.Entry(appTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();


        }

        public async Task AssignTaskAsync(UserTask userTask)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userTask.UserId);
            ApplicationTask task = _context.Tasks.FirstOrDefault(x => x.Id == userTask.TaskId);
            if (user == null || task == null)
                throw new Exception("There is no such User or Task");

            UserTask usertask =
                _context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask != null)
                throw new Exception($"User {usertask.UserId} already has task {usertask.TaskId}");
            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateUserTaskAsync(UserTask userTask)
        {
            UserTask usertask =
                _context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask == null)
                throw new Exception($"User {userTask.UserId} doesn't have task {userTask.TaskId}");

            _context.Entry(usertask).State = EntityState.Detached;
            _context.Entry(userTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public List<UserTask> GetUserTasks(ApplicationUser user)
        {
            List<UserTask> userTasks = _context.UserTasks.Where(x => x.UserId == user.Id).ToList();
            return userTasks;
        }

        public Avatar GetUserAvatarFromContext(ApplicationUser user)
        {
            try
            {
                Avatar avatar = _context.Avatars.FirstOrDefault(av => av.User == user);
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
    }
}
