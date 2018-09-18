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
    public class PasRecomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasRecomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasRecoms
        public async Task<IActionResult> Index()
        {
            return View(await _context.PasRecom.ToListAsync());
        }

        // GET: PasRecoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasRecom = await _context.PasRecom
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasRecom == null)
            {
                return NotFound();
            }

            return View(pasRecom);
        }

        // GET: PasRecoms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasRecoms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name")] PasRecom pasRecom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasRecom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasRecom);
        }

        // GET: PasRecoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasRecom = await _context.PasRecom.SingleOrDefaultAsync(m => m.Id == id);
            if (pasRecom == null)
            {
                return NotFound();
            }
            return View(pasRecom);
        }

        // POST: PasRecoms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name")] PasRecom pasRecom)
        {
            if (id != pasRecom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasRecom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasRecomExists(pasRecom.Id))
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
            return View(pasRecom);
        }

        // GET: PasRecoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasRecom = await _context.PasRecom
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasRecom == null)
            {
                return NotFound();
            }

            return View(pasRecom);
        }

        // POST: PasRecoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasRecom = await _context.PasRecom.SingleOrDefaultAsync(m => m.Id == id);
            _context.PasRecom.Remove(pasRecom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasRecomExists(int id)
        {
            return _context.PasRecom.Any(e => e.Id == id);
        }
    }
}
