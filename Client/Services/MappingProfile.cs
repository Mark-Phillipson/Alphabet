using AutoMapper;
using Client.DTO;
using Client.Models;
using Microsoft.Extensions.Logging;

namespace Client.Services
{
    public class MappingProfile : Profile
    {
        private readonly ILogger<MappingProfile> _logger;

        public MappingProfile(ILogger<MappingProfile> logger)
        {
            _logger = logger;
            _logger.LogInformation("Initializing MappingProfile");

            try
            {
                CreateMap<Prompt, PromptDTO>();
                _logger.LogInformation("Mapping created: Prompt -> PromptDTO");

                CreateMap<PromptDTO, Prompt>();
                _logger.LogInformation("Mapping created: PromptDTO -> Prompt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while configuring mappings in MappingProfile");
                throw;
            }
        }
    }
}
