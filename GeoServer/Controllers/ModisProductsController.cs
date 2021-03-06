﻿using System;
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
    public class ModisProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModisProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModisProducts
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ModisProduct.Include(m => m.ModisSource);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ModisProducts/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisProduct = await _context.ModisProduct
                .Include(m => m.ModisSource)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisProduct == null)
            {
                return NotFound();
            }

            return View(modisProduct);
        }

        // GET: ModisProducts/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ModisSourceId"] = new SelectList(_context.ModisSource, "Id", "Name");
            return View();
        }

        // POST: ModisProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,ModisSourceId,Name")] ModisProduct modisProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modisProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModisSourceId"] = new SelectList(_context.ModisSource, "Id", "Name", modisProduct.ModisSourceId);
            return View(modisProduct);
        }

        // GET: ModisProducts/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisProduct = await _context.ModisProduct.SingleOrDefaultAsync(m => m.Id == id);
            if (modisProduct == null)
            {
                return NotFound();
            }
            ViewData["ModisSourceId"] = new SelectList(_context.ModisSource, "Id", "Name", modisProduct.ModisSourceId);
            return View(modisProduct);
        }

        // POST: ModisProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModisSourceId,Name")] ModisProduct modisProduct)
        {
            if (id != modisProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modisProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModisProductExists(modisProduct.Id))
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
            ViewData["ModisSourceId"] = new SelectList(_context.ModisSource, "Id", "Name", modisProduct.ModisSourceId);
            return View(modisProduct);
        }

        // GET: ModisProducts/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modisProduct = await _context.ModisProduct
                .Include(m => m.ModisSource)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modisProduct == null)
            {
                return NotFound();
            }

            return View(modisProduct);
        }

        // POST: ModisProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modisProduct = await _context.ModisProduct.SingleOrDefaultAsync(m => m.Id == id);
            _context.ModisProduct.Remove(modisProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModisProductExists(int id)
        {
            return _context.ModisProduct.Any(e => e.Id == id);
        }
    }
}
