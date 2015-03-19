using System;
using System.Configuration;

namespace Light.Notice.Demo
{
	/// <summary>
	/// EmailHelper配置类
	/// </summary>
	public sealed class EmailHelperSection : ConfigurationSection
	{
		public EmailHelperSection ()
		{
		}

		[ConfigurationProperty ("Smtp_Host", IsRequired = false, IsKey = true)]
		public string Smtp_Host {
			get {
				return (string)this ["Smtp_Host"];
			}
			set {
				this ["Smtp_Host"] = value;
			}
		}

		[ConfigurationProperty ("Smtp_Host1", IsRequired = false, IsKey = true)]
		public string Smtp_Host1 {
			get {
				return (string)this ["Smtp_Host1"];
			}
			set {
				this ["Smtp_Host1"] = value;
			}
		}

		[ConfigurationProperty ("Smtp_Account", DefaultValue = "")]
		public string Smtp_Account {
			get {
				return (string)this ["Smtp_Account"];
			}
			set {
				this ["Smtp_Account"] = value;
			}
		}

		[ConfigurationProperty ("Smtp_Pwd", DefaultValue = "")]
		public string Smtp_Pwd {
			get {
				return (string)this ["Smtp_Pwd"];
			}
			set {
				this ["Smtp_Pwd"] = value;
			}
		}
	}
}

