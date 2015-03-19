using System;
using System.Collections.Generic;

namespace Light.Notice
{
	public interface IEmailConfig
	{
		bool Enable {
			get;
		}

		string Sender {
			get;
		}

		string SenderName {
			get;
		}

		string UserName {
			get;
		}

		string Password {
			get;
		}

		string Host {
			get;
		}

		int Port {
			get;
		}

		bool UseSSL {
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

		IEnumerable<IReceiver> Receivers {
			get;
		}
	}
}

