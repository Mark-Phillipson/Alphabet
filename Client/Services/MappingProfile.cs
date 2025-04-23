using AutoMapper;
using Client.DTO;
using Client.Models;

namespace Client.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Prompt, PromptDTO>();
            CreateMap<PromptDTO, Prompt>();
        }
    }
}
