namespace DeviceManagement.Models
{
    public class DashboardViewModel
    {
        public int TotalDevices { get; set; }
        public int DevicesInUse { get; set; }
        public int DevicesBroken { get; set; }
        public int DevicesUnderMaintenance { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public List<DeviceModel> RecentDevices { get; set; } = new();
    }
}
