using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace XZone.Models.DTO.GameDTOs
{
    public class GameCreateDTO
    {

        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(300)]
        public string ImageURL { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

    }
}
