using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Models
{
    public class Student : IdentityUser
    {
        public string FullName { get; set; }
        public string Class { get; set; }
        public int ReservationCount { get; set; }
    }
}
