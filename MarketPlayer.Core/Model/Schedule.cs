using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlayer.Core.Model
{
	///<summary>
	///Класс неоптимизирован, для упрощения строения. И этот момент ни является узким звеном для решения бизнес задачи.
	///Для улучшения оптимизации использовать LinkedList c задачами, сортироваными по времени. И бегать оп ним по кругу в течении дня.
	///Так же использовать ScheduleTask с пустым путём для момента тишины.
	///</summary>
	internal class Schedule
	{
		private List<ScheduleTask> _tasks = new List<ScheduleTask>();

		internal ScheduleTask GetCurrentScheduleTask()
		{
			var time = DateTime.Now.TimeOfDay;
			return _tasks.FirstOrDefault(t => t.Intersection(time));
		}

		internal TimeSpan GetTimeToNextScheduleTask()
		{
			var time = DateTime.Now.TimeOfDay;
			ScheduleTask curScheduleTask = null;
			double curToTaskmms = double.MaxValue;
			//Поиск ближайшей по времени задачи
			foreach (var task in _tasks)
			{
				var interval = (task.Start - time).TotalMilliseconds;
				if (interval < 0)
					continue;
				if (interval < curToTaskmms)
				{
					curToTaskmms = interval;
					curScheduleTask = task;
				}
			}
			if (curScheduleTask != null)
				return TimeSpan.FromMilliseconds(curToTaskmms);
			//Ближайшая задача в следующем дне
			//Поиск ближайшей по времени задачи
			var startDaytime = TimeSpan.FromMilliseconds(0);
			var timeToFinishDay = TimeSpan.FromDays(1) - time;
			foreach (var task in _tasks)
			{
				var interval = (task.Start - startDaytime).TotalMilliseconds;
				if (interval < 0)
					continue;
				if (interval < curToTaskmms)
				{
					curToTaskmms = interval;
					curScheduleTask = task;
				}
			}

			if (curScheduleTask == null)
				throw new Exception("Неверно собранное расписание оно пустое");

			return TimeSpan.FromMilliseconds(curToTaskmms) + timeToFinishDay;
		}
		internal bool TryAdd(ScheduleTask newTask)
		{
			if (_tasks.Any(t => t.Intersection(newTask)))
				return false;
			_tasks.Add(newTask);
			return true;
		}
	}
}