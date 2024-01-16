using _2023_p3a_prg_maui_project_FrantisekSilhan.ViewModels;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan
{
	public partial class AppShell : Shell
	{
		public MainViewModel MVM { get; set; } = new MainViewModel();
		public AppShell()
		{
			InitializeComponent();
			//Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		}
	}
}
