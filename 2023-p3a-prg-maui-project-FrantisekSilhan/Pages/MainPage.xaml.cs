using _2023_p3a_prg_maui_project_FrantisekSilhan.ViewModels;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		qrCodeReader.Options = new BarcodeReaderOptions
		{
			Formats = BarcodeFormats.All,
			AutoRotate = true
		};
		BindingContext = (Application.Current.MainPage! as AppShell)?.MVM;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		qrCodeReader.CameraLocation = CameraLocation.Front;
		qrCodeReader.CameraLocation = CameraLocation.Rear;
	}

	[Obsolete]
	private void qrCodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		BarcodeResult? result = e.Results?.FirstOrDefault();

		if (result == null)
		{
			return;
		}

		Device.BeginInvokeOnMainThread(() =>
		{
			MainViewModel? mainViewModel = BindingContext as MainViewModel;

			if (mainViewModel == null)
			{
				return;
			}

			mainViewModel.Raw = result.Raw;
			mainViewModel.Value = result.Value;
		});
	}
}