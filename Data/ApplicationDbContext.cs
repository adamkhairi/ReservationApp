using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReservationApp.Models;
using Microsoft.AspNetCore.Http;

namespace ReservationApp.Data
{
     public class ApplicationDbContext : IdentityDbContext
     {
          public virtual DbSet<Admin> Admins { get; set; }
          public virtual DbSet<Student> Students { get; set; }
          public virtual DbSet<Reservation> Reservations { get; set; }
          public virtual DbSet<ReservationType> ReservationTypes { get; set; }

          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
          {

          }

          public Object GetAllReservations()
          {
               //var reservationData =Reservations
               //    .Join(
               //        Students,
               //        reservations => reservations.Student.Id,
               //        students => students.Id,
               //        (reservations, students) => new
               //        {
               //            Email = students.Email,
               //            date = reservations.Date,
               //            cause = reservations.Cause,
               //            status = reservations.Status
               //        }
               //    )
               //    .ToList();

               var reservationData = Reservations.ToList();
               return reservationData;
          }
     }
}
