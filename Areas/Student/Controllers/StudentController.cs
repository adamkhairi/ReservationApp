using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Areas.Student.Controllers
{
     [Area("Student")]
     public class StudentController : Controller
     {
          private readonly UserManager<IdentityUser> _userManager;

          public StudentController(UserManager<IdentityUser> userManager)
          {
               _userManager = userManager;

          }


          [Route("/Student")]
          public IActionResult Student()
          {
               return View("Student");
          }

     }
}