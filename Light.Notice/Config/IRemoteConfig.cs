using System;

namespace Light.Notice
{
	public interface IRemoteConfig
	{
		bool Enable {
			get;
		}
		 
		string ServicePath {
			get;
		}

		bool FlagLimitEnable {
			get;
		}

		int FlagLimitCount {
			get;
		}

		int FlagLimitTime {
			get;
		}

		int SendTimeout {
			get;
		}

		int RetryTimes {
			get;
		}
	}
}

