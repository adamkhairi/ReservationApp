using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationApp.Models;

namespace ReservationApp.Help
{
     public static class Helpers
     {

          public static List<SelectListItem> StatusList()
          {
               var enumData = from Status e in Enum.GetValues(typeof(Status))
                              select new
                              {
                                   ID = (int)e,
                                   Name = e.ToString()
                              };

               var itemsList = new List<SelectListItem>();
               itemsList.AddRange(Enum.GetValues(typeof(Status)).Cast<Status>().Select(
                   (item, index) => new SelectListItem
                   {
                        Text = item.ToString(),
                        Value = item.ToString(),
                   }).ToList());

               var statusMessage = new SelectList(enumData, "Id", "Name");
               return itemsList;
          }
     }
}
