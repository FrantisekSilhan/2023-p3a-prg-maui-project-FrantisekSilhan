using _2023_p3a_prg_maui_project_FrantisekSilhan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan.ViewModels
{
	public class MainViewModel
	{
		private byte[] _raw;
		private string _value;

		public byte[] Raw
		{
			get { return _raw; }
			set { _raw = value; }
		}
		public string Value
		{
			get { return _value; }
			set { _value = value; OnPropertyChanged(); ExecuteActionCommand?.ChangeCanExecute(); Console.WriteLine("test " + Value); }
		}

		public Command ExecuteActionCommand { get; set; }
		private bool IsLink(string value)
		{
			return value.StartsWith("http://") || value.StartsWith("https://");
		}

		private bool IsWiFiQRCode(string value)
		{
			return value.StartsWith("WIFI:");
		}

		private void HandleWiFiQRCode(string value)
		{
			string wifiData = value.Substring("WIFI:".Length);

			var wifiParameters = wifiData.Split(";").Select(parameter =>
			{
				var parts = parameter.Split(":");
				return new { Key = parts[0], Value = parts.Length > 1 ? parts[1] : string.Empty };
			}).ToDictionary(item => item.Key, item => item.Value);

			if (wifiParameters.TryGetValue("T", out string securityType) &&
				wifiParameters.TryGetValue("S", out string ssid))
			{

			}
		}

		private QRCodeType GetQRCodeType(string value)
		{
			if (IsLink(value))
			{
				return QRCodeType.Link;
			} else if (IsWiFiQRCode(value))
			{
				return QRCodeType.WiFi;
			}

			return default(QRCodeType);
		}

		private void ExecuteAction(object parameter)
		{
			switch (GetQRCodeType(Value))
			{
				case QRCodeType.Link:
					System.Diagnostics.Debug.WriteLine(string.Format("Opening link: {0}", Value));
					Launcher.OpenAsync(Value);
					break;
				case QRCodeType.WiFi:
					System.Diagnostics.Debug.WriteLine(string.Format("Handling WiFi: {0}", Value));
					HandleWiFiQRCode(Value);
					break;
				default:
					System.Diagnostics.Debug.WriteLine(string.Format("Unknown QR Code type: {0}", Value));
					break;
			}
		}
		public MainViewModel()
		{
			Value = "test";
			ExecuteActionCommand = new Command(ExecuteAction, (x) =>
			{
				return GetQRCodeType(Value) != QRCodeType.Text;
			});
		}

		#region MVVM
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion //MVVM
	}
}
