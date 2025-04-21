using AutoMapper;
using Client.DTO;
using Client.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Prompt, PromptDTO>();
        CreateMap<PromptDTO, Prompt>();
    }
}
