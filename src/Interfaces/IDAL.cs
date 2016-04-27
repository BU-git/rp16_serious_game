using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Interfaces
{
    public interface IDAL
    {
        /// <summary>
        /// Create Coach user
        /// </summary>
        /// <param name="coach">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        Task CreateCoach(ApplicationUser coach, string password);
        /// <summary>
        /// Create Participant user
        /// </summary>
        /// <param name="participant">User model</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        Task CreateParticipant(ApplicationUser participant, string password);
        /// <summary>
        /// Create UserGroup
        /// </summary>
        /// <param name="userGroup">UserGroup model</param>
        /// <returns></returns>
        Task CreateUserGroup(UserGroup userGroup);
        /// <summary>
        /// Add existing user to existing group
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="group">User Group</param>
        /// <returns></returns>
        Task AddUserToGroup(ApplicationUser user, UserGroup group);

        /// <summary>
        /// Finds and returns user with specified id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserById(string id);
        /// <summary>
        /// Finds and returns user with specified email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserByEmail(string email);
        /// <summary>
        /// Finds and returns UserGroup with specified id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<UserGroup> GetUserGroupById(int id);
        /// <summary>
        /// Returns list of roles for specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        Task<IList<string>> GetUserRoles(ApplicationUser user);

        /// <summary>
        /// Applies changes made to user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        Task EditUser(ApplicationUser user);
        /// <summary>
        /// Applies changes made to User Group
        /// </summary>
        /// <param name="userGroup">User Group</param>
        /// <returns></returns>
        Task EditUserGroup(UserGroup userGroup);

        List<UserTask> GetUserGroupTasks(UserGroup group);
        List<UserGroup> GetUsersUserGroups(string userId);
        Task AddTaskAsync(ApplicationTask appTask);
        Task UpdateTaskAsync(ApplicationTask appTask);
        Task AssignTaskAsync(UserTask userTask);
        Task UpdateUserTaskAsync(UserTask userTask);
        List<UserTask> GetUserTasks(ApplicationUser user);
        ApplicationTask FindTaskbyName(string name);
        ApplicationTask FindTaskbyId(int taskId);
        UserTask FindUserTaskById(int taskId, string userId);
    }
}
