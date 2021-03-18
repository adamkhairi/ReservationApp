using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Help;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Roles = "Admin")]
     public class ReservationController : Controller
     {
          private readonly ApplicationDbContext _context;
          private readonly UserManager<IdentityUser> _userManager;

          public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
          {
               _context = context;
               _userManager = userManager;

          }


          public async Task<IActionResult> Index()
          {
               ViewBag.StatusList = Helpers.StatusList();
               var list = await _context.Reservations
                   .Include(x => x.ReservationType)
                   .Include(s => s.Student)
                   .Where(x => x.Status == Status.Pending.ToString())
                   .OrderBy(r => r.Date)
                   .ToListAsync();
               //return AbsenceHistories;
               return View("List", list);
          }


          public async Task<ActionResult> FilterByDate(DateTime filterDate)
          {
               if (filterDate.Year == 0001)
               {
                    return RedirectToAction(nameof(Index));
               }
               else
               {
                    ViewBag.StatusList = Helpers.StatusList();

                    var list = await _context.Reservations
                                   .Include(t => t.ReservationType)
                                   .Include(s => s.Student)
                                   .OrderBy(r => r.Date)
                                   .Where(r => r.Date == filterDate)
                                   .ToListAsync();
                    return View("List", list);
               }
          }

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


          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(string id)
          {
               var reservation = await _context.Reservations.FindAsync(id);
               _context.Reservations.Remove(reservation);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
          }

          [HttpPost]
          public async Task<IActionResult> Submit(string id, string status)
          {
               var reservation = await _context.Reservations
                   .Include(r => r.Student)
                   .FirstAsync(r => r.Id == id);
               reservation.Status = status;
               _context.Update(reservation);

               if (status == Status.Approved.ToString())
               {
                    var UId = reservation.Student.Id;
                    var student = await _context.FindAsync<Models.Student>(UId);


                    student.ReservationCount += 1;


                    _context.Update(student);
               }
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));

          }

          private bool ReservationExists(string id)
          {
               return _context.Reservations.Any(e => e.Id == id);
          }
     }
}
