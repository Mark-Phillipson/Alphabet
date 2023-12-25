using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client.Pages {
	public partial class Alphabet {
		private TalonAlphabet[]? alphabetItems;
 public  string? Message { get; set; }
		protected override async Task OnInitializedAsync() {
			try {
				alphabetItems = await Http.GetFromJsonAsync<TalonAlphabet[]>("sample-data/alphabet.json");
			  //alphabetItems= await Http.GetFromJsonAsync<TalonAlphabet[]>("/api/TalonAlphabet") ?? new TalonAlphabet[] { };
			} catch (Exception exception) {
				Message = exception.Message; 
				Console.WriteLine(exception.Message);
			}
			// try
			// {
			//    forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("/api/WeatherForecast") ?? new WeatherForecast[] { };
			// }
			// catch (Exception ex)
			// {
			//    Console.WriteLine(ex.Message);
			// }
		}

		public class TalonAlphabet {
			[JsonPropertyName("alphabetWord")]
			public string AlphabetWord { get; set; } = "Act";
			[JsonPropertyName("pictureUrl")]
			public string PictureUrl { get; set; } = "https://somedomain.com/images/act.png";
		}
	}
}