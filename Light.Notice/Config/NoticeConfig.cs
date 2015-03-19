using System;
using System.Configuration;

namespace Light.Notice
{
	public class NoticeConfig:ConfigurationElement,INoticeConfig
	{
		public NoticeConfig ()
		{
		}

		#region INoticeConfig implementation

		[ConfigurationProperty ("sender", IsRequired = true)]
		public string Sender {
			get {
				return this ["sender"] as string;
			}
		}

		[ConfigurationProperty ("name", IsRequired = true, IsKey = true)]
		public string Name {
			get {
				return this ["name"] as string;
			}
		}

		[ConfigurationProperty ("status", IsRequired = false, DefaultValue = ConfigStatus.Both)]
		public ConfigStatus Status {
			get {
				return (ConfigStatus)this ["status"];
			}
		}

		[ConfigurationProperty ("emailConfig", IsRequired = false, DefaultValue = null)]
		public EmailConfig EmailConfig {
			get {
				return this ["emailConfig"] as EmailConfig;
			}
		}

		[ConfigurationProperty ("remoteConfig", IsRequired = false, DefaultValue = null)]
		public RemoteConfig RemoteConfig {
			get {
				return this ["remoteConfig"] as RemoteConfig;
			}
		}

		IEmailConfig INoticeConfig.EmailConfig {
			get {
				return this.EmailConfig;
			}
		}

		IRemoteConfig INoticeConfig.RemoteConfig {
			get {
				return this.RemoteConfig;
			}
		}

		#endregion
	}
}

