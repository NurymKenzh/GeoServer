using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoServer.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace GeoServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoServerController _GeoServer;
        private readonly GDALController _GDAL;

        public HomeController(GeoServerController GeoServer,
            GDALController GDAL)
        {
            _GeoServer = GeoServer;
            _GDAL = GDAL;
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

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Test()
        {
            //try
            //{
            //    ViewData["Message"] = "GeoServer workspaces: " + string.Join(", ", _GeoServer.GetWorkspaces());
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "Raster bands count: " + _GDAL.GetRasterBandsCount(@"D:\Documents\MailCloud\Work\AtlasSolar\Maps\LandCover\glckz.tif").ToString();
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    string workspaceName = "Test";
            //    _GeoServer.CreateWorkspace(workspaceName);
            //    ViewData["Message"] = "Create GeoServer workspace: " + workspaceName;
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    string workspaceName = "Test";
            //    _GeoServer.DeleteWorkspace(workspaceName);
            //    ViewData["Message"] = "Delete GeoServer workspace: " + workspaceName;
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            try
            {
                ViewData["Message"] = "Test workspace files: " + string.Join(", ", _GeoServer.GetWorkspaceLayerFiles("Test"));
            }
            catch (Exception exception)
            {
                ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
