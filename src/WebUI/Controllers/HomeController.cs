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
        private IZipWorker _zipWorker;

        public HomeController(IDAL repo, IZipWorker zipWorker)
        {
            _repository = repo;
            _zipWorker = zipWorker;
        }
        public async Task<IActionResult> Index()
        {
            var ressult = await _zipWorker.GetAddressAsync("2012ES", 30);
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
