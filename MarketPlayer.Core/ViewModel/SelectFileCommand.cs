using System;
using System.Windows.Input;
using MarketPlayer.Core.EngineInterfaces;

namespace MarketPlayer.Core.ViewModel
{
	public class SelectFileCommand : ICommand
	{
		private IUIService _fileDialogService;

		private Action<string> _processAction;

		public SelectFileCommand(IUIService fileDialogService, Action<string> ProcessAction)
		{
			_fileDialogService = fileDialogService;
			_processAction = ProcessAction;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			var filename = _fileDialogService.GetFName();
			_processAction(filename);
		}
	}
}