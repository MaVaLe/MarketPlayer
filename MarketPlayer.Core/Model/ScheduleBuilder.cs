using System;
using System.Globalization;
using System.IO;

namespace MarketPlayer.Core.Model
{
	internal static class ScheduleBuilder
	{
		///<returns>В случае если в кортеже отсутсвует объект (null) то в котреже содежится описание ошибки.</returns>
		internal static (Schedule BuildResult, string BuildError) Build(string[] scheduleStrs)
		{
			var schedule = new Schedule();
			foreach (var str in scheduleStrs)
			{
				if (string.IsNullOrEmpty(str))
					return (null, "Присутствует пустая строка");
				var splites = str.Split(' ');
				if (splites.Length != 3)
					return (null, $"В строке {str} не три элемента разделённых пробелом");
				TimeSpan startTime;
				TimeSpan stopTime;
				if (!TimeSpan.TryParseExact(splites[0], @"hh\:mm", CultureInfo.InvariantCulture, out startTime))
					return (null, $"В строке {str} начальное время ({splites[0]}) задано неверно.");
				if (!TimeSpan.TryParseExact(splites[1], @"hh\:mm", CultureInfo.InvariantCulture, out stopTime))
					return (null, $"В строке {str} начальное время ({splites[1]}) задано неверно.");
				if (startTime >= stopTime)
					return (null, $"В строке {str} конечное время раньше начального.");
				if (!Directory.Exists(splites[2]))
					return (null, $"Директории {splites[2]} не существует");
				var scheduleTask = new ScheduleTask(startTime, stopTime, splites[2]);
				if (!schedule.TryAdd(scheduleTask))
					return (null, $"Событие {str} пересекается с ранее описанным событием.");
			}
			return (schedule, null);
		}
	}
}