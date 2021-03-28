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


          public async Task<IActionResult> Index()
          {
               return View(await _context.ReservationTypes.ToListAsync());
          }


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


          public IActionResult Create()
          {
               return View();
          }


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

          private bool ReservationTypeExists(string id)
          {
               return _context.ReservationTypes.Any(e => e.Id == id);
          }
     }
}
