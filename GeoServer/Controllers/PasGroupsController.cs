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
    public class PasGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PasGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.PasGroup.ToListAsync());
        }

        // GET: PasGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasGroup = await _context.PasGroup
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasGroup == null)
            {
                return NotFound();
            }

            return View(pasGroup);
        }

        // GET: PasGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,NameLat")] PasGroup pasGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pasGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pasGroup);
        }

        // GET: PasGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasGroup = await _context.PasGroup.SingleOrDefaultAsync(m => m.Id == id);
            if (pasGroup == null)
            {
                return NotFound();
            }
            return View(pasGroup);
        }

        // POST: PasGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,NameLat")] PasGroup pasGroup)
        {
            if (id != pasGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pasGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasGroupExists(pasGroup.Id))
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
            return View(pasGroup);
        }

        // GET: PasGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pasGroup = await _context.PasGroup
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pasGroup == null)
            {
                return NotFound();
            }

            return View(pasGroup);
        }

        // POST: PasGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pasGroup = await _context.PasGroup.SingleOrDefaultAsync(m => m.Id == id);
            _context.PasGroup.Remove(pasGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasGroupExists(int id)
        {
            return _context.PasGroup.Any(e => e.Id == id);
        }
    }
}
