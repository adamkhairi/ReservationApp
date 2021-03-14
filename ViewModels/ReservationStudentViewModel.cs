using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationApp.Models;

namespace ReservationApp.ViewModels
{
     public class ReservationStudentViewModel
     {
          //public Reservation Reservation { get; set; }
          //public ReservationType ReservationType { get; set; }
          //public Student Student { get; set; }

          public string Id { get; set; }
          public string StudentId { get; set; }
          //public string LastName { get; set; }
          //TODO:Fix This
          [DisplayName("Reservation Date")]
          [DataType(DataType.Date)]
          [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
          public DateTime Date { get; set; }
          public string Status { get; set; }
          public string Cause { get; set; }
          //public string SearchString { get; set; }
          public string ReservationTypeId { get; set; }


     }
}
