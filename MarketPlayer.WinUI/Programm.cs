using System;
using System.Windows;
using MarketPlayer.WinUI.EngineImplem;
using MarketPlayer.WinUI.ViewModel;

namespace MarketPlayer.WinUI
{
	public static class Programm
	{
		[System.STAThread]
		public static void Main()
		{
			var fileDialogService = new UIService();
			var vm = new MainWindowVM(fileDialogService);
			StartWindowsEngine(vm);
		}

		public static void StartWindowsEngine(MainWindowVM vm)
		{
			var app = new Application();
			ResourceDictionary myResourceDictionary = new ResourceDictionary();
			myResourceDictionary.Source = new Uri("Styles/ResourceDictionary.xaml", UriKind.Relative);
			app.Resources.MergedDictionaries.Add(myResourceDictionary);
			app.Startup += (s, e) =>
			{
				var mw = new MainWindow();
				mw.DataContext = vm;
				mw.Show();
			};
			app.Run();
		}
	}
}