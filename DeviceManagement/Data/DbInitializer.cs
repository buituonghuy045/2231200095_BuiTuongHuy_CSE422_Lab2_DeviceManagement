using DeviceManagement.Models;

namespace DeviceManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DeviceDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<CategoryModel>
                {
                    new CategoryModel { Id = "CAT-COMP", Name = "Computer", Description = "Laptops, Desktops, and Workstations" },
                    new CategoryModel { Id = "CAT-PRIN", Name = "Printer", Description = "Laser printers, Multifunction, and Color printers" },
                    new CategoryModel { Id = "CAT-PHON", Name = "Phone", Description = "Smartphones, VoIP, and Office Desk phones" },
                    new CategoryModel { Id = "CAT-NETW", Name = "Network Equipment", Description = "Routers, Switches, and Access Points" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            var usersToSeed = new List<UserModel>
            {
                new UserModel { Id = "USR-001", FullName = "Nguyen Van A", Email = "nguyenvana@company.com", PhoneNumber = "0901234567" },
                new UserModel { Id = "USR-002", FullName = "Tran Thi B", Email = "tranthib@company.com", PhoneNumber = "0912345678" },
                new UserModel { Id = "USR-003", FullName = "Le Van C", Email = "levanc@company.com", PhoneNumber = "0987654321" },
                new UserModel { Id = "USR-004", FullName = "Pham Minh Duc", Email = "duc.pham@company.com", PhoneNumber = "0934567890" },
                new UserModel { Id = "USR-005", FullName = "Hoang Thi Mai", Email = "mai.hoang@company.com", PhoneNumber = "0945678901" },
                new UserModel { Id = "USR-006", FullName = "Doan Van Nam", Email = "nam.doan@company.com", PhoneNumber = "0956789012" }
            };

            foreach (var user in usersToSeed)
            {
                if (!context.Users.Any(u => u.Id == user.Id || u.Email == user.Email))
                {
                    context.Users.Add(user);
                }
            }
            context.SaveChanges();

            var devicesToSeed = new List<DeviceModel>
            {
                new DeviceModel { Code = "DEV-COMP-01", Name = "Dell Latitude 5430", CategoryId = "CAT-COMP", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-6), UserId = "USR-001" },
                new DeviceModel { Code = "DEV-COMP-02", Name = "MacBook Pro M2 14-inch", CategoryId = "CAT-COMP", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-3), UserId = "USR-002" },
                new DeviceModel { Code = "DEV-PRIN-01", Name = "HP LaserJet Enterprise M507", CategoryId = "CAT-PRIN", Status = DeviceStatus.Broken, DateOfEntry = DateTime.Today.AddYears(-1), UserId = null },
                new DeviceModel { Code = "DEV-PHON-01", Name = "iPhone 15 128GB", CategoryId = "CAT-PHON", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-2), UserId = "USR-003" },
                new DeviceModel { Code = "DEV-NETW-01", Name = "Cisco Catalyst 2960-X Switch", CategoryId = "CAT-NETW", Status = DeviceStatus.UnderMaintenance, DateOfEntry = DateTime.Today.AddYears(-2), UserId = null },
                new DeviceModel { Code = "DEV-COMP-03", Name = "ThinkPad X1 Carbon Gen 11", CategoryId = "CAT-COMP", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-1), UserId = "USR-004" },
                new DeviceModel { Code = "DEV-PHON-02", Name = "Samsung Galaxy S24 Ultra", CategoryId = "CAT-PHON", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-4), UserId = "USR-005" },
                new DeviceModel { Code = "DEV-PRIN-02", Name = "Canon imageRUNNER 2625i", CategoryId = "CAT-PRIN", Status = DeviceStatus.UnderMaintenance, DateOfEntry = DateTime.Today.AddMonths(-7), UserId = null },
                new DeviceModel { Code = "DEV-COMP-04", Name = "Dell UltraSharp 27 4K Monitor", CategoryId = "CAT-COMP", Status = DeviceStatus.InUse, DateOfEntry = DateTime.Today.AddMonths(-5), UserId = "USR-006" }
            };

            foreach (var device in devicesToSeed)
            {
                if (!context.Devices.Any(d => d.Code == device.Code))
                {
                    context.Devices.Add(device);
                }
            }
            context.SaveChanges();
        }
    }
}
