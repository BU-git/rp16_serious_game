using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Mvc;
using Interfaces;
using WebUI.ViewModels.Task;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    public class TaskController : Controller
    {
        private readonly IDAL _dal;

        public TaskController(IDAL dal)
        {
            _dal = dal;
        }
        // GET: /<controller>/
        public async Task<IActionResult> TaskList()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            List<UserTask> userTasks = new List<UserTask>();
            var role = await _dal.GetUserRoles(user);
            if (role.Contains("Participant"))
            {
                userTasks = _dal.GetUserTasks(user);
                if (userTasks.Count > 0)
                {
                    userTasks =
                       userTasks.Where(
                           x => x.Status == Status.OPEN || x.Status == Status.EXPIRED || x.Status == Status.REOPENED).ToList();

                }
            }
            else if (role.Contains("Coach"))
            {
                List<UserGroup> groups = _dal.GetUsersUserGroups(user.Id);
                userTasks = new List<UserTask>();
                foreach (var group in groups)
                {
                    List<UserTask> t = _dal.GetUserGroupTasks(group);
                    if (t.Count > 0)
                        userTasks.AddRange(t);
                }
                if (userTasks.Count > 0)
                {
                    userTasks =  userTasks.Where(
                            x => x.Status == Status.RESOLVED ).ToList();

                }
            }

            TaskListViewModel model = new TaskListViewModel(userTasks);
            return View(model);
        }

        

        [ActionName("ViewAppTask")]
        public IActionResult ViewTask(int taskId)
        {
            ApplicationTask task = _dal.FindTaskbyId(taskId);
            TaskViewModel taskModel = new TaskViewModel(task);
            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> ViewUserTask(int taskId, string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                ApplicationUser user = await GetCurrentUserAsync();
                userId = user.Id;
            }
                
            UserTask task = _dal.FindUserTaskById(taskId, userId);
            TaskViewModel taskModel = new TaskViewModel(task);
            return View("ViewTask", taskModel);
        }

        public async Task<IActionResult> SubmitTask(int taskId, string userId)
        {
            UserTask task = _dal.FindUserTaskById(taskId, userId);
            task.Status = Status.RESOLVED;
            await _dal.UpdateUserTaskAsync(task);
            return RedirectToAction("TaskList");
        }

        [HttpPost]
        public IActionResult EditTask(string text,int coins, string command,int taskid, string userId)
        {
            UserTask usertask = _dal.FindUserTaskById(taskid, userId);
            usertask.Text = text;
            usertask.Coins = coins;
            if (command.Contains("Resent"))
            {
                usertask.Status = Status.REOPENED;
            }
            else if (command.Contains("Aprove"))
            {
                usertask.Status = Status.COMPLETED;
            }

            _dal.UpdateUserTaskAsync(usertask);
            return RedirectToAction("TaskList");
        }

        //[ActionName("ViewTask")]
        //public IActionResult ViewTask(TaskViewModel task)
        //{            
        //    return View(task);
        //}

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _dal.GetUserByEmail(HttpContext.User.Identity.Name);
        }
    }
}
