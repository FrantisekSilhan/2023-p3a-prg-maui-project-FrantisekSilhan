using _2023_p3a_prg_maui_project_FrantisekSilhan.ViewModels;
using ZXing;
using ZXing.Net.Maui;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan;

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
		BindingContext = (Application.Current.MainPage! as AppShell).MVM;
	}

	private void qrCodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
	{
		BarcodeResult? result = e.Results?.FirstOrDefault();

		if (result == null)
		{
			return;
		}


		MainViewModel? mainViewModel = BindingContext as MainViewModel;

		if (mainViewModel == null)
		{
			return;
		}

		Device.BeginInvokeOnMainThread(() =>
		{
			mainViewModel.Raw = result.Raw;
			mainViewModel.Value = result.Value;

			label.BindingContext = null;
			label.BindingContext = mainViewModel;
		});
	}
}