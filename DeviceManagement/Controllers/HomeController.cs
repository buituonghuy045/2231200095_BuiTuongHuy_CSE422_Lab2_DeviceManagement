using System.Diagnostics;
using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeviceDbContext _context;

        public HomeController(DeviceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalDevices = await _context.Devices.CountAsync(),
                DevicesInUse = await _context.Devices.CountAsync(d => d.Status == DeviceStatus.InUse),
                DevicesBroken = await _context.Devices.CountAsync(d => d.Status == DeviceStatus.Broken),
                DevicesUnderMaintenance = await _context.Devices.CountAsync(d => d.Status == DeviceStatus.UnderMaintenance),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                RecentDevices = await _context.Devices
                    .Include(d => d.Category)
                    .Include(d => d.User)
                    .OrderByDescending(d => d.DateOfEntry)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
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
