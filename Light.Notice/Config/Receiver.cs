using System;
using System.Configuration;

namespace Light.Notice
{
	public class Receiver:ConfigurationElement,IReceiver
	{
		public Receiver ()
		{
		}

		#region IReceiver implementation

		[ConfigurationProperty ("address", IsKey = true, IsRequired = true)]
		public string Address {
			get {
				return base ["address"] as string;
			}
			set {
				base ["address"] = value;
			}
		}

		[ConfigurationProperty ("name", IsRequired = false)]
		public string Name {
			get {
				return base ["name"] as string;
			}
			set {
				base ["name"] = value;
			}
		}

		#endregion
	}
}

