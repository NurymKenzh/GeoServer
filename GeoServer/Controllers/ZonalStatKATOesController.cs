using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoServer.Data;
using GeoServer.Models;

namespace GeoServer.Controllers
{
    public class ZonalStatKATOesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZonalStatKATOesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZonalStatKATOes
        public async Task<IActionResult> Index(string SortOrder,
            string KATO,
            int? Year,
            int? DayOfYear,
            string ModisSource,
            string ModisProduct,
            string DataSet,
            int? Page)
        {
            var zonalStatKATOes = _context.ZonalStatKATO
                .Where(z => true);

            ViewBag.KATOFilter = KATO;
            ViewBag.YearFilter = Year;
            ViewBag.DayOfYearFilter = DayOfYear;
            ViewBag.ModisSourceFilter = ModisSource;
            ViewBag.ModisProductFilter = ModisProduct;
            ViewBag.DataSetFilter = DataSet;

            ViewBag.KATOSort = SortOrder == "KATO" ? "KATODesc" : "KATO";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.DayOfYearSort = SortOrder == "DayOfYear" ? "DayOfYearDesc" : "DayOfYear";
            ViewBag.ModisSourceSort = SortOrder == "ModisSource" ? "ModisSourceDesc" : "ModisSource";
            ViewBag.ModisProductSort = SortOrder == "ModisProduct" ? "ModisProductDesc" : "ModisProduct";
            ViewBag.DataSetSort = SortOrder == "DataSet" ? "DataSetDesc" : "DataSet";

            if (!string.IsNullOrEmpty(KATO))
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.KATO.ToLower().Contains(KATO.ToLower()));
            }
            if (Year!=null)
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.Year == Year);
            }
            if (DayOfYear != null)
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.DayOfYear == DayOfYear);
            }
            if (!string.IsNullOrEmpty(ModisSource))
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.ModisSource.ToLower().Contains(ModisSource.ToLower()));
            }
            if (!string.IsNullOrEmpty(ModisProduct))
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.ModisProduct.ToLower().Contains(ModisProduct.ToLower()));
            }
            if (!string.IsNullOrEmpty(DataSet))
            {
                zonalStatKATOes = zonalStatKATOes.Where(z => z.DataSet.ToLower().Contains(DataSet.ToLower()));
            }

            switch (SortOrder)
            {
                case "KATO":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.KATO);
                    break;
                case "KATODesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.KATO);
                    break;
                case "Year":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.Year);
                    break;
                case "YearDesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.Year);
                    break;
                case "DayOfYear":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.DayOfYear);
                    break;
                case "DayOfYearDesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.DayOfYear);
                    break;
                case "ModisSource":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.ModisSource);
                    break;
                case "ModisSourceDesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.ModisSource);
                    break;
                case "ModisProduct":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.ModisProduct);
                    break;
                case "ModisProductDesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.ModisProduct);
                    break;
                case "DataSet":
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.DataSet);
                    break;
                case "DataSetDesc":
                    zonalStatKATOes = zonalStatKATOes.OrderByDescending(z => z.DataSet);
                    break;
                default:
                    zonalStatKATOes = zonalStatKATOes.OrderBy(z => z.Id);
                    break;
            }
            ViewBag.SortOrder = SortOrder;

            var pager = new Pager(zonalStatKATOes.Count(), Page);

            var viewModel = new ZonalStatKATOIndexPageViewModel
            {
                Items = zonalStatKATOes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(viewModel);
        }

        // GET: ZonalStatKATOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatKATO = await _context.ZonalStatKATO
                .SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatKATO == null)
            {
                return NotFound();
            }

            return View(zonalStatKATO);
        }

        // GET: ZonalStatKATOes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZonalStatKATOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KATO,Year,DayOfYear,ModisSource,ModisProduct,DataSet,Value")] ZonalStatKATO zonalStatKATO)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zonalStatKATO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zonalStatKATO);
        }

        // GET: ZonalStatKATOes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatKATO = await _context.ZonalStatKATO.SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatKATO == null)
            {
                return NotFound();
            }
            return View(zonalStatKATO);
        }

        // POST: ZonalStatKATOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KATO,Year,DayOfYear,ModisSource,ModisProduct,DataSet,Value")] ZonalStatKATO zonalStatKATO)
        {
            if (id != zonalStatKATO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zonalStatKATO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZonalStatKATOExists(zonalStatKATO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(zonalStatKATO);
        }

        // GET: ZonalStatKATOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatKATO = await _context.ZonalStatKATO
                .SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatKATO == null)
            {
                return NotFound();
            }

            return View(zonalStatKATO);
        }

        // POST: ZonalStatKATOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zonalStatKATO = await _context.ZonalStatKATO.SingleOrDefaultAsync(m => m.Id == id);
            _context.ZonalStatKATO.Remove(zonalStatKATO);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZonalStatKATOExists(int id)
        {
            return _context.ZonalStatKATO.Any(e => e.Id == id);
        }
    }
}
