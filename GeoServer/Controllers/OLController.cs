using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeoServer.Controllers
{
    public class OLController : Controller
    {
        public IActionResult ViewModis()
        {
            return View();
        }
    }
}