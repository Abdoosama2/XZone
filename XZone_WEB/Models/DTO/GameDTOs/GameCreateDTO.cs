using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XZone_WEB.Models.DTO.GameDTOs
{
    public class GameCreateDTO
    {

        public string Name { get; set; }

       
        [MaxLength(300)]
        public IFormFile ImageURL { get; set; }
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        [Display(Name = "Supported Devices")]
        public List<int> SelectedDevices { get; set; } = new List<int>();


        
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> devices { get; set; } = Enumerable.Empty<SelectListItem>();

        [MaxLength(500)]
        public string Description { get; set; }


    }
}
