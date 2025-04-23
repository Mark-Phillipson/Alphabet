using AutoMapper;
using Client.DTO;
using Client.Models;
using Client.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class MappingProfileTests
{
    [Fact]
    public void MappingProfile_ConfigurationIsValid()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<MappingProfile>>();
        var mappingProfile = new MappingProfile(loggerMock.Object);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void MappingProfile_CanMapPromptToPromptDTO()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<MappingProfile>>();
        var mappingProfile = new MappingProfile(loggerMock.Object);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        var mapper = configuration.CreateMapper();

        var prompt = new Prompt
        {
            Id = 1,
            PromptText = "Test Prompt",
            Description = "Test Description",
            IsDefault = true
        };

        // Act
        var promptDTO = mapper.Map<PromptDTO>(prompt);

        // Assert
        Assert.Equal(prompt.Id, promptDTO.Id);
        Assert.Equal(prompt.PromptText, promptDTO.PromptText);
        Assert.Equal(prompt.Description, promptDTO.Description);
        Assert.Equal(prompt.IsDefault, promptDTO.IsDefault);
    }
}