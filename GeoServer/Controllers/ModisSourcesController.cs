using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoServer.Data;
using GeoServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace GeoServer.Controllers
{
    public class ModisSourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModisSourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModisSources
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModisSource.ToListAsync());
        }

        // GET: ModisSources/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSource = await _context.ModisSource
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisSource == null)
            {
                return NotFound();
            }

            return View(modisSource);
        }

        // GET: ModisSources/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModisSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name")] ModisSource modisSource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modisSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modisSource);
        }

        // GET: ModisSources/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSource = await _context.ModisSource.SingleOrDefaultAsync(m => m.Id == id);
            if (modisSource == null)
            {
                return NotFound();
            }
            return View(modisSource);
        }

        // POST: ModisSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ModisSource modisSource)
        {
            if (id != modisSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modisSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModisSourceExists(modisSource.Id))
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
            return View(modisSource);
        }

        // GET: ModisSources/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSource = await _context.ModisSource
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisSource == null)
            {
                return NotFound();
            }

            return View(modisSource);
        }

        // POST: ModisSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modisSource = await _context.ModisSource.SingleOrDefaultAsync(m => m.Id == id);
            _context.ModisSource.Remove(modisSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModisSourceExists(int id)
        {
            return _context.ModisSource.Any(e => e.Id == id);
        }
    }
}
