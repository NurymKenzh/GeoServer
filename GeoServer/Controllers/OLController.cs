using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoServer.Data;
using GeoServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
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
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("ru")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );


            ViewBag.KATOType = new List<SelectListItem>()
            {
                new SelectListItem() { Text="Области", Value="adm1pol"},
                new SelectListItem() { Text="Районы", Value="adm2pol"},
                new SelectListItem() { Text="Сельские округи", Value="adm3pol"},
                new SelectListItem() { Text="Пастбища", Value="pastALA"}
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

        public IActionResult ViewModisChart(
            string KATOType,
            string KATO,
            string PastId,
            int Year,
            string ModisSource,
            string ModisProduct,
            string ModisDataSet)
        {
            ViewBag.KATOType = KATOType;
            ViewBag.KATO = KATO;
            ViewBag.PastId = PastId;
            ViewBag.Year = Year;
            ViewBag.ModisSource = ModisSource;
            ViewBag.ModisProduct = ModisProduct;
            ViewBag.ModisDataSet = ModisDataSet;
            return View();
        }

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
                if (today.Count > 0)
                {
                    ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == Year);
                    if(currenZonalStatKATO!=null)
                    {
                        current.Add(currenZonalStatKATO);
                    }
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
        public ActionResult GetPastZonalStat(string PastId, int Year, string ModisSource, string ModisProduct, string ModisDataSet)
        {
            List<int> days = _context.ZonalStatPast.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<ZonalStatPast> current = new List<ZonalStatPast>();
            List<decimal> min = new List<decimal>(),
                max = new List<decimal>(),
                average = new List<decimal>();
            var all = _context.ZonalStatPast.Where(z => z.PastId == PastId && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            foreach (int day in days)
            {
                List<ZonalStatPast> today = all.Where(z => z.DayOfYear == day).ToList();
                if (today.Count > 0)
                {
                    ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == Year);
                    if (currenZonalStatPast != null)
                    {
                        current.Add(currenZonalStatPast);
                    }
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

        [HttpPost]
        public ActionResult GetPasInfo(int class_id,
            int otdely_id,
            int subtype_id,
            int group_id,
            int recom_id)
        {
            string class_name = _context.PasClass.FirstOrDefault(p => p.Code == class_id).Name,
                otdely_name = _context.PasOtdel.FirstOrDefault(p => p.Code == otdely_id).Name,
                subtype_name = _context.PasSubtype.FirstOrDefault(p => p.Code == subtype_id).Name,
                group_name = _context.PasGroup.FirstOrDefault(p => p.Code == group_id).Name,
                group_nameLat = _context.PasGroup.FirstOrDefault(p => p.Code == group_id).NameLat,
                recom_name = _context.PasRecom.FirstOrDefault(p => p.Code == recom_id).Name;
            if(!string.IsNullOrEmpty(group_nameLat))
            {
                group_name += $" ({group_nameLat})";
            }
            return Json(new
            {
                class_name,
                otdely_name,
                subtype_name,
                group_name,
                recom_name
            });
        }
    }
}