using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoServer.Data;
using GeoServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace GeoServer.Controllers
{
    public class OLController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OLController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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
                new SelectListItem() { Text="Пастбища", Value="pastALA"},
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
            ViewBag.Year = new SelectList(Enumerable.Range(minYear, (maxYear - minYear) + 1), maxYear);

            ViewBag.GeoserverAddress = Startup.Configuration["GeoServer:Address"];
            return View();
        }

        public IActionResult ViewModisChart1(
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
            ViewBag.SelectYear = Year;
            ViewBag.ModisSource = ModisSource;
            ViewBag.ModisProduct = ModisProduct;
            ViewBag.ModisDataSet = ModisDataSet;
            int pastId = 0;
            try
            {
                pastId = Convert.ToInt32(PastId);
            }
            catch
            {

            }
            Pasture pasture = _context.Pasture.FirstOrDefault(p => p.Id == pastId);
            ViewBag.ClassId = pasture?.class_id;
            KATO kato = _context.KATO.FirstOrDefault(k => k.Number == KATO);
            ViewBag.KATOName = kato?.NameRU;

            int minYear = _context.ZonalStatKATO.Min(z => z.Year),
                maxYear = _context.ZonalStatKATO.Max(z => z.Year);
            ViewBag.Year = new SelectList(Enumerable.Range(minYear, (maxYear - minYear) + 1), maxYear);

            List<SelectListItem> month = new List<SelectListItem>();
            if (DateTime.IsLeapYear(Year))
            {
                string[] monthArray = { "Январь", "Февраль", "Март",
                    "Апрель", "Май", "Июнь", "Июль", "Август",
                    "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
                string[] value = { "1-1", "2-33", "3-65", "4-97", "5-129", "6-161", "7-193", "8-225", "9-257", "10-289", "11-321", "12-337" };
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == "1")
                    {
                        month.Add(new SelectListItem() { Text = monthArray[i], Value = value[i], Selected = true });
                    }
                    else
                    {
                        month.Add(new SelectListItem() { Text = monthArray[i], Value = value[i], Selected = false });
                    }
                }
            }
            else
            {
                string[] monthArray = { "Январь", "Февраль", "Март",
                    "Апрель", "Май", "Июнь", "Июль", "Август",
                    "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
                string[] value = { "1-1", "2-33", "3-65", "4-97", "5-129", "6-161", "7-193", "8-225", "9-257", "10-289", "11-305", "12-337" };
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == "1")
                    {
                        month.Add(new SelectListItem() { Text = monthArray[i], Value = value[i], Selected = true });
                    }
                    else
                    {
                        month.Add(new SelectListItem() { Text = monthArray[i], Value = value[i], Selected = false });
                    }
                }
            }

            List<SelectListItem> numberOfMonth = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                if (i == 12)
                {
                    numberOfMonth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = true });
                }
                else
                {
                    numberOfMonth.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = false });
                }
            }
            ViewBag.Month = month;
            ViewBag.NumberOfMonth = numberOfMonth;
            return View();
        }

        public IActionResult ViewModisChart2(
            string KATOType,
            string KATO,
            string PastId,
            string ModisSource,
            string ModisProduct,
            string ModisDataSet)
        {
            ViewBag.KATOType = KATOType;
            ViewBag.KATO = KATO;
            ViewBag.PastId = PastId;
            ViewBag.ModisSource = ModisSource;
            ViewBag.ModisProduct = ModisProduct;
            ViewBag.ModisDataSet = ModisDataSet;
            int pastId = 0;
            try
            {
                pastId = Convert.ToInt32(PastId);
            }
            catch
            {

            }
            Pasture pasture = _context.Pasture.FirstOrDefault(p => p.Id == pastId);
            ViewBag.ClassId = pasture?.class_id;
            KATO kato = _context.KATO.FirstOrDefault(k => k.Number == KATO);
            ViewBag.KATOName = kato?.NameRU;

            int minYear = _context.ZonalStatKATO.Min(z => z.Year),
                maxYear = _context.ZonalStatKATO.Max(z => z.Year);
            List<SelectListItem> year = new List<SelectListItem>();
            for (int i = minYear; i < maxYear + 1; i++)
            {
                year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = true });
            }
            ViewBag.Year = year;
            return View();
        }

        [HttpPost]
        public ActionResult GetKATOZonalStat(string KATO, int[] Year, string ModisSource, string ModisProduct, string ModisDataSet, string Month, int NumberOfMonth)
        {
            List<int> days = _context.ZonalStatKATO.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<ZonalStatKATO> currentKATO = new List<ZonalStatKATO>();
            List<decimal> current = new List<decimal>();
            List<int> years = new List<int>();
            List<int> labels = new List<int>(),
                nextYearLabels = new List<int>();
            List<decimal> min = new List<decimal>(),
                max = new List<decimal>(),
                average = new List<decimal>(),
                nextYearMin = new List<decimal>(),
                nextYearMax = new List<decimal>(),
                nextYearAverage = new List<decimal>();
            bool check = true;
            string[] value = { "1-1", "2-33", "3-65", "4-97", "5-129", "6-161", "7-193", "8-225", "9-257", "10-289", "11-305", "12-337" };
            var all = _context.ZonalStatKATO.Where(z => z.KATO == KATO && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            if (((NumberOfMonth - 1) + Convert.ToInt32(Month.Remove(Month.IndexOf('-'), (Month.Length - Month.IndexOf('-'))))) > 12) {
                int countMonthOfNextYear = ((NumberOfMonth - 1) + Convert.ToInt32(Month.Remove(Month.IndexOf('-'), (Month.Length - Month.IndexOf('-'))))) - 12;
                int DayOfMonthOfNextYear = 0;
                for (int i = 0; i < value.Length; i++)
                {
                    if (countMonthOfNextYear == Convert.ToInt32(value[i].Remove(value[i].IndexOf('-'), (value[i].Length - value[i].IndexOf('-')))))
                    {
                        DayOfMonthOfNextYear = Convert.ToInt32(value[i + 1].Substring(value[i + 1].IndexOf('-') + 1));
                        break;
                    }
                }
                foreach (int oneYear in Year)
                {
                    List<decimal> nextYearCurrent = new List<decimal>();
                    List<int> nextYears = new List<int>();
                    foreach (int day in days)
                    {
                        List<ZonalStatKATO> today = new List<ZonalStatKATO>();
                        if (day > Convert.ToInt32(Month.Substring(Month.IndexOf('-') + 1)) - 1)
                        {

                            today = all.Where(z => z.DayOfYear == day).ToList();
                            if (today.Count > 0)
                            {
                                ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == oneYear);
                                if (currenZonalStatKATO != null)
                                {
                                    current.Add(currenZonalStatKATO.Value);
                                    years.Add(oneYear);
                                }
                            }
                            if (check)
                            {
                                min.Add(today.Min(z => z.Value));
                                max.Add(today.Max(z => z.Value));
                                average.Add(today.Average(z => z.Value));
                                labels.Add(day);
                            }
                        }
                        else
                        {
                            if (day < DayOfMonthOfNextYear)
                            {
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == oneYear + 1);
                                    if (currenZonalStatKATO != null)
                                    {
                                        nextYearCurrent.Add(currenZonalStatKATO.Value);
                                        nextYears.Add(oneYear);
                                    }
                                }
                                if (check)
                                {
                                    nextYearMin.Add(today.Min(z => z.Value));
                                    nextYearMax.Add(today.Max(z => z.Value));
                                    nextYearAverage.Add(today.Average(z => z.Value));
                                    nextYearLabels.Add(day);
                                }
                            }
                        }
                    }
                    check = false;

                    for (int i = 0; i < nextYearCurrent.Count; i++)
                    {
                        current.Add(nextYearCurrent[i]);
                        years.Add(nextYears[i]);
                    }
                }
            }
            else
            {
                int endMonth = NumberOfMonth * 2;
                foreach (int oneYear in Year)
                {
                    int counter = 0;
                    foreach (int day in days)
                    {
                        List<ZonalStatKATO> today = new List<ZonalStatKATO>();
                        if (day > Convert.ToInt32(Month.Substring(Month.IndexOf('-') + 1)) - 1 && counter < endMonth)
                        {
                            if (DateTime.IsLeapYear(oneYear))
                            {
                                if (endMonth == 22)
                                {
                                    endMonth = endMonth - 1;
                                }
                                if (endMonth == 19)
                                {
                                    endMonth = endMonth + 1;
                                }
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == oneYear);
                                    if (currenZonalStatKATO != null)
                                    {
                                        current.Add(currenZonalStatKATO.Value);
                                        years.Add(oneYear);
                                        currentKATO.Add(currenZonalStatKATO);
                                    }
                                    if (check)
                                    {
                                        min.Add(today.Min(z => z.Value));
                                        max.Add(today.Max(z => z.Value));
                                        average.Add(today.Average(z => z.Value));
                                        labels.Add(day);
                                    }
                                }
                            }
                            else
                            {
                                if (endMonth == 20)
                                {
                                    endMonth = endMonth - 1;
                                }
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == oneYear);
                                    if (currenZonalStatKATO != null)
                                    {
                                        current.Add(currenZonalStatKATO.Value);
                                        years.Add(oneYear);
                                        currentKATO.Add(currenZonalStatKATO);
                                    }
                                }
                                if (check)
                                {
                                    min.Add(today.Min(z => z.Value));
                                    max.Add(today.Max(z => z.Value));
                                    average.Add(today.Average(z => z.Value));
                                    labels.Add(day);
                                }
                            }
                        }
                        counter++;
                    }
                    check = false;
                }
            }
            for (int i = 0; i < nextYearMin.Count; i++)
            {
                min.Add(nextYearMin[i]);
                max.Add(nextYearMax[i]);
                average.Add(nextYearAverage[i]);
                labels.Add(nextYearLabels[i]);
            }
            return Json(new
            {
                currentKATO,
                current,
                years,
                labels,
                min,
                max,
                average
            });
        }

        [HttpPost]
        public ActionResult GetPastZonalStat(string PastId, int[] Year, string ModisSource, string ModisProduct, string ModisDataSet, string Month, int NumberOfMonth)
        {
            List<int> days = _context.ZonalStatPast.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<decimal> current = new List<decimal>();
            List<int> years = new List<int>();
            List<int> labels = new List<int>(),
                nextYearLabels = new List<int>();
            List<decimal> min = new List<decimal>(),
                max = new List<decimal>(),
                average = new List<decimal>(),
                nextYearMin = new List<decimal>(),
                nextYearMax = new List<decimal>(),
                nextYearAverage = new List<decimal>();
            bool check = true;
            string[] value = { "1-1", "2-33", "3-65", "4-97", "5-129", "6-161", "7-193", "8-225", "9-257", "10-289", "11-305", "12-337" };
            var all = _context.ZonalStatPast.Where(z => z.PastId == PastId && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            if (((NumberOfMonth - 1) + Convert.ToInt32(Month.Remove(Month.IndexOf('-'), (Month.Length - Month.IndexOf('-'))))) > 12)
            {
                int countMonthOfNextYear = ((NumberOfMonth - 1) + Convert.ToInt32(Month.Remove(Month.IndexOf('-'), (Month.Length - Month.IndexOf('-'))))) - 12;
                int DayOfMonthOfNextYear = 0;
                for (int i = 0; i < value.Length; i++)
                {
                    if (countMonthOfNextYear == Convert.ToInt32(value[i].Remove(value[i].IndexOf('-'), (value[i].Length - value[i].IndexOf('-')))))
                    {
                        DayOfMonthOfNextYear = Convert.ToInt32(value[i + 1].Substring(value[i + 1].IndexOf('-') + 1));
                        break;
                    }
                }
                foreach (int oneYear in Year)
                {
                    List<decimal> nextYearCurrent = new List<decimal>();
                    List<int> nextYears = new List<int>();
                    foreach (int day in days)
                    {
                        List<ZonalStatPast> today = new List<ZonalStatPast>();
                        if (day > Convert.ToInt32(Month.Substring(Month.IndexOf('-') + 1)) - 1)
                        {

                            today = all.Where(z => z.DayOfYear == day).ToList();
                            if (today.Count > 0)
                            {
                                ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == oneYear);
                                if (currenZonalStatPast != null)
                                {
                                    current.Add(currenZonalStatPast.Value);
                                    years.Add(oneYear);
                                }
                            }
                            if (check)
                            {
                                min.Add(today.Min(z => z.Value));
                                max.Add(today.Max(z => z.Value));
                                average.Add(today.Average(z => z.Value));
                                labels.Add(day);
                            }
                        }
                        else
                        {
                            if (day < DayOfMonthOfNextYear)
                            {
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == oneYear + 1);
                                    if (currenZonalStatPast != null)
                                    {
                                        nextYearCurrent.Add(currenZonalStatPast.Value);
                                        nextYears.Add(oneYear);
                                    }
                                }
                                if (check)
                                {
                                    nextYearMin.Add(today.Min(z => z.Value));
                                    nextYearMax.Add(today.Max(z => z.Value));
                                    nextYearAverage.Add(today.Average(z => z.Value));
                                    nextYearLabels.Add(day);
                                }
                            }
                        }
                    }
                    check = false;

                    for (int i = 0; i < nextYearCurrent.Count; i++)
                    {
                        current.Add(nextYearCurrent[i]);
                        years.Add(nextYears[i]);
                    }
                }
            }
            else
            {
                int endMonth = NumberOfMonth * 2;
                foreach (int oneYear in Year)
                {
                    int counter = 0;
                    foreach (int day in days)
                    {
                        List<ZonalStatPast> today = new List<ZonalStatPast>();
                        if (day > Convert.ToInt32(Month.Substring(Month.IndexOf('-') + 1)) - 1 && counter < endMonth)
                        {
                            if (DateTime.IsLeapYear(oneYear))
                            {
                                if (endMonth == 22)
                                {
                                    endMonth = endMonth - 1;
                                }
                                if (endMonth == 19)
                                {
                                    endMonth = endMonth + 1;
                                }
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == oneYear);
                                    if (currenZonalStatPast != null)
                                    {
                                        current.Add(currenZonalStatPast.Value);
                                        years.Add(oneYear);
                                    }
                                    if (check)
                                    {
                                        min.Add(today.Min(z => z.Value));
                                        max.Add(today.Max(z => z.Value));
                                        average.Add(today.Average(z => z.Value));
                                        labels.Add(day);
                                    }
                                }
                            }
                            else
                            {
                                if (endMonth == 20)
                                {
                                    endMonth = endMonth - 1;
                                }
                                today = all.Where(z => z.DayOfYear == day).ToList();
                                if (today.Count > 0)
                                {
                                    ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == oneYear);
                                    if (currenZonalStatPast != null)
                                    {
                                        current.Add(currenZonalStatPast.Value);
                                        years.Add(oneYear);
                                    }
                                }
                                if (check)
                                {
                                    min.Add(today.Min(z => z.Value));
                                    max.Add(today.Max(z => z.Value));
                                    average.Add(today.Average(z => z.Value));
                                    labels.Add(day);
                                }
                            }
                        }
                        counter++;
                    }
                    check = false;
                }
            }
            for (int i = 0; i < nextYearMin.Count; i++)
            {
                min.Add(nextYearMin[i]);
                max.Add(nextYearMax[i]);
                average.Add(nextYearAverage[i]);
                labels.Add(nextYearLabels[i]);
            }
            return Json(new
            {
                current,
                years,
                labels,
                min,
                max,
                average
            });
        }

        //[HttpPost]
        //public ActionResult GetPastZonalStat(string PastId, int Year, string ModisSource, string ModisProduct, string ModisDataSet)
        //{
        //    List<int> days = _context.ZonalStatPast.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
        //    List<ZonalStatPast> current = new List<ZonalStatPast>();
        //    List<decimal> min = new List<decimal>(),
        //        max = new List<decimal>(),
        //        average = new List<decimal>();
        //    var all = _context.ZonalStatPast.Where(z => z.PastId == PastId && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
        //    foreach (int day in days)
        //    {
        //        List<ZonalStatPast> today = all.Where(z => z.DayOfYear == day).ToList();
        //        if (today.Count > 0)
        //        {
        //            ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == Year);
        //            if (currenZonalStatPast != null)
        //            {
        //                current.Add(currenZonalStatPast);
        //            }
        //            min.Add(today.Min(z => z.Value));
        //            max.Add(today.Max(z => z.Value));
        //            average.Add(today.Average(z => z.Value));
        //        }
        //    }
        //    return Json(new
        //    {
        //        current,
        //        min,
        //        max,
        //        average
        //    });
        //}

        [HttpPost]
        public ActionResult GetKATOZonalStatChart2(string KATO, int[] Year, string ModisSource, string ModisProduct, string ModisDataSet)
        {
            List<int> days = _context.ZonalStatKATO.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<decimal> current = new List<decimal>();
            List<int> years = new List<int>();
            List<decimal> average = new List<decimal>();
            var all = _context.ZonalStatKATO.Where(z => z.KATO == KATO && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            foreach (int oneYear in Year)
            {
                foreach (int day in days)
                {
                    List<ZonalStatKATO> today = new List<ZonalStatKATO>();
                    today = all.Where(z => z.DayOfYear == day).ToList();
                    if (today.Count > 0)
                    {
                        ZonalStatKATO currenZonalStatKATO = today.FirstOrDefault(z => z.Year == oneYear);
                        if (currenZonalStatKATO != null)
                        {
                            current.Add(currenZonalStatKATO.Value);
                            years.Add(oneYear);
                            average.Add(today.Average(z => z.Value));
                        }
                    }
                }
            }
            return Json(new
            {
                current,
                years,
                average
            });
        }

        [HttpPost]
        public ActionResult GetPastZonalStatChart2(string PastId, int[] Year, string ModisSource, string ModisProduct, string ModisDataSet)
        {
            List<int> days = _context.ZonalStatPast.Select(z => z.DayOfYear).Distinct().OrderBy(d => d).ToList();
            List<decimal> current = new List<decimal>();
            List<int> years = new List<int>();
            List<decimal> average = new List<decimal>();
            var all = _context.ZonalStatPast.Where(z => z.PastId == PastId && z.ModisSource == ModisSource && z.ModisProduct == ModisProduct && z.DataSet == ModisDataSet).ToList();
            foreach (int oneYear in Year)
            {
                foreach (int day in days)
                {
                    List<ZonalStatPast> today = new List<ZonalStatPast>();
                    today = all.Where(z => z.DayOfYear == day).ToList();
                    if (today.Count > 0)
                    {
                        ZonalStatPast currenZonalStatPast = today.FirstOrDefault(z => z.Year == oneYear);
                        if (currenZonalStatPast != null)
                        {
                            current.Add(currenZonalStatPast.Value);
                            years.Add(oneYear);
                            average.Add(today.Average(z => z.Value));
                        }
                    }
                }
            }
            return Json(new
            {
                current,
                years,
                average
            });
        }


        [HttpPost]
        public ActionResult GetYearDates(int Year)
        {
            List<SelectListItem> dates = new List<SelectListItem>();
            if(DateTime.IsLeapYear(Year))
            {
                int dayOfYear = _context.ZonalStatKATO.Where(m => m.Year == Year).Max(m => m.DayOfYear);
                string[] numberOf = { ".01.01 - 1", ".01.17 - 17", ".02.02 - 33", ".02.18 - 49", ".03.05 - 65",
                    ".03.21 - 81", ".04.06 - 97", ".04.22 - 113", ".05.08 - 129", ".05.24 - 145", ".06.09 - 161",
                    ".06.25 - 177", ".07.11 - 193", ".07.27 - 209", ".08.12 - 225", ".08.28 - 241", ".09.13 - 257",
                    ".09.29 - 273", ".10.15 - 289", ".10.31 - 305", ".11.16 - 321", ".12.02 - 337", ".12.18 - 353" };
                string[] value = { "1", "17", "33", "49", "65", "81", "97", "113", "129", "145", "161",
                    "177", "193", "209", "225", "241", "257", "273", "289", "305", "321", "337", "353" };
                for (int i = 0; i < value.Length; i++)
                {
                    if (dayOfYear == Convert.ToInt32(value[i]))
                    {
                        dates.Add(new SelectListItem() { Text = $"{Year.ToString()}" + numberOf[i], Value = value[i], Selected = true });
                    }
                    else
                    {
                        dates.Add(new SelectListItem() { Text = $"{Year.ToString()}" + numberOf[i], Value = value[i], Selected = false });
                    }
                }

                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.01 - 1", Value = "1" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.17 - 17", Value = "17" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.02 - 33", Value = "33" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.18 - 49", Value = "49" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.05 - 65", Value = "65" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.21 - 81", Value = "81" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.06 - 97", Value = "97" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.22 - 113", Value = "113" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.08 - 129", Value = "129" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.24 - 145", Value = "145" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.09 - 161", Value = "161" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.25 - 177", Value = "177" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.11 - 193", Value = "193" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.27 - 209", Value = "209" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.12 - 225", Value = "225" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.28 - 241", Value = "241" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.13 - 257", Value = "257" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.29 - 273", Value = "273" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.15 - 289", Value = "289" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.31 - 305", Value = "305" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.16 - 321", Value = "321" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.02 - 337", Value = "337" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.18 - 353", Value = "353" });
            }
            else
            {
                int dayOfYear = _context.ZonalStatKATO.Where(m => m.Year == Year).Max(m => m.DayOfYear);
                string[] numberOf = { ".01.01 - 1", ".01.17 - 17", ".02.02 - 33", ".02.18 - 49", ".03.06 - 65",
                    ".03.22 - 81", ".04.07 - 97", ".04.23 - 113", ".05.09 - 129", ".05.25 - 145", ".06.10 - 161",
                    ".06.26 - 177", ".07.12 - 193", ".07.28 - 209", ".08.13 - 225", ".08.29 - 241", ".09.14 - 257",
                    ".09.30 - 273", ".10.16 - 289", ".11.01 - 305", ".11.17 - 321", ".12.03 - 337", ".12.19 - 353" };
                string[] value = { "1", "17", "33", "49", "65", "81", "97", "113", "129", "145", "161",
                    "177", "193", "209", "225", "241", "257", "273", "289", "305", "321", "337", "353" };
                for (int i = 0; i < value.Length; i++)
                {
                    if(dayOfYear == Convert.ToInt32(value[i]))
                    {
                        dates.Add(new SelectListItem() { Text = $"{Year.ToString()}" + numberOf[i], Value = value[i], Selected = true });
                    }
                    else
                    {
                        dates.Add(new SelectListItem() { Text = $"{Year.ToString()}" + numberOf[i], Value = value[i], Selected = false });
                    }
                }

                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.01 - 1", Value = "1" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.01.17 - 17", Value = "17" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.02 - 33", Value = "33" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.02.18 - 49", Value = "49" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.06 - 65", Value = "65" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.03.22 - 81", Value = "81" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.07 - 97", Value = "97" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.04.23 - 113", Value = "113" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.09 - 129", Value = "129" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.05.25 - 145", Value = "145" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.10 - 161", Value = "161" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.06.26 - 177", Value = "177" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.12 - 193", Value = "193" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.07.28 - 209", Value = "209" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.13 - 225", Value = "225" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.08.29 - 241", Value = "241" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.14 - 257", Value = "257" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.09.30 - 273", Value = "273" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.10.16 - 289", Value = "289" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.01 - 305", Value = "305" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.11.17 - 321", Value = "321" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.03 - 337", Value = "337" });
                //dates.Add(new SelectListItem() { Text = $"{Year.ToString()}.12.19 - 353", Value = "353" });
            }
            return Json(new
            {
                dates
            });
        }

        [HttpPost]
        public ActionResult GetPasInfo(int pid,
            int class_id,
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

            Pasture pasture = _context.Pasture.FirstOrDefault(p => p.pid == pid);
            decimal e = pasture.E;

            return Json(new
            {
                class_name,
                otdely_name,
                subtype_name,
                group_name,
                recom_name,
                e
            });
        }

        [HttpPost]
        public ActionResult SaveToExcelChart1(string KATOName, string KATO, string[] Title, string[] Min, string[] Max, string[] Average, int[] Years, string[] Values)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");

            List<int> years = new List<int>();
            years.Add(Years[0]);
            int counter = 0;
            for (int i = 1; i < Years.Length; i++)
            {
                if (years[counter] != Years[i])
                {
                    years.Add(Years[i]);
                    counter++;
                }
                else
                {
                    continue;
                }
            }

            string sYear = "";
            for (int i = 0; i < years.Count; i++)
            {
                sYear = sYear + years[i] + "-";
            }
            sYear = sYear.Remove(sYear.Length - 1, 1);

            string sName = KATOName + " (" + KATO + ") " + sYear + " (Chart1)";
            string sFileName = $"{sName}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(KATOName);
                worksheet.Cells[1, 1].Value = Title[0];
                worksheet.Cells[1, 2].Value = Title[1];
                worksheet.Cells[1, 3].Value = Title[2];
                for (int col = 3; col < Title.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = Title[col];
                }

                for (int row = 0; row < Min.Length; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = Min[row];
                }

                for (int row = 0; row < Max.Length; row++)
                {
                    worksheet.Cells[row + 2, 2].Value = Max[row];
                }

                for (int row = 0; row < Average.Length; row++)
                {
                    worksheet.Cells[row + 2, 3].Value = Average[row];
                }

                for (int i = 0; i < years.Count; i++)
                {
                    int row = 2;
                    for (int j = 0; j < Values.Length; j++)
                    {
                        if (years[i] == Years[j])
                        {
                            worksheet.Cells[row, i + 4].Value = Values[j];
                            row++;
                        }
                    }
                }
                for(int i = 1; i < Title.Length + 1; i++)
                {
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Column(i).AutoFit();
                }
                package.Save();
            }
            //var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));
            //return File(fileBytes, mimeType, sFileName);

            string filepath = Url.Content("~/Download/" + file.Name);
            return Json(new
            {
                filepath
            });
        }

        [HttpPost]
        public ActionResult SaveToExcelChart2(string KATOName, string KATO, string[] Title, string[] Average, int[] Years, string[] Values)
        {
            string sContentRootPath = _hostingEnvironment.WebRootPath;
            sContentRootPath = Path.Combine(sContentRootPath, "Download");

            List<int> years = new List<int>();
            years.Add(Years[0]);
            int counter = 0;
            for (int i = 1; i < Years.Length; i++)
            {
                if (years[counter] != Years[i])
                {
                    years.Add(Years[i]);
                    counter++;
                }
                else
                {
                    continue;
                }
            }

            string sYear = "";
            for (int i = 0; i < years.Count; i++)
            {
                sYear = sYear + years[i] + "-";
            }
            sYear = sYear.Remove(sYear.Length - 1, 1);

            string sName = KATOName + " (" + KATO + ") " + sYear + " (Chart2)";
            string sFileName = $"{sName}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sContentRootPath, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(KATOName);
                worksheet.Cells[1, 1].Value = Title[0];
                for (int col = 1; col < Title.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = Title[col];
                }

                for (int row = 0; row < 23; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = Average[row];
                }

                for (int i = 0; i < years.Count; i++)
                {
                    int row = 2;
                    for (int j = 0; j < Values.Length; j++)
                    {
                        if (years[i] == Years[j])
                        {
                            worksheet.Cells[row, i + 2].Value = Values[j];
                            row++;
                        }
                    }
                }
                for (int i = 1; i < Title.Length + 1; i++)
                {
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Column(i).AutoFit();
                }
                package.Save();
            }
            //var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(sContentRootPath, file.Name));

            string filepath = Url.Content("~/Download/" + file.Name);
            return Json(new
            {
                filepath
            });
        }
    }
}