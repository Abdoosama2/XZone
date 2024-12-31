using AutoMapper;
using XZone.Models;
using XZone.Models.DTO;
using XZone.Models.DTO.DeviceDTOs;
using XZone.Models.DTO.GameDTOs;
using XZone.Models.DTO.UserDto_s;

namespace XZone
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryUpdatedDTO, Category>().ReverseMap();
           
            CreateMap<Device,DeviceDTO>().ReverseMap();
            CreateMap<Device, DeviceCreateDTO>().ReverseMap();
            CreateMap<Device, DeviceUpdatedDTO>().ReverseMap();
            CreateMap<Game, GameDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Game, GameUpdateDTO>().ReverseMap();
            CreateMap<Game, GameCreateDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserRegistrationDTO>().ReverseMap();

            //CreateMap<GameDevice,Game>
        }
    }
}
