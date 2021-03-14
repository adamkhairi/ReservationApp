using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Areas.Student.Controllers
{
     [Area("Student")]
     [Authorize(Roles = "Student")]
     public class ReservationController : Controller
     {
          private readonly ApplicationDbContext _context;
          private readonly UserManager<IdentityUser> _userManager;
          public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
          {
              _context = context;
              _userManager = userManager;
          }

          // GET: Student/Reservation
          public async Task<IActionResult> Index()
          {
               return View(await _context.Reservations.ToListAsync());
          }

          // GET: Student/Reservation/Details/5
          public async Task<IActionResult> Details(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (reservation == null)
               {
                    return NotFound();
               }

               return View(reservation);
          }

          // GET: Student/Reservation/Create
          public IActionResult Create()
          {
               var reservationType = _context.ReservationTypes.Select(t => new SelectListItem
               {
                    Value = t.Id,
                    Text = t.Name
               });
               ViewBag.ResType = reservationType;
               return View();
          }

          // POST: Student/Reservation/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("Id,Date,Status,Cause,CreateDate")] ReservationStudentViewModel reservation)
          {
               if (ModelState.IsValid)
               {
                   if (User.Identity != null) return NotFound();
                       var user = await _userManager.FindByNameAsync(User.Identity.Name);

                       Reservation res = new Reservation
                       {
                           Date = reservation.Date,
                           Status = reservation.Status,
                           Cause = reservation.Cause,
                           Student = user,
                       };
                           

                       var type = _context.ReservationTypes.SingleOrDefault(t => t.Id == reservation.ReservationTypeId);
                       res.ReservationType = type;
                   

                   _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
               }
               return View(reservation);
          }

          // GET: Student/Reservation/Edit/5
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
               return View(reservation);
          }

          // POST: Student/Reservation/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(string id, [Bind("Id,Date,Status,Cause,CreateDate")] Reservation reservation)
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
               return View(reservation);
          }

          // GET: Student/Reservation/Delete/5
          public async Task<IActionResult> Delete(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (reservation == null)
               {
                    return NotFound();
               }

               return View(reservation);
          }

          // POST: Student/Reservation/Delete/5
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
