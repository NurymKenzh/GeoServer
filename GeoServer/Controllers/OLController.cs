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
            int minYear = _context.ZonalStatKATO.Min(z => z.Year),
                maxYear = _context.ZonalStatKATO.Max(z => z.Year);
            ViewBag.Year = new SelectList(Enumerable.Range(minYear, (maxYear - minYear) + 1));
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

        [HttpPost]
        public ActionResult GetYearDates(int Year)
        {
            List<SelectListItem> dates = new List<SelectListItem>();
            if(DateTime.IsLeapYear(Year))
            {
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.01 - 1", Value = "1" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.17 - 17", Value = "17" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.02 - 33", Value = "33" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.18 - 49", Value = "49" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.05 - 65", Value = "65" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.21 - 81", Value = "81" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.06 - 97", Value = "97" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.22 - 113", Value = "113" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.08 - 129", Value = "129" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.24 - 145", Value = "145" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.09 - 161", Value = "161" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.25 - 177", Value = "177" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.11 - 193", Value = "193" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.27 - 209", Value = "209" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.12 - 225", Value = "225" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.28 - 241", Value = "241" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.13 - 257", Value = "257" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.29 - 273", Value = "273" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.15 - 289", Value = "289" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.31 - 305", Value = "305" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.16 - 321", Value = "321" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.02 - 337", Value = "337" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.18 - 353", Value = "353" });
            }
            else
            {
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.01 - 1", Value = "1" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.17 - 17", Value = "17" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.02 - 33", Value = "33" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.18 - 49", Value = "49" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.06 - 65", Value = "65" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.22 - 81", Value = "81" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.07 - 97", Value = "97" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.23 - 113", Value = "113" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.09 - 129", Value = "129" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.25 - 145", Value = "145" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.10 - 161", Value = "161" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.26 - 177", Value = "177" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.12 - 193", Value = "193" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.28 - 209", Value = "209" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.13 - 225", Value = "225" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.29 - 241", Value = "241" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.14 - 257", Value = "257" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.30 - 273", Value = "273" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.16 - 289", Value = "289" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.01 - 305", Value = "305" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.17 - 321", Value = "321" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.03 - 337", Value = "337" });
                dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.19 - 353", Value = "353" });
            }
            return Json(new
            {
                dates
            });
        }
    }
}