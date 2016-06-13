using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using System;

namespace Interfaces
{
    // ReSharper disable once InconsistentNaming
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
        /// Get all users
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationUser>> GetUsers();
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
        List<UserTask> GetUserTasks(ApplicationUser user);
        Task AddTaskAsync(ApplicationTask appTask);
        Task UpdateTaskAsync(ApplicationTask appTask);
        List<ApplicationTask> GetAllApplicationTasks();
        Task AssignTaskAsync(UserTask userTask);
        Task UpdateUserTaskAsync(UserTask userTask);
        Task<List<UserTask>> GetUserTasksAsync(ApplicationUser user);
        List<UserTask> GetUserTasksForRegion(ApplicationUser user, Region region);
        ApplicationTask FindTaskbyName(string name);
        ApplicationTask FindTaskbyId(int taskId);
        UserTask FindUserTaskById(int id);
        Avatar GetUserAvatarFromContext(ApplicationUser user);
        Avatar GetUserAvatarByUserId(string userId);
        string GetAvatarPathByUserId(string userId);
        Task<Avatar> GetAvatarById(int id);
        Task<IdentityResult> UpdateUserAvatar(Avatar avatar, ApplicationUser appUser);
        Task<List<Avatar>> GetAllAvatarsAsync();
        Task<List<ApplicationUser>> GetAllUsersWithAvatarsAsync();
        Task<List<Avatar>> GetAllAvatarsWithPrice(int price);
        string GetAvatarPath(Avatar avatar);
        Task<int> UpdateAvatarPath(Avatar avatar, string path);
        List<UserTask> GetUserGroupTasks(UserGroup group);
        List<UserGroup> GetUsersUserGroups(string userId);
        List<ApplicationUser> GetUserGroupUsers(UserGroup group);
        Task<IdentityResult> UpdateUserAvailableAvatars(Avatar avatar, ApplicationUser appUser);
        Task<List<Avatar>> FindNotAvailableAvatars(ApplicationUser appUser);
        Task<List<Avatar>> FindAvailableAvatars(ApplicationUser appUser);
        /// <summary>
        /// Create Appointment and assign it's owner
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="creatorId">Creator which automatically assigned as owner</param>
        /// <param name="attendeesIds">List of participants UserIds</param>
        /// <returns></returns>
        Task CreateAppointment(Appointment appointment, string creatorId, IEnumerable<string> attendeesIds);
        /// <summary>
        /// Edit appointment
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="attendeesIds">List of participants UserIds</param>
        /// <returns></returns>
        Task EditAppointment(Appointment appointment, IEnumerable<string> attendeesIds);
        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task DeleteAppointment(int id);
        /// <summary>
        /// Get all future appointments where specified user participates
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        Task<List<Appointment>> GetUserAppointments(string userId);
        /// <summary>
        /// Get appointment by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<Appointment> GetAppointmentById(int id);
        Task<Appointment_User> ValidateAppointment(DateTime start, DateTime end, IEnumerable<string> users);

        Task<List<Comment>> GetTaskComments(int taskId);
        Task AddComment(Comment comment, int taskId);
    }
}
