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
        public async Task<IActionResult> Index()
        {
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
