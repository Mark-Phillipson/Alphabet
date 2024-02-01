using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Client.Pages
{
	public partial class Alphabet
	{
		private List<TalonAlphabet>? alphabetItems;
		[Inject] public required ILocalStorageService LocalStorage { get; set; }
		[Inject] public required HttpClient HttpClient { get; set; }
		[Inject] public required IJSRuntime JSRuntime { get; set; }
		[Inject] public required IToastService ToastService { get; set; }
		[Parameter] public bool Shuffle { get; set; }
		[Parameter] public bool Guess { get; set; }
		private TalonAlphabet? _alphabet;
		private int _index = 0;
		private bool _show = true;
		private string _guess = string.Empty;
		private string _message = "Enter Single Letter";
		public string? Message { get; set; }
		private async Task LoadData()
		{
			alphabetItems = await LocalStorage.GetItemAsync<List<TalonAlphabet>>("alphabet");
			if (alphabetItems == null || alphabetItems.Count == 0)
			{
				alphabetItems = await Http.GetFromJsonAsync<List<TalonAlphabet>>("sample-data/alphabet.json") ?? new List<TalonAlphabet>();
			}
			if (Shuffle)
			{
				alphabetItems = alphabetItems.OrderBy(a => Guid.NewGuid()).ToList();
			}
			if (Guess)
			{
				alphabetItems = alphabetItems.OrderBy(a => Guid.NewGuid()).ToList();
				_alphabet = alphabetItems[_index];
			}
		}
		protected override async Task OnInitializedAsync()
		{
			try
			{
				await LoadData();
				//alphabetItems= await Http.GetFromJsonAsync<TalonAlphabet[]>("/api/TalonAlphabet") ?? new TalonAlphabet[] { };
			}
			catch (Exception exception)
			{
				Message = exception.Message;
				Console.WriteLine(exception.Message);
			}
		}
		private async Task SaveAlphabetAsync()
		{
			if (alphabetItems == null) { return; }
			await LocalStorage.SetItemAsync<List<TalonAlphabet>>("alphabet", alphabetItems);
			ToastService.ShowSuccess("The Talon Alphabet has been Saved Successfully!");
		}
		private void AddAlphabet()
		{
			if (alphabetItems == null) { return; }
			TalonAlphabet talonAlphabet = new TalonAlphabet { AlphabetWord = "Air", PictureUrl = "https://somedomain.com/images/act.png" };
			alphabetItems.Add(talonAlphabet);
		}
		private void DeleteTalonAlphabet(TalonAlphabet talonAlphabet)
		{
			if (alphabetItems == null) { return; }
			alphabetItems.Remove(talonAlphabet);
		}
		public class TalonAlphabet
		{
			public Guid Id { get; set; } = Guid.NewGuid();
			[JsonPropertyName("alphabetWord")]
			public string AlphabetWord { get; set; } = "Act";
			[JsonPropertyName("pictureUrl")]
			public string PictureUrl { get; set; } = "https://somedomain.com/images/act.png";
		}
		private void GetNextWord()
		{
			if (alphabetItems == null) { return; }
			_index++;
			if (_index >= alphabetItems.Count)
			{
				_index = 0;
			}
			_alphabet = alphabetItems[_index];
		}
		private void CheckGuess(ChangeEventArgs e)
		{
			if (e == null || e.Value == null) { return; }
			_show = false; _message = "Enter Single Letter";
			string? value = _alphabet?.AlphabetWord.Substring(0, 1).ToLower();
			value = CheckValue(value, _alphabet?.AlphabetWord);
			if (e.Value?.ToString()?.ToLower() == value)
			{
				_message = $"Correct it was {_alphabet?.AlphabetWord}!";
				Thread.Sleep(400);
				GetNextWord();
			}
			else if (e.Value?.ToString()?.Length > 0)
			{
				_message = "Try Again!";
				_guess = string.Empty;
			}
			else
			{
				_message = "Enter Single Letter";
			}
			_show = true;
		}

		private string? CheckValue(string? value, string? alphabetWord)
		{
			if (value == null) { return null; }
			if (alphabetWord == null) { return null; }
			if (alphabetWord?.ToLower() == "sit") { return "i"; }
			if (alphabetWord?.ToLower() == "plex") { return "x"; }
			return value;
		}
	}
}