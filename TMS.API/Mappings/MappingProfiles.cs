using AutoMapper;
using TMS.API.DTOs;
using TMS.Core;

namespace TMS.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<ApplicationUser, PostUserDTO>()
               .ReverseMap();
        }
    }
}
