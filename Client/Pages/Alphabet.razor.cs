using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Client.Pages {
	public partial class Alphabet {
		private List<TalonAlphabet>? alphabetItems;
		[Inject] public required ILocalStorageService LocalStorage { get; set; }
		[Inject] public required HttpClient HttpClient { get; set; }
		[Inject] public required IJSRuntime JSRuntime { get; set; }
		[Inject] public required IToastService ToastService { get; set; }
		[Parameter] public bool Shuffle { get; set; }
		public string? Message { get; set; }
		private async Task LoadData() {
			alphabetItems = await LocalStorage.GetItemAsync<List<TalonAlphabet>>("alphabet");
			if (alphabetItems == null || alphabetItems.Count == 0) {
				alphabetItems = await Http.GetFromJsonAsync<List<TalonAlphabet>>("sample-data/alphabet.json") ?? new List<TalonAlphabet>();
			}
			if (Shuffle) {
				alphabetItems = alphabetItems.OrderBy(a => Guid.NewGuid()).ToList();
			}
		}
		protected override async Task OnInitializedAsync() {
			try {
				await LoadData();
				//alphabetItems= await Http.GetFromJsonAsync<TalonAlphabet[]>("/api/TalonAlphabet") ?? new TalonAlphabet[] { };
			} catch (Exception exception) {
				Message = exception.Message;
				Console.WriteLine(exception.Message);
			}
		}
		private async Task SaveAlphabetAsync() {
			if (alphabetItems== null) { return; }
			await LocalStorage.SetItemAsync<List<TalonAlphabet>>("alphabet", alphabetItems);
			ToastService.ShowSuccess("The Talon Alphabet has been Saved Successfully!");
		}
      private void AddAlphabet() {
         if (alphabetItems == null) { return; }
         TalonAlphabet talonAlphabet = new TalonAlphabet { AlphabetWord = "Air", PictureUrl = "https://somedomain.com/images/act.png" };
         alphabetItems.Add(talonAlphabet);
      }
      private void DeleteTalonAlphabet(TalonAlphabet talonAlphabet) {
         if (alphabetItems == null) { return; }
         alphabetItems.Remove(talonAlphabet);
		}


		public class TalonAlphabet {
			 public Guid Id { get; set; }= Guid.NewGuid();
			[JsonPropertyName("alphabetWord")]
			public string AlphabetWord { get; set; } = "Act";
			[JsonPropertyName("pictureUrl")]
			public string PictureUrl { get; set; } = "https://somedomain.com/images/act.png";
		}
	}
}