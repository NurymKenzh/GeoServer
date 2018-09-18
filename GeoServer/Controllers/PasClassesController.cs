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
    public class PasClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.PasClass.ToListAsync());
        }

        // GET: PasClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasClass = await _context.PasClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasClass == null)
            {
                return NotFound();
            }

            return View(pasClass);
        }

        // GET: PasClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name")] PasClass pasClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasClass);
        }

        // GET: PasClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasClass = await _context.PasClass.SingleOrDefaultAsync(m => m.Id == id);
            if (pasClass == null)
            {
                return NotFound();
            }
            return View(pasClass);
        }

        // POST: PasClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name")] PasClass pasClass)
        {
            if (id != pasClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasClassExists(pasClass.Id))
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
            return View(pasClass);
        }

        // GET: PasClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasClass = await _context.PasClass
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasClass == null)
            {
                return NotFound();
            }

            return View(pasClass);
        }

        // POST: PasClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasClass = await _context.PasClass.SingleOrDefaultAsync(m => m.Id == id);
            _context.PasClass.Remove(pasClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasClassExists(int id)
        {
            return _context.PasClass.Any(e => e.Id == id);
        }
    }
}
