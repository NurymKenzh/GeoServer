using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoServer.Models;

namespace GeoServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoServerController _GeoServer;

        public HomeController(GeoServerController GeoServer)
        {
            _GeoServer = GeoServer;
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

        public IActionResult Test()
        {
            try
            {
                ViewData["Message"] = "GeoServer Workspaces: " + string.Join(", ", _GeoServer.GetWorkspaces());
            }
            catch(Exception exception)
            {
                ViewData["Message"] = $"{exception.ToString()}. {(exception.InnerException != null ? exception.InnerException.Message : string.Empty)}";
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
