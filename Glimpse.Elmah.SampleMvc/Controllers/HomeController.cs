using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Glimpse.Elmah.SampleMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var exceptionType = (typeof(Exception));
            var exceptionTypes = Assembly
                .GetAssembly(exceptionType)
                .GetTypes()
                .Where(t => t.IsSubclassOf(exceptionType))
                .ToList();
            
            return View(exceptionTypes);
        }

        [HttpPost]
        public ActionResult Index(string exceptionTypeToThrow)
        {
            var exceptionType = Type.GetType(exceptionTypeToThrow);
            if (exceptionType == null)
                return View();

            var exception = (Exception)Activator.CreateInstance(exceptionType);
            throw exception;
        }
    }
}