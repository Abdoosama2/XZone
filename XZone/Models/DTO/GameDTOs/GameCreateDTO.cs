using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XZone.Models.DTO.GameDTOs
{
    public class GameCreateDTO
    {

        public string Name { get; set; }

        public string ImagePath { get; set; }

        [MaxLength(300)]
        public IFormFile ImageURL { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public List<int> SelectedDevices { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> devices { get; set; } = Enumerable.Empty<SelectListItem>();

        [MaxLength(500)]
        public string Description { get; set; }

    }
}
