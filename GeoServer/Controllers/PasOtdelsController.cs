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
    public class PasOtdelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasOtdelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasOtdels
        public async Task<IActionResult> Index()
        {
            return View(await _context.PasOtdel.ToListAsync());
        }

        // GET: PasOtdels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasOtdel = await _context.PasOtdel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasOtdel == null)
            {
                return NotFound();
            }

            return View(pasOtdel);
        }

        // GET: PasOtdels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasOtdels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name")] PasOtdel pasOtdel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasOtdel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasOtdel);
        }

        // GET: PasOtdels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasOtdel = await _context.PasOtdel.SingleOrDefaultAsync(m => m.Id == id);
            if (pasOtdel == null)
            {
                return NotFound();
            }
            return View(pasOtdel);
        }

        // POST: PasOtdels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name")] PasOtdel pasOtdel)
        {
            if (id != pasOtdel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasOtdel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasOtdelExists(pasOtdel.Id))
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
            return View(pasOtdel);
        }

        // GET: PasOtdels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasOtdel = await _context.PasOtdel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasOtdel == null)
            {
                return NotFound();
            }

            return View(pasOtdel);
        }

        // POST: PasOtdels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasOtdel = await _context.PasOtdel.SingleOrDefaultAsync(m => m.Id == id);
            _context.PasOtdel.Remove(pasOtdel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasOtdelExists(int id)
        {
            return _context.PasOtdel.Any(e => e.Id == id);
        }
    }
}
