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
    public class ZonalStatPastsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZonalStatPastsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZonalStatPasts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ZonalStatPast.ToListAsync());
        }

        // GET: ZonalStatPasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatPast = await _context.ZonalStatPast
                .SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatPast == null)
            {
                return NotFound();
            }

            return View(zonalStatPast);
        }

        // GET: ZonalStatPasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZonalStatPasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PastId,Year,DayOfYear,ModisSource,ModisProduct,DataSet,Value")] ZonalStatPast zonalStatPast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zonalStatPast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zonalStatPast);
        }

        // GET: ZonalStatPasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatPast = await _context.ZonalStatPast.SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatPast == null)
            {
                return NotFound();
            }
            return View(zonalStatPast);
        }

        // POST: ZonalStatPasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PastId,Year,DayOfYear,ModisSource,ModisProduct,DataSet,Value")] ZonalStatPast zonalStatPast)
        {
            if (id != zonalStatPast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zonalStatPast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZonalStatPastExists(zonalStatPast.Id))
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
            return View(zonalStatPast);
        }

        // GET: ZonalStatPasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonalStatPast = await _context.ZonalStatPast
                .SingleOrDefaultAsync(m => m.Id == id);
            if (zonalStatPast == null)
            {
                return NotFound();
            }

            return View(zonalStatPast);
        }

        // POST: ZonalStatPasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zonalStatPast = await _context.ZonalStatPast.SingleOrDefaultAsync(m => m.Id == id);
            _context.ZonalStatPast.Remove(zonalStatPast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZonalStatPastExists(int id)
        {
            return _context.ZonalStatPast.Any(e => e.Id == id);
        }
    }
}
