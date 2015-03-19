using System;
using System.Configuration;
using System.Collections.Generic;

namespace Light.Notice
{
	public class EmailConfig:ConfigurationElement,IEmailConfig
	{
		#region IEmailConfig implementation

		[ConfigurationProperty ("enable", IsRequired = true)]
		public bool Enable {
			get {
				return (bool)this ["enable"];
			}
		}

		[ConfigurationProperty ("sender", IsRequired = true)]
		public string Sender {
			get {
				return this ["sender"] as string;
			}
		}

		[ConfigurationProperty ("senderName", IsRequired = false)]
		public string SenderName {
			get {
				return this ["senderName"] as string;
			}
		}

		[ConfigurationProperty ("username", IsRequired = true)]
		public string UserName {
			get {
				return this ["username"] as string;
			}
		}

		[ConfigurationProperty ("password", IsRequired = true)]
		public string Password {
			get {
				return this ["password"] as string;
			}
		}

		[ConfigurationProperty ("host", IsRequired = true)]
		public string Host {
			get {
				return this ["host"] as string;
			}
		}

		[ConfigurationProperty ("port", IsRequired = false)]
		public int Port {
			get {
				return (int)this ["port"];
			}
		}

		[ConfigurationProperty ("useSSL", IsRequired = false, DefaultValue = false)]
		public bool UseSSL {
			get {
				return (bool)this ["useSSL"];
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

		[ConfigurationProperty ("receivers", IsRequired = true)]
		public ReceiverCollection Receivers {
			get {
				return this ["receivers"] as ReceiverCollection;
			}
		}

		IEnumerable<IReceiver> IEmailConfig.Receivers {
			get {
				return this.Receivers;
			}
		}

		#endregion
	}
}

