using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;
using ReservationApp.Models;
using ReservationApp.ViewModels;
using ReservationApp.Help;
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


          public async Task<IActionResult> Index()
          {
               var user = await _userManager.GetUserAsync(User);

               var tomorrow = Helpers.CurrentDay().AddDays(1);
               var UserReservation = await _context.SudentResOfDay(user.Id, tomorrow);
               var ApprovedReservation = await _context.SudentApprovedRes(user.Id);
               return View(UserReservation);
          }


          public async Task<IActionResult> Details(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.ResByIdViewModel(id);

               if (reservation == null)
               {
                    return NotFound();
               }

               return View(reservation);
          }


          [HttpGet]
          public IActionResult Create()
          {
               var reservationType = _context.ReservationTypes.Select(t => new SelectListItem
               {
                    Value = t.Id,
                    Text = t.Name
               });

               ViewBag.StatusList = Helpers.StatusList();
               ViewBag.ResType = reservationType;
               return View();
          }


          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create(ReservationStudentViewModel reservation)
          {
               if (ModelState.IsValid)
               {

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var type = await _context.GetReservationTypeById(reservation.ReservationTypeId);

                    Reservation res = new Reservation
                    {
                         Date = reservation.Date,
                         Status = reservation.Status,
                         Cause = reservation.Cause,
                         StudentId = userId,
                         ReservationTypeId = type.Id,
                    };

                    _context.Add(res);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
               return View(reservation);
          }


          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(string id, Reservation reservation)
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

          public async Task<IActionResult> Delete(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.ResByID(id);
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
               var reservation = await _context.ResByID(id);

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
