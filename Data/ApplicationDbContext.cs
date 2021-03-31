using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReservationApp.Models;
using Microsoft.AspNetCore.Http;
using ReservationApp.ViewModels;
using ReservationApp.Help;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Data
{
     public class ApplicationDbContext : IdentityDbContext
     {
          private readonly UserManager<IdentityUser> _userManager;

          public virtual DbSet<Admin> Admins { get; set; }
          public virtual DbSet<Student> Students { get; set; }
          public virtual DbSet<Reservation> Reservations { get; set; }
          public virtual DbSet<ReservationType> ReservationTypes { get; set; }


          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
          {

          }


          // GET_ALL_RESERVATIONS
          public async Task<List<Reservation>> GetAllReservations()
          {
               var reservationData = await Reservations
                    .Include(x => x.ReservationType)
                    .Include(s => s.Student)
                    .OrderByDescending(r => r.Date)
                    .ToListAsync();
               return reservationData;
          }


          // GET_RESERVATIONS_To_Approve
          public async Task<List<Reservation>> GetReservationsToApprove()
          {
               //Get Next week Reservations List
               var reservationData = await Reservations
                    .Include(x => x.ReservationType)
                    .Include(s => s.Student)
                    .Where(r => r.Date >= Helpers.CurrentDay() && r.Date <= Helpers.CurrentDay().AddDays(7))
                    .Where(r => r.Status == Status.Pending.ToString())
                    .OrderByDescending(r => r.Date)
                    .ToListAsync();
               return reservationData;
          }

          // GET_ALL_RESERVATIONS
          public async Task<List<Reservation>> GetReservationsByStatusDate(string status, DateTime date)
          {
               var reservationData = await Reservations
                    .Include(r => r.ReservationType)
                    .Include(r => r.Student)
                    .Where(r => r.Status == status)
                    .Where(r => r.Date == date)
                    .OrderByDescending(r => r.Date)
                    .ToListAsync();
               return reservationData;
          }


          // GET_ALL_RESERVATIONS_BY_DATE
          public async Task<List<Reservation>> GetReservationsByDate(DateTime date)
          {
               var list = await Reservations
                                   .Include(t => t.ReservationType)
                                   .Include(s => s.Student)
                                   .OrderBy(r => r.Date)
                                   .Where(r => r.Date == date)
                                   .ToListAsync();
               return list;
          }


          // GET_STUDENT_RESERVATIONS (History)
          public async Task<List<ReservationStudentViewModel>> SudentReservations(string studentId)
          {

               var list = await Reservations.Select(
                    res => new ReservationStudentViewModel
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
                    .Where(res => res.StudentId == studentId)
                    .OrderByDescending(res => res.Date)
                    .ToListAsync();

               return list;
          }


          // GET_STUDENT_RESERVATIONS_BY_DATE
          public async Task<List<ReservationStudentViewModel>> SudentResOfDay(string studentId, DateTime date)
          {

               var list = await Reservations.Select(
                    res => new ReservationStudentViewModel
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
                    .Where(res => res.StudentId == studentId)
                    .Where(res => res.Date == date)
                    .ToListAsync();

               return list;
          }


          // GET_STUDENT_RESERVATIONS_BY_DATE
          public async Task<List<ReservationStudentViewModel>> SudentResOfCreateDate(string studentId, DateTime date)
          {

               var list = await Reservations.Select(
                    res => new ReservationStudentViewModel
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
                    .Where(res => res.StudentId == studentId)
                    .Where(res => res.CreateDate == date)
                    .ToListAsync();

               return list;
          }
          // GET_RESERVATION_BY_ID
          public async Task<Reservation> ResByID(string id)
          {
               var reservation = await Reservations
                                  .Include(r => r.Student)
                                  .Include(r => r.ReservationType)
                                  .FirstOrDefaultAsync(r => r.Id == id);
               return reservation;
          }


          // GET_RESERVATION_BY_ID_VIEW_MODEL
          public async Task<ReservationStudentViewModel> ResByIdViewModel(string id)
          {
               var reservation = await Reservations
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
               }).FirstOrDefaultAsync(m => m.Id == id);
               return reservation;
          }


          // GET_RESERVATION_TYPE_BY_ID
          public async Task<ReservationType> GetReservationTypeById(string id)
          {
               return await ReservationTypes.FirstOrDefaultAsync(m => m.Id == id);
          }


          //GET_STUDENT_BY_ID
          public async Task<Student> GetStudentById(string id)
          {
               return await Students.FirstOrDefaultAsync(s => s.Id == id);
          }


          //GET ALL USERS
          public async Task<List<IdentityUser>> GetAllUsers()
          {
               return await _userManager.Users.ToListAsync();
          }


          //GET_STUDENT_APPROVED_RESERVATION
          public async Task<List<ReservationStudentViewModel>> SudentApprovedRes(string studentId)
          {
               var list = await Reservations.Select(
                                res => new ReservationStudentViewModel
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
                                .Where(res => res.StudentId == studentId)
                                .Where(res => res.Status == Status.Approved.ToString())
                                .ToListAsync();
               return list;
          }


          //GET LIST OF APPROVED RESERVATIONS BY DATE
          public async Task<List<Reservation>> GetApprovedResByDate(DateTime Date)
          {

               var list = await Reservations
                              .Include(r => r.Student)
                              .Include(r => r.ReservationType)
                              .Where(r => r.Date == Date)
                              .Where(r => r.Status == Status.Approved.ToString())
                              .ToListAsync();
               return list;
          }

          //GET LIST OF Rejected RESERVATIONS BY DATE
          public async Task<List<Reservation>> GetRejectedResByDate(DateTime Date)
          {

               var list = await Reservations
                              .Include(r => r.Student)
                              .Include(r => r.ReservationType)
                              .Where(r => r.Date == Date)
                              .Where(r => r.Status == Status.Rejected.ToString())
                              .ToListAsync();
               return list;
          }


          //GET LIST OF This Week RESERVATIONS 
          public async Task<List<Reservation>> GetResOfThisWeek(DateTime Date)
          {

               var list = await Reservations
                              .Include(r => r.Student)
                              .Include(r => r.ReservationType)
                              .Where(r => r.Date.DayOfWeek == DayOfWeek.Saturday && r.Date.DayOfWeek == DayOfWeek.Sunday)
                              .Where(r => r.Status == Status.Rejected.ToString())
                              .ToListAsync();
               return list;
          }

     }
}
