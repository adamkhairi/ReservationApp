using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace ReservationApp.Models
{
    public class ReservationType
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int AccessNumber { get; set; }
    }
}
