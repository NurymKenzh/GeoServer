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
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZonalStatKATO.ToListAsync());
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
