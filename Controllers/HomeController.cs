using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Data;

namespace ReservationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

          public IActionResult Index()
          {
               //var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
               //var userRes = _context.Reservations.Include(r => r.ReservationType == _context.ReservationTypes.Include(
               //    rT => rT.Id == r.Id

               //)).AsAsyncEnumerable();

               //var x =  await _userManager.GetUsersInRoleAsync("Student");
               //ViewBag.Students = x;
               return View();
          }

          public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
