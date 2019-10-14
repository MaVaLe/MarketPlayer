using System.Windows;
using MarketPlayer.Core.EngineInterfaces;
using Microsoft.Win32;

namespace MarketPlayer.WinUI.EngineImplem
{
	internal class UIService : IUIService
	{
		public string GetFName()
		{
			var dialog = new OpenFileDialog
			{
				Filter = "Text documents (*.txt)|*.txt"
			};
			if (dialog.ShowDialog() == true)
			{
				return dialog.FileName;
			}
			return string.Empty;
		}

		public void ShowMsgForUser(string msg)
		{
			MessageBox.Show(msg, string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}