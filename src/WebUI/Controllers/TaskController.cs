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
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Http;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(IDAL dal, UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult TaskList()
        {
            var model = new TaskListViewModel();
            return View(model);
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> PartialTaskList(string type)
        {
            var user = await GetCurrentUserAsync();
            var userTasks = new List<UserTask>();
            var role = await _dal.GetUserRoles(user);
            if (role.Contains("Participant"))
            {
                userTasks = _dal.GetUserTasks(user);
                if (userTasks.Count > 0)
                {
                    switch (type)
                    {
                        case "ResolvedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Resolved).ToList();
                            break;
                        case "AssignedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Open || x.Status == Status.Expired || x.Status == Status.Reopened).ToList();
                            break;
                        case "VerifiedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Closed || x.Status == Status.Completed).ToList();
                            break;
                        default:
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Open || x.Status == Status.Expired || x.Status == Status.Reopened).ToList();
                            break;
                    }


                }
            }
            else if (role.Contains("Coach"))
            {
                var groups = _dal.GetUsersUserGroups(user.Id);
                userTasks = new List<UserTask>();
                foreach (var group in groups)
                {
                    var t = _dal.GetUserGroupTasks(group);
                    if (t.Count > 0)
                        userTasks.AddRange(t);
                }
                if (userTasks.Count > 0)
                {
                    switch (type)
                    {
                        case "ResolvedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Resolved).ToList();
                            break;
                        case "AssignedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Open || x.Status == Status.Expired || x.Status == Status.Reopened).ToList();
                            break;
                        case "VerifiedTasks":
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Closed || x.Status == Status.Completed).ToList();
                            break;
                        default:
                            userTasks =
                                userTasks.Where(
                                     x => x.Status == Status.Open || x.Status == Status.Expired || x.Status == Status.Reopened).ToList();
                            break;
                    }

                }
            }

            var model = new TaskListViewModel(userTasks);
            return PartialView("_TaskList", model);
        }

        [ActionName("ViewAppTask")]
        public IActionResult ViewTask(int taskId)
        {
            var task = _dal.FindTaskbyId(taskId);
            var taskModel = new TaskViewModel(task);
            return View(taskModel);
        }

        public List<CommentViewModel> CommentsToModel(List<Comment> comments)
        {
            return comments?.Select(c =>
                new CommentViewModel
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Author = c.Author.Name,
                    Text = c.Text,
                    Date = c.EditDate.ToString("dd.MM.yy HH:mm"),
                    Children = CommentsToModel(c.Replies) ?? new List<CommentViewModel>()
                }).ToList();
        }

        public async Task<List<CommentViewModel>> GetTaskComments(int taskId)
        {
            var comments = (await _dal.GetTaskComments(taskId)).Where(c => c.ParentId == null).ToList();//get root level comments

            return CommentsToModel(comments);
        }

        public async Task<IActionResult> Comments(int id)
        {
            return Json(await GetTaskComments(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel commentModel, int id)
        {

            var file = Request.Form.Files.GetFile("Image");

            if (file != null)
            {
                string UploadDestination = $"upload/";
                string Filename = "";

                if (file.ContentDisposition != null)
                {
                    //parse uploaded file
                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    Filename = parsedContentDisposition.FileName.Trim('"');
                    string uploadPath = UploadDestination + Filename;

                    //save the file to upload destination
                    file.SaveAs(uploadPath);
                }

                string Photo = Filename;

                if (Photo != "")
                {
                    Photo = Url.Content($"~/upload/{Photo}");
                }
            }
            var comment = new Comment
            {
                ParentId = commentModel.ParentId,
                Author = await _dal.GetUserById(HttpContext.User.GetUserId()),
                Text = commentModel.Text,
                EditDate = DateTime.Now
            };
            await _dal.AddComment(comment, id);
            return Content("Success :)");
        }

        // GET: /Task/Region/region
        public async Task<IActionResult> ViewTasksByRegion(string region)
        {
            var user = await GetCurrentUserAsync();
            Region r;
            if (Enum.TryParse(region, out r))
            {
                var tasks = _dal.GetUserTasksForRegion(user, r);
                var taskVm = new TaskRegionViewModel(r, tasks);
                return View(taskVm);
            }
            else
            {
                TempData["warn"] = $"There is no such region: {region}";
                return RedirectToAction("TaskList");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ViewUserTask(int taskId)
        {

            UserTask task = _dal.FindUserTaskById(taskId);
            TaskViewModel taskModel = new TaskViewModel(task) {Comments = await GetTaskComments(task.Id)};
            return View("ViewTask", taskModel);
        }

        // [HttpPost]
        public async Task<IActionResult> SubmitTask(int taskId)
        {
            UserTask task = _dal.FindUserTaskById(taskId);
            task.Status = Status.Resolved;
            task.ResolutionDate = DateTime.Now;
            await _dal.UpdateUserTaskAsync(task);
            return RedirectToAction("TaskList");
        }

        public IActionResult AddTask(string coachId)
        {
            List<UserGroup> groups = _dal.GetUsersUserGroups(coachId);

            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var group in groups)
            {
                var u = _dal.GetUserGroupUsers(group);
                if (u.Count > 0)
                    users.AddRange(u);
            }


            var tasks = _dal.GetAllApplicationTasks();
            AddTaskViewModel model = new AddTaskViewModel
            {
                Tasks = tasks,
                Users = users
            };
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> RenderTask(int id, string userId)
        {
            var appTask = _dal.FindTaskbyId(id);
            var user = await _dal.GetUserById(userId);
            TaskViewModel task = new TaskViewModel(appTask) { UserId = user.Id, UserName = user.UserName };
            return PartialView("_NewTask", task);
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(string text, int coins, string command, int userTaskId)
        {
            UserTask usertask = _dal.FindUserTaskById(userTaskId);
            usertask.Text = text;
            usertask.Coins = coins;
            var user = await GetCurrentUserAsync();
            if (command.Contains("Resent"))
            {
                usertask.ResolutionDate = null;
                usertask.Status = Status.Reopened;
            }
            else if (command.Contains("Aprove"))
            {
                usertask.Status = Status.Completed;
                user.Coins += usertask.Coins;
            }

            await _dal.UpdateUserTaskAsync(usertask);
            await _userManager.UpdateAsync(user);
            return RedirectToAction("TaskList");
        }

        [HttpPost]
        public async Task<IActionResult> AsignTask(string userId, int appTaskId, string expireDt, string text, int coins)
        {
            DateTime expire = DateTime.Parse(expireDt);
            UserTask task = new UserTask()
            {
                Coins = coins,
                Status = Status.Open,
                TaskId = appTaskId,
                Text = text,
                UserId = userId,
                ExpireDt = expire
            };
            await _dal.AssignTaskAsync(task);
            return RedirectToAction("TaskList");
        }

        public async Task<IActionResult> UserStatistics()
        {
            if (!User.IsInRole("Coach"))
            {
                return TaskList();
            }

            var currentUser = await GetCurrentUserAsync();
            var userGroups = _dal.GetUsersUserGroups(currentUser.Id);
            var statistics = new StatisticsViewModel()
            {
                GroupsTasks = new List<GroupViewModel>()
            };

            foreach (var userGroup in userGroups)
            {
                var groupVm = new GroupViewModel
                {
                    UserTasks = _dal.GetUserGroupTasks(userGroup),
                    UserGroup = userGroup,
                    UserStats = new List<UserWithStatsViewModel>()
                };

                foreach (var user in _dal.GetUserGroupUsers(userGroup)
                    .Where(u => u.Id != currentUser.Id))
                {
                    var stats = new UserWithStatsViewModel(user) {Tasks = _dal.GetUserTasks(user)};

                    stats.CompletedTasks = stats.Tasks.Count(
                        t => t.Status == Status.Completed ||
                        t.Status == Status.Resolved);

                    stats.TasksLeft = stats.Tasks
                        .Count(t => t.Status == Status.Open ||
                        t.Status == Status.Reopened);

                    List<DailyStatistics> monthlyReport = new List<DailyStatistics>();

                    var dailyStats = (from task in stats.Tasks
                                      where task.ResolutionDate > DateTime.Now.AddDays(-DateTime.Now.Day) //Get statistics from the beggining of the month
                                      group task by task.ResolutionDate?.Day
                        into tasksByDays
                                      select new DailyStatistics(tasksByDays.Count(), tasksByDays.Key, DateTime.Now.Month)).ToList();

                    for (int i = 0; i < DateTime.Now.Day; i++)
                    {
                        monthlyReport.Add(dailyStats.Any(d => d.Day == i + 1)
                            ? dailyStats.FirstOrDefault(st => st.Day == i + 1)
                            : new DailyStatistics(0, i + 1, DateTime.Now.Month));
                    }

                    stats.TasksByDays = monthlyReport;

                    groupVm.UserStats.Add(stats);
                }

                groupVm.CompletedTasks = groupVm.UserTasks
                        .Count(t => t.Status == Status.Completed ||
                        t.Status == Status.Resolved);
                groupVm.TasksLeft = groupVm.UserTasks
                    .Count(t => t.Status == Status.Open ||
                    t.Status == Status.Reopened);

                statistics.GroupsTasks.Add(groupVm);
            }

            return View(statistics);
        }

        //[HttpPost]
        //public async Task<JsonResult> GetStatistics()
        //{

        //}

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
