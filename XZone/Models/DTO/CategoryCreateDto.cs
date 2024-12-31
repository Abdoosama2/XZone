using System.ComponentModel.DataAnnotations;

namespace XZone.Models.DTO
{
    public class CategoryCreateDto
    {

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
