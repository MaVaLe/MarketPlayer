using System;
using System.Windows.Input;

namespace MarketPlayer.Core.ViewModel
{
	internal class RoutedCommand : ICommand
	{
		private Action _processCommand;

		public RoutedCommand(Action processCommand)
		{
			_processCommand = processCommand;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			_processCommand();
		}
	}
}