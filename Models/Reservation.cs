using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
     public class Reservation
     {
          [Key]
          public string Id { get; set; }
          public DateTime Date { get; set; }
          public string Status { get; set; }
          public string? Cause { get; set; }
          public Student Student { get; set; }
          //   public string TypeId { get; set; }
          public ReservationType ReservationType { get; set; }
          public DateTime CreateDate { get; set; }
     }
}

