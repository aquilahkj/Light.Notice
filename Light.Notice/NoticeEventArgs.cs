using System;

namespace Light.Notice
{
	public class NoticeEventArgs
	{
		NoticeData data;

		public NoticeData Data {
			get {
				return data;
			}
		}

		NoticeResult result;

		public NoticeResult Result {
			get {
				return result;
			}
		}

		string message;

		public string Message {
			get {
				return message;
			}
		}

		public NoticeEventArgs (NoticeData data, NoticeResult result, string message)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			this.data = data;
			this.result = result;
			this.message = message;
		}
	}
}

