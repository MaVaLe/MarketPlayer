using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using MarketPlayer.Core.EngineInterfaces;
using MarketPlayer.Core.Model;
using MarketPlayer.Core.ViewModel;

namespace MarketPlayer.WinUI.ViewModel
{
	public class MainWindowVM : INotifyPropertyChanged
	{
		private PlayProcessor _playProcessor;
		private IUIService _uiService;

		public MainWindowVM(IUIService uIService)
		{
			MediaEnded = new RoutedCommand(ProcessMediaEnded);
			_playProcessor = new PlayProcessor();
			_playProcessor.CurrentVideoSourceChanged += PlayProcessorCurrentVideoSourceChanged;
			_uiService = uIService;
			LoadSchedule = new SelectFileCommand(uIService, ProcessLoadScheduleFile);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string FileName { get; private set; }
		public ICommand LoadSchedule { get; private set; }
		public Uri MediaElementSource { get; private set; }
		public ICommand MediaEnded { get; private set; }
		private void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private void PlayProcessorCurrentVideoSourceChanged()
		{
			if (string.IsNullOrEmpty(_playProcessor.CurrentVideoSource))
			{
				MediaElementSource = null;
				FileName = null;
			}
			else
			{
				MediaElementSource = new Uri(_playProcessor.CurrentVideoSource, UriKind.RelativeOrAbsolute);
				FileName = Path.GetFileName(_playProcessor.CurrentVideoSource);
			}

			NotifyPropertyChanged("MediaElementSource");
			NotifyPropertyChanged("FileName");
		}
		private void ProcessLoadScheduleFile(string fName)
		{
			if (string.IsNullOrEmpty(fName))
				return;
			var scheduleStrs = File.ReadAllLines(fName);
			var result = ScheduleBuilder.Build(scheduleStrs);
			if (result.BuildResult == null)
			{
				_uiService.ShowMsgForUser(result.BuildError);
				return;
			}
			_playProcessor.Schedule = result.BuildResult;
		}
		private void ProcessMediaEnded()
		{
			_playProcessor.ToNextVideo();
		}
	}
}