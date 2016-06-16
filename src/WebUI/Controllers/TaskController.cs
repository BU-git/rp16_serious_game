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
using WebUI.Services.Abstract;
using WebUI.Services.Concrete;
using Region = Domain.Entities.Region;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationEnvironment _appEnvironment;
        private readonly ICountryListProvider _countryProvider;


        public TaskController(IDAL dal, UserManager<ApplicationUser> userManager, ICountryListProvider  countryProvider, IApplicationEnvironment appEnvironment)
        {
            _dal = dal;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _countryProvider = countryProvider;
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

        public async Task<FileResult> GetImage(int id)
        {
            var imagePath = (await _dal.GetComment(id)).Image.MainPath;
            string path = Path.Combine(_appEnvironment.ApplicationBasePath, imagePath);
            byte[] mas = System.IO.File.ReadAllBytes(path);
            string file_type = "application/octet-stream";
            string file_name = Path.GetFileName(imagePath);
            return File(mas, file_type, file_name);
        }

        public string GetImagePath(int commentId)
        {
            string src;
            var imagePath = _dal.GetComment(commentId).Result.Image.MainPath;

            if (Path.GetExtension(imagePath).ToLower() == ".jpg")
            {
                string path = Path.Combine(_appEnvironment.ApplicationBasePath, imagePath);
                byte[] mas = System.IO.File.ReadAllBytes(path);

                src = "data:image/jpeg;base64," + Convert.ToBase64String(mas);
            }
            else
                src = $"/Task/GetImage/{commentId}";

            return src;
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
                    ImagePath = c.Image == null ? "" : GetImagePath(c.Id),
                    Children = CommentsToModel(c.Replies) ?? new List<CommentViewModel>()
                }).ToList();
        }

        public async Task<List<CommentViewModel>> GetTaskComments(int taskId)
        {
            var comments = (await _dal.GetTaskComments(taskId))
                .Where(c => c.ParentId == null) //get root level comments
                .ToList();

            return CommentsToModel(comments);
        }

        public async Task<IActionResult> Comments(int id)
        {
            return Json(await GetTaskComments(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel commentModel, int id)
        {
            var imageFolder = "Upload";
            var file = Request.Form.Files.GetFile("Image");
            Media commentImage = null;

            if (file != null && file.ContentDisposition != null)
            {
                //parse uploaded file
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                string Filename = parsedContentDisposition.FileName.Trim('"');
                string relativePath = Path.Combine(imageFolder, Filename);
                string uploadPath = Path.Combine(_appEnvironment.ApplicationBasePath, relativePath);

                //check for folder existence
                string dirPath = Path.Combine(_appEnvironment.ApplicationBasePath, imageFolder);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                //check for duplicated filename
                int n = 1;
                while (System.IO.File.Exists(uploadPath))
                {
                    string newFilename = Path.GetFileNameWithoutExtension(Filename) + $"({n})" + Path.GetExtension(Filename);
                    relativePath = Path.Combine(imageFolder, newFilename);
                    uploadPath = Path.Combine(_appEnvironment.ApplicationBasePath, relativePath);
                    n++;
                }

                //save the file to upload destination
                file.SaveAs(uploadPath);

                commentImage = new Media { Type = Domain.Entities.Type.Image, MainPath = relativePath };
            }

            var comment = new Comment
            {
                ParentId = commentModel.ParentId,
                Author = await _dal.GetUserById(HttpContext.User.GetUserId()),
                Text = commentModel.Text,
                EditDate = DateTime.Now,
                Image = commentImage
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
        public async Task<ActionResult> RenderTask(int id, string userId, string region, string country)
        {
            var appTask = _dal.FindTaskbyId(id);
            var user = await _dal.GetUserById(userId);
            var _region = (Region)Enum.Parse(typeof(Region), region.Replace(" ",""), false);

            TaskViewModel task = new TaskViewModel(appTask) { UserId = user.Id, UserName = user.UserName, Region = _region ,Country = country };
            return PartialView("_NewTask", task);
        }


        [HttpGet]
        public async Task<JsonResult> GetCountries(string userId, string region)
        {
            List<string> subRegions = new List<string>();
            List<string> countries = new List<string>();

            switch (region)
            {
                case "NorthAmerica":
                    subRegions.Add("Northern America");
                    subRegions.Add("Caribbean");
                    subRegions.Add("Central America");
                    break;
                case "SouthAmerica":
                    subRegions.Add("South America");
                    break;
                case "Africa":
                    subRegions.Add("Northern Africa");
                    subRegions.Add("Western Africa");
                    subRegions.Add("Middle Africa");
                    subRegions.Add("Eastern Africa");
                    subRegions.Add("Southern Africa");
                    break;
                case "Europe":
                    subRegions.Add("Northern Europe");
                    subRegions.Add("Western Europe");
                    subRegions.Add("Southern Europe");
                    
                    break;
                case "Australia":
                    subRegions.Add("Australia and New Zealand");
                    subRegions.Add("Melanesia");
                    subRegions.Add("Micronesia");
                    subRegions.Add("Polynesia");
                    break;
                case "NorthAsia":
                    subRegions.Add("Eastern Europe");
                    break;
                case "NearEast":
                    subRegions.Add("Central Asia");
                    subRegions.Add("Eastern Asia");
                    subRegions.Add("South-Eastern Asia");
                    break;
                case "SouthAsia":
                    subRegions.Add("Southern Asia");
                    subRegions.Add("Western Asia");
                    break;
                default:
                    subRegions.Add("Western Europe");
                    break;
            }

            foreach ( var subregion in subRegions)
            {
                var _countries = await _countryProvider.GetCountries(subregion);
                countries.AddRange(_countries);
            }
            var user = await _dal.GetUserById(userId);
            var usedCountries = _dal.GetUserTasksCountries(user);


            countries = countries.Except(usedCountries, StringComparer.OrdinalIgnoreCase).ToList();
        


            return Json(countries);
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
        public async Task<IActionResult> AsignTask(string name,string userId, int appTaskId, string expireDt, string text, int coins, string region,string country)
        {
            DateTime expire = DateTime.Parse(expireDt);
            var _region = (Region) Enum.Parse(typeof (Region), region, false);
            UserTask task = new UserTask()
            {
                Name =name,
                Coins = coins,
                Status = Status.Open,
                TaskId = appTaskId,
                Text = text,
                UserId = userId,
                ExpireDt = expire,
                Region = _region,
                Country = country
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
