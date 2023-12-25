using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API {
	public class TalonAlphabetFunction {
		private readonly ILogger _logger;
		public TalonAlphabetFunction(ILoggerFactory loggerFactory) {
			_logger = loggerFactory.CreateLogger<TalonAlphabetFunction>();
		}

		[Function("TalonAlphabet")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req) {
			//var result= Http.GetFromJsonAsync<TalonAlphabet[]>("sample-data/alphabet.json");
			string? basedDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);
			string jsonString = string.Empty;
			if (basedDirectory != null) {
				string filePath = Path.Combine(basedDirectory, "App_Data", "alphabet.json");
				jsonString = File.ReadAllText(filePath);
			}
			TalonAlphabet? alphabetWords = JsonSerializer.Deserialize<TalonAlphabet>(jsonString);
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
			if (jsonString?.Length > 0) {
			}
			if (alphabetWords != null) {
			}
			response.WriteAsJsonAsync(alphabetWords);
			return response;
		}

		private string GetSummary(int temp) {
			var summary = "Mild";
			if (temp >= 32) {
				summary = "Hot";
			} else if (temp <= 16 && temp > 0) {
				summary = "Cold";
			} else if (temp <= 0) {
				summary = "Freezing";
			}
			return summary;
		}

		public class TalonAlphabet {
			[JsonPropertyName("alphabetWord")]
			public string AlphabetWord { get; set; } = "Act";
			[JsonPropertyName("pictureUrl")]
			public string PictureUrl { get; set; } = "https://somedomain.com/images/act.png";
		}
	}
}
