using System;

namespace Light.Notice
{
	public interface IReceiver
	{
		string Address {
			get;
		}

		string Name {
			get;
		}
	}
}

