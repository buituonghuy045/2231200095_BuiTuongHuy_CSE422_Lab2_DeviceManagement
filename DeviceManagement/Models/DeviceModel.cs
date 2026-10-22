using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManagement.Models
{
    public class DeviceModel
    {
        [Key]
        [Required(ErrorMessage = "Device code is required.")]
        [StringLength(50, ErrorMessage = "Device code cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Device code can only contain letters, numbers, and hyphens.")]
        [Display(Name = "Device Code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Device name must be between 2 and 150 characters.")]
        [Display(Name = "Device Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a device category.")]
        [Display(Name = "Device Category")]
        public string CategoryId { get; set; } = string.Empty;

        [ForeignKey(nameof(CategoryId))]
        public virtual CategoryModel? Category { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(DeviceStatus))]
        [Display(Name = "Status")]
        public DeviceStatus Status { get; set; } = DeviceStatus.InUse;

        [Required(ErrorMessage = "Date of entry is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Entry")]
        public DateTime DateOfEntry { get; set; } = DateTime.Today;

        [Display(Name = "Assigned User")]
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserModel? User { get; set; }
    }
}
