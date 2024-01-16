namespace _2023_p3a_prg_maui_project_FrantisekSilhan.Pages;

public partial class SavedPage : ContentPage
{
	public SavedPage()
	{
		InitializeComponent();
		BindingContext = (Application.Current.MainPage! as AppShell)?.MVM;
	}
}