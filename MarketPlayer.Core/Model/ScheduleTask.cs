using System;
using System.Collections.Generic;
using System.IO;

namespace MarketPlayer.Core.Model
{
	internal class ScheduleTask
	{
		private int _currentFileIndex = -1;
		private string[] _filesNames;

		internal ScheduleTask(TimeSpan start, TimeSpan stop, string path)
		{
			Path = path;
			Start = start;
			Stop = stop;
			var list = new List<string>(Directory.GetFiles(Path));
			list.Sort();
			_filesNames = list.ToArray();
		}
		internal string Path { get; private set; }
		internal TimeSpan Start { get; private set; }
		internal TimeSpan Stop { get; private set; }
		internal string GetNextFName()
		{
			_currentFileIndex = (_currentFileIndex + 1) % _filesNames.Length;
			return _filesNames[_currentFileIndex];
		}
		internal bool Intersection(TimeSpan time)
		{
			if (Start <= time && Stop > time)
				return true;
			return false;
		}
		internal bool Intersection(ScheduleTask newTask)
		{
			if (Stop <= newTask.Start)
				return false;
			else if (newTask.Stop <= Start)
				return false;
			return true;
		}
		internal void Refresh()
		{
			_currentFileIndex = -1;
		}
	}
}