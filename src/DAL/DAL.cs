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
    public class DAL : IDAL
    {
        private const string COACH_ROLE = "Coach";
        private const string PARTICIPANT_ROLE = "Participant";

        private ApplicationDbContext context;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public DAL(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Create Coach user
        /// </summary>
        /// <param name="coach">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task CreateCoach(ApplicationUser coach, string password)
        {
            var coachRole = await roleManager.FindByNameAsync(COACH_ROLE);
            if (coachRole == null)
                throw new Exception("Coach Role is missing.");

            var user = await userManager.FindByEmailAsync(coach.Email);
            if (user != null)
                throw new Exception("User already exists.");

            await userManager.CreateAsync(coach, password);
            await userManager.AddToRoleAsync(coach, COACH_ROLE);
        }

        /// <summary>
        /// Create Participant user
        /// </summary>
        /// <param name="participant">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task CreateParticipant(ApplicationUser participant, string password)
        {
            var participantRole = await roleManager.FindByNameAsync(PARTICIPANT_ROLE);
            if (participantRole == null)
                throw new Exception("Participant Role is missing.");

            var user = await userManager.FindByEmailAsync(participant.Email);
            if (user != null)
                throw new Exception("User already exists.");

            await userManager.CreateAsync(participant, password);
            await userManager.AddToRoleAsync(participant, PARTICIPANT_ROLE);
        }

        /// <summary>
        /// Create UserGroup
        /// </summary>
        /// <param name="userGroup">UserGroup model</param>
        /// <returns></returns>
        public async Task CreateUserGroup(UserGroup userGroup)
        {
            context.UserGroups.Add(userGroup);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Add existing user to existing group
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="group">User Group</param>
        /// <returns></returns>
        public async Task AddUserToGroup(ApplicationUser user, UserGroup group)
        {
            var userToGroup = new ApplicationUser_UserGourp()
            {
                ApplicationUser = user,
                UserGroup = group
            };

            context.Add(userToGroup);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds and returns user with specified id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Finds and returns user with specified email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Finds and returns UserGroup with specified id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public async Task<UserGroup> GetUserGroupById(int id)
        {
            return await context.UserGroups.FirstOrDefaultAsync(g => g.UserGroupId == id);
        }

        /// <summary>
        /// Returns list of roles for specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Applies changes made to user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        public async Task EditUser(ApplicationUser user)
        {
            context.Update(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Applies changes made to User Group
        /// </summary>
        /// <param name="userGroup">User Group</param>
        /// <returns></returns>
        public async Task EditUserGroup(UserGroup userGroup)
        {
            context.Update(userGroup);
            await context.SaveChangesAsync();
        }

        public async Task AddTaskAsync(ApplicationTask appTask)
        {
            var task = context.Tasks.FirstOrDefault(x => x.Name == appTask.Name);
            if (task != null)
                throw new Exception($"Task {appTask.Name} already exists in database.Task Id: {task.Id}");

            context.Tasks.Add(appTask);
            await context.SaveChangesAsync();


        }

        public ApplicationTask FindTaskbyName(string name)
        {
            var task = context.Tasks.FirstOrDefault(x => x.Name == name);
            return task;

        }

        public ApplicationTask FindTaskbyId(int taskId)
        {

            var task = context.Tasks.FirstOrDefault(x => x.Id == taskId);
            return task;

        }
        
        public async Task UpdateTaskAsync(ApplicationTask appTask)
        {
            var taskWithSameName = context.Tasks.FirstOrDefault(x => x.Name == appTask.Name && x.Id != appTask.Id);
            if (taskWithSameName!=null)
                throw new Exception($"Task {taskWithSameName.Id} already has this name.");
            var task = context.Tasks.FirstOrDefault(x => x.Id == appTask.Id);
            if (task == null)
                throw new Exception($"There is no task: {appTask.Name} in database.");

            context.Entry(task).State = EntityState.Detached;
            context.Entry(appTask).State = EntityState.Modified;
            await context.SaveChangesAsync();


        }

        public async Task AssignTaskAsync(UserTask userTask)
        {
            var user = await userManager.FindByIdAsync(userTask.UserId);
            var task = context.Tasks.FirstOrDefault(x => x.Id == userTask.TaskId);
            if (user == null || task == null)
                throw new Exception("There is no such User or Task");

            var usertask =
                context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask != null)
                throw new Exception($"User {usertask.UserId} already has task {usertask.TaskId}");
            context.UserTasks.Add(userTask);
            await context.SaveChangesAsync();

        }

        public async Task UpdateUserTaskAsync(UserTask userTask)
        {
            var usertask =
                context.UserTasks.FirstOrDefault(x => x.UserId == userTask.UserId && x.TaskId == userTask.TaskId);

            if (usertask == null)
                throw new Exception($"User {userTask.UserId} doesn't have task {userTask.TaskId}");
            
            context.Entry(usertask).State = EntityState.Detached;
            context.Entry(userTask).State = EntityState.Modified;
            await context.SaveChangesAsync();

        }

        public List<UserTask> GetUserTasks(ApplicationUser user)
        {

            var userTasks = context.UserTasks.Where(x => x.UserId == user.Id).ToList();
            return userTasks;

        }

    }
}
