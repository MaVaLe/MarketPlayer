using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketPlayer.Core.Model
{
	internal class PlayProcessor : IDisposable
	{
		internal ScheduleTask _currentScheduleTask;
		internal Schedule _schedule;
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private string _currentVideoSource;

		//Инициализировано, для отсутвия реализации проверки на null при вызове.
		internal event Action CurrentVideoSourceChanged = () => { };

		internal string CurrentVideoSource
		{
			get { return _currentVideoSource; }
			set
			{
				_currentVideoSource = value;
				CurrentVideoSourceChanged();
			}
		}
		internal Schedule Schedule
		{
			set
			{
				_schedule = value;
				ScheduleChange();
			}
		}

		public void Dispose()
		{
			_cancellationTokenSource.Dispose();
		}
		internal void ToNextVideo()
		{
			if (_currentScheduleTask != null)
			{
				var time = DateTime.Now.TimeOfDay;
				//Если текущая задача из расписания ещё актуальна.
				if (_currentScheduleTask.Intersection(time))
				{
					//Просто перещёлкиваемна следющее видео
					CurrentVideoSource = _currentScheduleTask.GetNextFName();
					return;
				}
				//Т.к. задача из расписания уже истекла, то просто перещёлкивает её на старт (для следующего запуска)
				_currentScheduleTask.Refresh();
			}
			//Текущая задача из расписания не актуально переходим ко следующей
			ToNextScheduleTask();
		}
		///<summary>Расписание изменено</summary>
		private void ScheduleChange()
		{
			_currentScheduleTask = _schedule?.GetCurrentScheduleTask();
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = new CancellationTokenSource();
			//Если сейчас не проигрывается видео, то переходим к следующему видео.
			if (string.IsNullOrEmpty(CurrentVideoSource))
				ToNextVideo();
			//Если проигрывается видео, то просто выходим, из нового расписание видео включится когда произойдёт вызов ToNextVideo
		}
		private void ToNextScheduleTask()
		{
			_currentScheduleTask = _schedule?.GetCurrentScheduleTask();
			//Если текущая задача из расписания успешно установлена
			if (_currentScheduleTask != null)
			{
				//Просто устанавливает следующее видео из текущегозадания
				ToNextVideo();
				return;
			}
			//Если расписание отсутвует
			if (_schedule == null)
				return; //Просто выходим следующий запус расчитан на ввод нового расписания

			//Т.к. текущего задания нет, необходимо определить следующую задачу из расписания
			var nextT = _schedule.GetTimeToNextScheduleTask();
			Task.Delay(nextT, _cancellationTokenSource.Token).ContinueWith((t) => ToNextVideo(), _cancellationTokenSource.Token, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}