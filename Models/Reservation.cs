using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Models
{
     public class Reservation
     {
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          [Key, Column(Order = 0)]
          public string Id { get; set; }

          [DisplayName("Reservation Date")]
          [DataType(DataType.Date)]
          [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
          public DateTime Date { get; set; }
          public string Status { get; set; }

          [AllowNull]
          public string Cause { get; set; }
          public Student Student { get; set; }

          [ForeignKey("StudentId,ReservationTypeId")]
          public string StudentId { get; set; }
          public string ReservationTypeId { get; set; }
          //public Student Student { get; set; }
          //   public string TypeId { get; set; }

          [DisplayName("Type")]
          public ReservationType ReservationType { get; set; }

          [DisplayName("Creation Date")]
          [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
          public DateTime CreateDate { get; set; }

          public Reservation()
          {
               var dateNow = DateTime.UtcNow;
               //CreateDate = TimeZoneInfo.ConvertTimeFromUtc(dateNow,TimeZoneInfo.Local);
               this.CreateDate = dateNow.ToLocalTime().Date;
               this.Status = Models.Status.Pending.ToString();
          }
     }
}

