using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace ReservationApp.Models
{
     public class ReservationType
     {
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          [Key, Column(Order = 0)]
          public string Id { get; set; }
          [DisplayName("Reservation Type")]
          public string Name { get; set; }
          public int AccessNumber { get; set; }
     }
}
