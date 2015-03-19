using System;
using System.Configuration;

namespace Light.Notice
{
	public class RemoteConfig:ConfigurationElement,IRemoteConfig
	{
		public RemoteConfig ()
		{
		}

		#region IRemoteConfig implementation

		[ConfigurationProperty ("enable", IsRequired = true)]
		public bool Enable {
			get {
				return (bool)this ["enable"];
			}
		}

		[ConfigurationProperty ("servicePath", IsRequired = false)]
		public string ServicePath {
			get {
				return this ["servicePath"] as string;
			}
		}

		[ConfigurationProperty ("flagLimitEnable", IsRequired = false, DefaultValue = false)]
		public bool FlagLimitEnable {
			get {
				return (bool)this ["flagLimitEnable"];
			}
		}

		[ConfigurationProperty ("flagLimitCount", IsRequired = false)]
		public int FlagLimitCount {
			get {
				return (int)this ["flagLimitCount"];
			}
		}

		[ConfigurationProperty ("flagLimitTime", IsRequired = false)]
		public int FlagLimitTime {
			get {
				return (int)this ["flagLimitTime"];
			}
		}

		[ConfigurationProperty ("sendTimeout", IsRequired = false)]
		public int SendTimeout {
			get {
				return (int)this ["sendTimeout"];
			}
		}

		[ConfigurationProperty ("retryTimes", IsRequired = false)]
		public int RetryTimes {
			get {
				return (int)this ["retryTimes"];
			}
		}

		#endregion
	}
}

