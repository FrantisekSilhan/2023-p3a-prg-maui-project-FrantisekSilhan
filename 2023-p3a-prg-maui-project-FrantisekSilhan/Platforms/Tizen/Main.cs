using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace _2023_p3a_prg_maui_project_FrantisekSilhan
{
	internal class Program : MauiApplication
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

		static void Main(string[] args)
		{
			var app = new Program();
			app.Run(args);
		}
	}
}
