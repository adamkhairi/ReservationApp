using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ReservationTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReservationTypes.ToListAsync());
        }

        // GET: Admin/ReservationTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationType == null)
            {
                return NotFound();
            }

            return View(reservationType);
        }

        // GET: Admin/ReservationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReservationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AccessNumber")] ReservationType reservationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservationType);
        }

        // GET: Admin/ReservationTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationTypes.FindAsync(id);
            if (reservationType == null)
            {
                return NotFound();
            }
            return View(reservationType);
        }

        // POST: Admin/ReservationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,AccessNumber")] ReservationType reservationType)
        {
            if (id != reservationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationTypeExists(reservationType.Id))
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
            return View(reservationType);
        }

        // GET: Admin/ReservationTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationType = await _context.ReservationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationType == null)
            {
                return NotFound();
            }

            return View(reservationType);
        }

        // POST: Admin/ReservationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var reservationType = await _context.ReservationTypes.FindAsync(id);
            _context.ReservationTypes.Remove(reservationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationTypeExists(string id)
        {
            return _context.ReservationTypes.Any(e => e.Id == id);
        }
    }
}
