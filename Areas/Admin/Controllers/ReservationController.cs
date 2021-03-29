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

          //GET Reservations of tomorrow
          public async Task<IActionResult> Index()
          {
               ViewBag.StatusList = Helpers.StatusList();
               var list = await _context.GetReservationsToApprove();
               //return AbsenceHistories;
               return View("List", list);
          }

          //GET All Reservations History
          public async Task<IActionResult> History()
          {
               ViewBag.StatusList = Helpers.StatusList();
               var list = await _context.GetAllReservations();
               //return AbsenceHistories;
               return View(list);
          }

          //GET All Reservations APPROVED RESERVATIONS BY DATE
          public async Task<IActionResult> History(DateTime date)
          {
               ViewBag.StatusList = Helpers.StatusList();
               var list = await _context.GetApprovedResByDate(date);
               //return AbsenceHistories;
               return View(list);
          }



          //Index Filter
          [HttpPost]
          public async Task<ActionResult> Index(DateTime filterDate)
          {
               if (filterDate.Year == 0001)
               {
                    return RedirectToAction(nameof(Index));
               }
               else
               {
                    ViewBag.StatusList = Helpers.StatusList();

                    var list = await _context.GetReservationsByDate(filterDate);
                    return View("List", list);
               }
          }


          //TODO: Add View To 
          public async Task<IActionResult> ResByStudent(string id)
          {
               if (id == null)
               {
                    return NotFound();
               }

               var user = await _userManager.GetUserAsync(User);
               if (user == null)
               {
                    return NotFound();
               }
               var list = await _context.SudentReservations(user.Id);

               if (list.Count == 0)
               {
                    return NotFound();
               }
               return View("List", list);
          }


          [HttpGet]
          public async Task<IActionResult> Details(string id)
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

          [HttpGet]
          public async Task<IActionResult> Edit(string id)
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

          [HttpPost]
          public async Task<IActionResult> Submit(string id, string status)
          {
               var reservation = await _context.ResByID(id);

               reservation.Status = status;


               if (status == Status.Approved.ToString())
               {
                    _context.Update(reservation);
                    var UId = reservation.Student.Id;
                    var student = await _context.GetStudentById(UId);

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
