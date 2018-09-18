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
    public class PasSubtypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasSubtypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasSubtypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PasSubtype.ToListAsync());
        }

        // GET: PasSubtypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasSubtype = await _context.PasSubtype
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasSubtype == null)
            {
                return NotFound();
            }

            return View(pasSubtype);
        }

        // GET: PasSubtypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasSubtypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name")] PasSubtype pasSubtype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasSubtype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasSubtype);
        }

        // GET: PasSubtypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasSubtype = await _context.PasSubtype.SingleOrDefaultAsync(m => m.Id == id);
            if (pasSubtype == null)
            {
                return NotFound();
            }
            return View(pasSubtype);
        }

        // POST: PasSubtypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name")] PasSubtype pasSubtype)
        {
            if (id != pasSubtype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasSubtype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasSubtypeExists(pasSubtype.Id))
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
            return View(pasSubtype);
        }

        // GET: PasSubtypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasSubtype = await _context.PasSubtype
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasSubtype == null)
            {
                return NotFound();
            }

            return View(pasSubtype);
        }

        // POST: PasSubtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasSubtype = await _context.PasSubtype.SingleOrDefaultAsync(m => m.Id == id);
            _context.PasSubtype.Remove(pasSubtype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasSubtypeExists(int id)
        {
            return _context.PasSubtype.Any(e => e.Id == id);
        }
    }
}
