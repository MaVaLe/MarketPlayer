using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MarketPlayer.WinUI.Styles
{
	public partial class WindowStyle
	{
		private static Window FindWindow(object obj)
		{
			return ((FrameworkElement)obj).TemplatedParent as Window;
		}
		private void CloseButtonClick(object sender, RoutedEventArgs e)
		{
			var w = FindWindow(sender);
			if (w != null)
				SystemCommands.CloseWindow(w);
		}
		private void CollapseButtonClick(object sender, RoutedEventArgs e)
		{
			var w = FindWindow(sender);
			if (w != null)
				SystemCommands.MinimizeWindow(w);
		}
		private void ExpandButtonClick(object sender, RoutedEventArgs e)
		{
			var w = FindWindow(sender);
			if (w != null)
			{
				if (w.WindowState == WindowState.Maximized)
					SystemCommands.RestoreWindow(w);
				else
					SystemCommands.MaximizeWindow(w);
			}
		}
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			var win = sender as Window;
			win.StateChanged += WindowStateChanged;
		}
		private void WindowStateChanged(object sender, EventArgs e)
		{
			var w = sender as Window;
			var handle = new WindowInteropHelper(w).Handle;
			var containerBorder = (Border)w.Template.FindName("FantomBorder", w);

			if (w.WindowState == WindowState.Maximized)
			{
				var screen = Screen.FromHandle(handle);
				if (screen.Primary)
				{
					containerBorder.Padding = new Thickness(
						SystemParameters.WorkArea.Left + 7,
						SystemParameters.WorkArea.Top + 7,
						(SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right) + 7,
						(SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) + 7);
				}
			}
			else
			{
				containerBorder.Padding = new Thickness(7, 7, 7, 7);
			}
		}
	}
}