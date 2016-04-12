using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Mvc;

namespace RP16_SeriousGame.Controllers
{
    public class HomeController : Controller
    {
        private IDAL reposetory;

       public  HomeController(IDAL repo)
        {
            reposetory = repo;
        }
        public IActionResult Index()
        {
            var task = new ApplicationTask()
            {
                Id = 1,
                Coins = 1,
                Name = "First Task",
                Recurency = new DateTime(),
                Text = "This is first test task"
            };
            reposetory.AddTask(task);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
