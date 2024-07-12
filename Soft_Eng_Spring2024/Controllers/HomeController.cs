using Microsoft.AspNetCore.Mvc;
using Soft_Eng_Spring2024.Data;
using Soft_Eng_Spring2024.Models;
using System.Diagnostics;

namespace Soft_Eng_Spring2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            var viewModel = new PollAnnouncement
            {
                Poll = _context.Poll
                                      .OrderByDescending(p => p.Id)
                                      .FirstOrDefault(),

                Announcement = _context.Announcement
                                              .OrderByDescending(a => a.Id)
                                              .Take(2)
                                              .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Services()
        {
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

        public IActionResult Study() 
        {
            return View();
        }
    }
}
