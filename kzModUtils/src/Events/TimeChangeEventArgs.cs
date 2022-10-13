using System;

namespace kzModUtils.Events
{
	public class TimeChangeEventArgs : EventArgs
	{
		public TimeModel Time { get; private set; }

		public TimeChangeEventArgs(TimeModel time)
		{
			Time = time;
		}
	}
}
