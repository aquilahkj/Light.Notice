using System;

namespace Light.Notice
{
	public interface INoticeConfig
	{

		string Name {
			get;
		}

		ConfigStatus Status {
			get;
		}

		IEmailConfig EmailConfig {
			get;
		}

		IRemoteConfig RemoteConfig {
			get;
		}
	}
}

