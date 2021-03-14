using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Roles = "Admin")]
     public class ReservationController : Controller
     {
          private readonly ApplicationDbContext _context;

          public ReservationController(ApplicationDbContext context)
          {
               _context = context;
          }

          // GET: Admin/Reservation
          public async Task<IActionResult> Index()
          {
               //var applicationDbContext = _context.Reservations.Include(r => r.Student);
               //var Lisst = (

               //        from r in _context.Reservations
               //        join s in _context.Students
               //            on r.Student.Id equals s.Id
               //        join rt in _context.ReservationTypes
               //            on r.ReservationType.Id equals rt.Id
               //        select new ReservationStudentViewModel()
               //        {
               //            Id = s.Id,
               //            UserName = s.UserName,
               //            Email = s.Email,
               //            Date = r.Date,
               //            Cause = r.Cause,
               //            Status = r.Status,
               //            ReservationType = rt.Name,
               //        }
               //);

               //var xx = _context.ReservationTypes;
               var List = await _context.Reservations
                   .Include(x => x.ReservationType)
                   .Include(s => s.Student)


                   .ToListAsync();
               //return AbsenceHistories;
               return View("List", List);
          }

          // GET: Admin/Reservation/Details/5
          public async Task<IActionResult> Details(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations
                   .Include(r => r.Student)
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (reservation == null)
               {
                    return NotFound();
               }

               return View(reservation);
          }

          // GET: Admin/Reservation/Edit/5
          public async Task<IActionResult> Edit(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations.FindAsync(id);
               if (reservation == null)
               {
                    return NotFound();
               }
               ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", reservation.Student.Id);
               return View(reservation);
          }

          // POST: Admin/Reservation/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(string id, [Bind("Id,Date,Status,Cause,StudentId,TypeId")] Reservation reservation)
          {
               if (id != reservation.Id)
               {
                    return NotFound();
               }

               if (ModelState.IsValid)
               {
                    try
                    {
                         _context.Update(reservation);
                         await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                         if (!ReservationExists(reservation.Id))
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
               ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", reservation.Student.Id);
               return View(reservation);
          }

          // GET: Admin/Reservation/Delete/5
          public async Task<IActionResult> Delete(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations
                   .Include(r => r.Student)
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (reservation == null)
               {
                    return NotFound();
               }

               return View(reservation);
          }

          // POST: Admin/Reservation/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(string id)
          {
               var reservation = await _context.Reservations.FindAsync(id);
               _context.Reservations.Remove(reservation);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
          }

          private bool ReservationExists(string id)
          {
               return _context.Reservations.Any(e => e.Id == id);
          }
     }
}
