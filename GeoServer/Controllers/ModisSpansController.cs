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
    public class ModisSpansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModisSpansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModisSpans
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModisSpan.ToListAsync());
        }

        // GET: ModisSpans/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSpan = await _context.ModisSpan
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisSpan == null)
            {
                return NotFound();
            }

            return View(modisSpan);
        }

        // GET: ModisSpans/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModisSpans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name")] ModisSpan modisSpan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modisSpan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modisSpan);
        }

        // GET: ModisSpans/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSpan = await _context.ModisSpan.SingleOrDefaultAsync(m => m.Id == id);
            if (modisSpan == null)
            {
                return NotFound();
            }
            return View(modisSpan);
        }

        // POST: ModisSpans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ModisSpan modisSpan)
        {
            if (id != modisSpan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modisSpan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModisSpanExists(modisSpan.Id))
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
            return View(modisSpan);
        }

        // GET: ModisSpans/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisSpan = await _context.ModisSpan
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisSpan == null)
            {
                return NotFound();
            }

            return View(modisSpan);
        }

        // POST: ModisSpans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modisSpan = await _context.ModisSpan.SingleOrDefaultAsync(m => m.Id == id);
            _context.ModisSpan.Remove(modisSpan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModisSpanExists(int id)
        {
            return _context.ModisSpan.Any(e => e.Id == id);
        }
    }
}
