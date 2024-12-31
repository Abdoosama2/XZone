using System.ComponentModel.DataAnnotations;

namespace XZone.Models.DTO.DeviceDTOs
{
    public class DeviceCreateDTO
    {
        [MaxLength(100)]
        public string Name { get; set; }


        [MaxLength(500)]
        public string Icon { get; set; }
    }
}
