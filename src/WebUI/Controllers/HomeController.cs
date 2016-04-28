using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNet.Mvc;
using WebUI.Services.Abstract;
using WebUI.Services.Concrete;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IDAL _repository;

        public HomeController(IDAL repo)
        {
            _repository = repo;
        }
        public IActionResult Index()
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
