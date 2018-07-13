using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoServer.Data;
using GeoServer.Models;
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

        //[HttpPost]
        //public JsonResult GetKATOZonalStat(string KATO, int Year)
        //{
        //    JsonResult result = new JsonResult(_context.ZonalStatKATO.Where(z => z.KATO == KATO && z.Year == Year).OrderBy(z => z.DayOfYear));
        //    return result;
        //}

        [HttpPost]
        public ActionResult GetKATOZonalStat(string KATO, int Year, string ModisSource, string ModisProduct, string ModisDataSet)
        {
            List<int> days = _context.ZonalStatKATO.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<ZonalStatKATO> current = new List<ZonalStatKATO>();
            List<decimal> min = new List<decimal>(),
                max = new List<decimal>(),
                average = new List<decimal>();
            var all = _context.ZonalStatKATO.Where(z => z.KATO == KATO && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            foreach (int day in days)
            {
                List<ZonalStatKATO> today = all.Where(z => z.DayOfYear == day).ToList();
                if(today.Count > 0)
                {
                    current.Add(today.FirstOrDefault());
                    min.Add(today.Min(z => z.Value));
                    max.Add(today.Max(z => z.Value));
                    average.Add(today.Average(z => z.Value));
                }
            }
            return Json(new
            {
                current,
                min,
                max,
                average
            });
        }
    }
}