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
    public class CoordinateSystemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinateSystemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoordinateSystems
        public async Task<IActionResult> Index()
        {
            return View(await _context.CoordinateSystems.ToListAsync());
        }

        // GET: CoordinateSystems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinateSystems = await _context.CoordinateSystems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (coordinateSystems == null)
            {
                return NotFound();
            }

            return View(coordinateSystems);
        }

        // GET: CoordinateSystems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoordinateSystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CoordinateSystems coordinateSystems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinateSystems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coordinateSystems);
        }

        // GET: CoordinateSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinateSystems = await _context.CoordinateSystems.SingleOrDefaultAsync(m => m.Id == id);
            if (coordinateSystems == null)
            {
                return NotFound();
            }
            return View(coordinateSystems);
        }

        // POST: CoordinateSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CoordinateSystems coordinateSystems)
        {
            if (id != coordinateSystems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinateSystems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinateSystemsExists(coordinateSystems.Id))
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
            return View(coordinateSystems);
        }

        // GET: CoordinateSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinateSystems = await _context.CoordinateSystems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (coordinateSystems == null)
            {
                return NotFound();
            }

            return View(coordinateSystems);
        }

        // POST: CoordinateSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coordinateSystems = await _context.CoordinateSystems.SingleOrDefaultAsync(m => m.Id == id);
            _context.CoordinateSystems.Remove(coordinateSystems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinateSystemsExists(int id)
        {
            return _context.CoordinateSystems.Any(e => e.Id == id);
        }
    }
}
