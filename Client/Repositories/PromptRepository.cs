using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Client.DTO;
using Client.Models;
using Microsoft.Extensions.Options;

namespace Client.Repositories
{
    public class PromptRepository : IPromptRepository
    {
        private readonly string _jsonFilePath;
        private readonly IMapper _mapper;

        public PromptRepository(IOptions<JsonRepositoryOptions> options, IMapper mapper)
        {
            _jsonFilePath = options.Value.JsonFilePath;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PromptDTO>> GetAllPromptsAsync(int maxRows = 400)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            return _mapper.Map<IEnumerable<PromptDTO>>(prompts.Take(maxRows));
        }

        public async Task<IEnumerable<PromptDTO>> SearchPromptsAsync(string serverSearchTerm)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            var filteredPrompts = prompts.Where(p => p.PromptText.Contains(serverSearchTerm, StringComparison.OrdinalIgnoreCase));
            return _mapper.Map<IEnumerable<PromptDTO>>(filteredPrompts);
        }

        public async Task<PromptDTO?> GetPromptByIdAsync(int id)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            var prompt = prompts.FirstOrDefault(p => p.Id == id);
            return prompt == null ? null : _mapper.Map<PromptDTO>(prompt);
        }

        public async Task<PromptDTO?> AddPromptAsync(PromptDTO promptDTO)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            var prompt = _mapper.Map<Prompt>(promptDTO);
            prompt.Id = prompts.Any() ? prompts.Max(p => p.Id) + 1 : 1;
            prompts.Add(prompt);
            await File.WriteAllTextAsync(_jsonFilePath, JsonSerializer.Serialize(prompts));
            return _mapper.Map<PromptDTO>(prompt);
        }

        public async Task<PromptDTO?> UpdatePromptAsync(PromptDTO promptDTO)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            var prompt = prompts.FirstOrDefault(p => p.Id == promptDTO.Id);
            if (prompt == null) return null;

            _mapper.Map(promptDTO, prompt);
            await File.WriteAllTextAsync(_jsonFilePath, JsonSerializer.Serialize(prompts));
            return _mapper.Map<PromptDTO>(prompt);
        }

        public async Task DeletePromptAsync(int id)
        {
            var jsonData = await File.ReadAllTextAsync(_jsonFilePath);
            var prompts = JsonSerializer.Deserialize<List<Prompt>>(jsonData) ?? new List<Prompt>();
            var prompt = prompts.FirstOrDefault(p => p.Id == id);
            if (prompt == null) return;

            prompts.Remove(prompt);
            await File.WriteAllTextAsync(_jsonFilePath, JsonSerializer.Serialize(prompts));
        }
    }
public class JsonRepositoryOptions
{
    public required string JsonFilePath { get; set; }
}


}