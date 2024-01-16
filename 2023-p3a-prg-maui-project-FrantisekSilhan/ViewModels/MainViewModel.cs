using _2023_p3a_prg_maui_project_FrantisekSilhan.Models;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private string _copyButtonText;
		private string _saveButtonText;
		private byte[] _raw;
		private string _value;
		private string _path = Path.Combine(FileSystem.AppDataDirectory, "data.json");
		private List<Data> _savedData;
		private List<Data> _filteredData;

		public List<Data> FilteredData
		{
			get { return _filteredData; }
			set
			{
				_filteredData = value;
				OnPropertyChanged();
			}
		}

		public byte[] Raw
		{
			get { return _raw; }
			set { _raw = value; }
		}
		public string Value
		{
			get { return _value; }
			set {
				_value = value;
				OnPropertyChanged();
				OpenInBrowserCommand?.ChangeCanExecute();
			}
		}

		public string CopyButtonText
		{
			get { return _copyButtonText; }
			set
			{
				if (_copyButtonText != value)
				{
					_copyButtonText = value;
					OnPropertyChanged();
				}
			}
		}
		public string SaveButtonText
		{
			get { return _saveButtonText; }
			set
			{
				if (_saveButtonText != value)
				{
					_saveButtonText = value;
					OnPropertyChanged();
				}
			}
		}

		public List<Data> SavedData
		{
			get { return _savedData; }
			set
			{
				_savedData = value;
				OnPropertyChanged();
			}
		}

		public Command OpenInBrowserCommand { get; set; }
		public Command CopyCommand { get; set; }
		public Command DeleteCommand { get; set; }
		public Command SaveCommand { get; set; }
		private void OpenInBrowser(object param)
		{
			try
			{
				Launcher.OpenAsync(Value);
			} catch
			{

			}
		}

		private async void Copy(object param)
		{
			string valueToCopy = param as string;
			if (string.IsNullOrEmpty(valueToCopy))
			{
				await Clipboard.SetTextAsync(Value);
			} else
			{
				await Clipboard.SetTextAsync(valueToCopy);
			}

			CopyButtonText = "Copied";

			await Task.Delay(1000);
			CopyButtonText = "Copy";
		}

		private async void Save(object param)
		{
			SavedData.Add(new Data { Id = SavedData.Count, Value = Value });

			await File.WriteAllTextAsync(_path, JsonConvert.SerializeObject(SavedData));
			
			SaveButtonText = "Saved";

			await Task.Delay(1000);
			SaveButtonText = "Save";

			FilteredData = SavedData.Where(d => !d.Deleted).ToList();
		}
		private async void Delete(object param)
		{
			if (param is Data data)
			{
				var test = SavedData.FirstOrDefault(x => x.Id == data.Id);
				if (test == null)
				{
					return;
				}
				test.Deleted = true;
				await File.WriteAllTextAsync(_path, JsonConvert.SerializeObject(SavedData));
			}

			FilteredData = SavedData.Where(d => !d.Deleted).ToList();
			OnPropertyChanged(nameof(SavedData));
			OnPropertyChanged();
		}

		public static readonly HashSet<string> ValidSchemes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"http://", "https://", "ftp://", "file://", "mailto:", "tel:", "data:", "chrome://", "edge://",
			"firefox://", "safari://", "android-app://", "ios-app://", "whatsapp://", "slack://", "zoommtg://",
			"spotify://", "microsoft-edge://", "microsoft-edge-holographic://", "ms-word://", "ms-excel://", "ms-powerpoint://", "ms-outlook://"
		};

		public bool IsValidBrowserLink(string link)
		{
			foreach (string validScheme in ValidSchemes)
			{
				if (link.StartsWith(validScheme, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}
		public MainViewModel()
		{
			Value = string.Empty;
			CopyButtonText = "Copy";
			SaveButtonText = "Save";
			try
			{
				if (!File.Exists(_path))
				{
					SavedData = new List<Data>();
					File.WriteAllText(_path, JsonConvert.SerializeObject(SavedData));
				}
				string jsonData = File.ReadAllText(_path);
				SavedData = JsonConvert.DeserializeObject<List<Data>>(jsonData);
				FilteredData = SavedData.Where(d => !d.Deleted).ToList();

			} catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			OpenInBrowserCommand = new Command(OpenInBrowser, (x) =>
			{
				return IsValidBrowserLink(Value);
			});
			CopyCommand = new Command(Copy);
			SaveCommand = new Command(Save);
			DeleteCommand = new Command(Delete);
		}

		#region MVVM
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion //MVVM
	}
}
