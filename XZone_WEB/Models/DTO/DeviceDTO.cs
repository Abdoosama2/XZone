using System.ComponentModel.DataAnnotations;

namespace XZone_WEB.Models.DTO
{
    public class DeviceDTO
    {
        public string Name { get; set; }


        [MaxLength(500)]
        public string Icon { get; set; }
    }
}
