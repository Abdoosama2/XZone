using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace XZone.Models.DTO.GameDTOs
{
    public class GameCreateDTO
    {

        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; } // Expecting the URL here
        public List<int> SelectedDevices { get; set; }

    }
}
