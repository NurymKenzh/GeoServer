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
    public class ModisDataSetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModisDataSetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModisDataSets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ModisDataSet.Include(m => m.ModisProduct);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ModisDataSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisDataSet = await _context.ModisDataSet
                .Include(m => m.ModisProduct)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisDataSet == null)
            {
                return NotFound();
            }

            return View(modisDataSet);
        }

        // GET: ModisDataSets/Create
        public IActionResult Create()
        {
            ViewData["ModisProductId"] = new SelectList(_context.ModisProduct, "Id", "Name");
            return View();
        }

        // POST: ModisDataSets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModisProductId,Index,Name,Description,Units,DataType,FillValue,ValidRange,ScalingFactor")] ModisDataSet modisDataSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modisDataSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModisProductId"] = new SelectList(_context.ModisProduct, "Id", "Name", modisDataSet.ModisProductId);
            return View(modisDataSet);
        }

        // GET: ModisDataSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisDataSet = await _context.ModisDataSet.SingleOrDefaultAsync(m => m.Id == id);
            if (modisDataSet == null)
            {
                return NotFound();
            }
            ViewData["ModisProductId"] = new SelectList(_context.ModisProduct, "Id", "Name", modisDataSet.ModisProductId);
            return View(modisDataSet);
        }

        // POST: ModisDataSets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModisProductId,Index,Name,Description,Units,DataType,FillValue,ValidRange,ScalingFactor")] ModisDataSet modisDataSet)
        {
            if (id != modisDataSet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modisDataSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModisDataSetExists(modisDataSet.Id))
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
            ViewData["ModisProductId"] = new SelectList(_context.ModisProduct, "Id", "Name", modisDataSet.ModisProductId);
            return View(modisDataSet);
        }

        // GET: ModisDataSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisDataSet = await _context.ModisDataSet
                .Include(m => m.ModisProduct)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisDataSet == null)
            {
                return NotFound();
            }

            return View(modisDataSet);
        }

        // POST: ModisDataSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modisDataSet = await _context.ModisDataSet.SingleOrDefaultAsync(m => m.Id == id);
            _context.ModisDataSet.Remove(modisDataSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModisDataSetExists(int id)
        {
            return _context.ModisDataSet.Any(e => e.Id == id);
        }
    }
}
