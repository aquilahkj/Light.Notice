using System;

namespace Light.Notice
{
	public class FlagLimitStatus
	{
		DateTime timeStamp;

		public DateTime TimeStamp {
			get {
				return timeStamp;
			}
			set {
				timeStamp = value;
			}
		}

		int sendTimes;

		public int SendTimes {
			get {
				return sendTimes;
			}
			set {
				sendTimes = value;
			}
		}
	}
}

