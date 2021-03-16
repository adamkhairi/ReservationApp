using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Roles = "Admin")]
     public class AdminController : Controller
     {

          private readonly UserManager<IdentityUser> _userManager;

          public AdminController(UserManager<IdentityUser> userManager)
          {
               _userManager = userManager;
          }


          [Route("/Admin")]
          public IActionResult Dashboard()
          {
               return View("DashBoard");
          }


     }
}
