using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeoServer.Controllers
{
    public class OLController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OLController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ViewModis()
        {
            ViewBag.KATOType = new List<SelectListItem>()
            {
                new SelectListItem() { Text="Области", Value="adm1pol"},
                new SelectListItem() { Text="Районы", Value="adm2pol"},
                new SelectListItem() { Text="Сельские округи", Value="adm3pol"}
            };

            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name");
            var modisProducts = _context.ModisProduct.Where(m => m.ModisSourceId == _context.ModisSource.OrderBy(ms => ms.Name).FirstOrDefault().Id).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name");
            ViewBag.ModisDataSet = new SelectList(_context.ModisDataSet.Where(m => m.ModisProductId == modisProducts.FirstOrDefault().Id).OrderBy(m => m.Index), "Name", "IndexName");
            return View();
        }
    }
}