using System.Linq;
using Interfaces;
using Microsoft.AspNet.Mvc;
using WebUI.Services.Abstract;
using WebUI.Services.Concrete;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITranslationProvider _translationProvider;

        public HomeController(IDAL repo)
        {
            _translationProvider = new JsonTranslationProvider(@".\Assets\json\translation.json");
        }

        public IActionResult Index()
        {
            return RedirectToAction("TaskList", "Task");
            //return View();
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

        [HttpPost]
        public JsonResult Translate(string[] keys)
        {
            //var language = Request.Cookies.ContainsKey("language") ? Request.Cookies["language"][0] : "en";

            var dictionary = keys.ToDictionary(key => key, key => _translationProvider.Translate(key));
            return Json(dictionary);
        }
    }
}
