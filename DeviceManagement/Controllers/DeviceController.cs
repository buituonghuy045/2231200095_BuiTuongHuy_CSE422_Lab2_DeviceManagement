using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Controllers
{
    public class DeviceController : Controller
    {
        private readonly DeviceDbContext _context;

        public DeviceController(DeviceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchTerm, string? categoryId, DeviceStatus? status)
        {
            var query = _context.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(d => d.Name.ToLower().Contains(term) || d.Code.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                query = query.Where(d => d.CategoryId == categoryId);
            }

            if (status.HasValue)
            {
                query = query.Where(d => d.Status == status.Value);
            }

            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            var statuses = Enum.GetValues(typeof(DeviceStatus))
                .Cast<DeviceStatus>()
                .Select(s => new { Id = (int)s, Name = s.ToString() })
                .ToList();
            ViewBag.Statuses = new SelectList(statuses, "Id", "Name", status.HasValue ? (int)status.Value : null);

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentStatus = status;

            var devices = await query.OrderBy(d => d.Name).ToListAsync();
            return View(devices);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Code == id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDownsAsync();
            return View(new DeviceModel { DateOfEntry = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceModel device)
        {

            if (await _context.Devices.AnyAsync(d => d.Code.ToLower() == device.Code.Trim().ToLower()))
            {
                ModelState.AddModelError("Code", $"Device code '{device.Code}' is already in use.");
            }

            if (ModelState.IsValid)
            {
                device.Code = device.Code.Trim();
                _context.Devices.Add(device);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Device '{device.Name}' ({device.Code}) was created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDownsAsync(device.CategoryId, device.UserId);
            return View(device);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            await PopulateDropDownsAsync(device.CategoryId, device.UserId);
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DeviceModel device)
        {
            if (id != device.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Device '{device.Name}' ({device.Code}) was updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DeviceExists(device.Code))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            await PopulateDropDownsAsync(device.CategoryId, device.UserId);
            return View(device);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Category)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Code == id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Device '{device.Name}' ({device.Code}) was deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDownsAsync(string? selectedCategory = null, string? selectedUser = null)
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", selectedCategory);

            var users = await _context.Users.OrderBy(u => u.FullName).ToListAsync();
            ViewBag.UserId = new SelectList(users, "Id", "FullName", selectedUser);
        }

        private async Task<bool> DeviceExists(string code)
        {
            return await _context.Devices.AnyAsync(e => e.Code == code);
        }
    }
}
