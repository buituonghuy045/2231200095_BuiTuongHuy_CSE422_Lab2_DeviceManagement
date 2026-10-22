using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Models
{
    public class CategoryModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public virtual ICollection<DeviceModel> Devices { get; set; } = new List<DeviceModel>();
    }
}
