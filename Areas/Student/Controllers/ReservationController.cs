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
               //var res = await _context.Reservations.ToListAsync();

               var user = await _userManager.GetUserAsync(User);
               var UserReservation = await _context.Reservations
                   .Select(res => new ReservationStudentViewModel
                   {
                        Id = res.Id,
                        StudentId = res.StudentId,
                        Date = res.Date,
                        Status = res.Status,
                        Cause = res.Cause,
                        ReservationTypeId = res.ReservationType.Id,
                        Name = res.ReservationType.Name,
                        Student = res.Student,
                        CreateDate = res.CreateDate,
                   })
               .Where(res => res.StudentId == user.Id)
               .ToListAsync();
               return View(UserReservation);
          }


          public async Task<IActionResult> Details(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var reservation = await _context.Reservations
                   .Select(m => new ReservationStudentViewModel
                     {
                          Id = m.Id,
                          StudentId = m.StudentId,
                          Date = m.Date,
                          Status = m.Status,
                          Cause = m.Cause,
                          ReservationTypeId = m.ReservationType.Id,
                          Name = m.ReservationType.Name,
                          Student = m.Student,
                          CreateDate = m.CreateDate,
                     }).FirstOrDefaultAsync(m=>m.Id == id);
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
            //var StatusMessage = new SelectList(Status.GetValues(typeof(Status)))
            //.OfType<Status>()
            //.Select(t => new SelectListItem{
            //     Text = t.ToString(),
            //     Value = t,
            //});
            
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
                    //if (User.Identity != null) return NotFound();
                    //var user =(Models.Student) await _userManager.GetUserAsync(User);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var type = _context.ReservationTypes.Single(t => t.Id == reservation.ReservationTypeId);

                    Reservation res = new Reservation
                    {
                         Date = reservation.Date,
                         Status = reservation.Status,
                         Cause = reservation.Cause,
                         StudentId = userId,
                         ReservationTypeId = type.Id,

                         // Student = await _context.Users.FirstAsync(s=>s.Id == userId),
                         // ReservationType = await  _context.ReservationTypes.FirstAsync(x=> x.Id == type.Id)
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
               //var res = new ReservationStudentViewModel
               //{
               //    Id = id,
               //    StudentId = reservation.StudentId,
               //    Cause = reservation.Cause,
               //    Date = reservation.Date,
               //    Name = reservation.ReservationType.Name,
               //    ReservationTypeId = reservation.ReservationTypeId,
               //    Status = reservation.Status
               //};


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
                        //var upRes = await _context.Reservations.SingleAsync(x=>x.Id == id);
                       
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

               var reservation = await _context.Reservations
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

          private bool ReservationExists(string id)
          {
               return _context.Reservations.Any(e => e.Id == id);
          }
     }
}
