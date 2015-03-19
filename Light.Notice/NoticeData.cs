using System;

namespace Light.Notice
{
	public class NoticeData
	{
		NoticeDataType dataType;

		public NoticeDataType DataType {
			get {
				return dataType;
			}
		}

		string title;

		public string Title {
			get {
				return title;
			}
		}

		string content;

		public string Content {
			get {
				return content;
			}
		}

		string flag;

		public string Flag {
			get {
				return flag;
			}
		}

		bool isHtmlContent;

		public bool IsHtmlContent {
			get {
				return isHtmlContent;
			}
		}

		int emailSendTimes = 0;

		public int EmailSendTimes {
			get {
				return emailSendTimes;
			}
		}

		int remoteSendTimes = 0;

		public int RemoteSendTimes {
			get {
				return remoteSendTimes;
			}
		}

		bool highLevel = false;

		public bool HighLevel {
			get {
				return highLevel;
			}
			set {
				highLevel = value;
			}
		}

		bool addSource = false;

		public bool AddSource {
			get {
				return addSource;
			}
			set {
				addSource = value;
			}
		}

		DateTime createTime = DateTime.Now;

		public DateTime CreateTime {
			get {
				return createTime;
			}
		}

		NoticeData ()
		{

		}

		public static NoticeData CreateNoticeData (string title, string content, string flag, bool isHtmlContent)
		{
			NoticeData data = new NoticeData ();
			data.title = title;
			data.content = content;
			data.flag = flag;
			data.isHtmlContent = isHtmlContent;
			data.dataType = NoticeDataType.Normal;
			return data;
		}

		public static NoticeData CreateErrorData (string title, string content)
		{
			NoticeData data = new NoticeData ();
			data.title = title;
			data.content = content;
			data.flag = null;
			data.isHtmlContent = false;
			data.dataType = NoticeDataType.Error;
			return data;
		}

		//		public SendData (string title, string content, string flag, bool isHtmlContent)
		//		{
		//			this.title = title;
		//			this.content = content;
		//			this.flag = flag;
		//			this.isHtmlContent = isHtmlContent;
		//		}

		//		public SendData (string title, string content, string flag, bool isHtmlContent)
		//		{
		//			this.title = title;
		//			this.content = content;
		//			this.flag = flag;
		//			this.isHtmlContent = isHtmlContent;
		//		}


		public void AddEmailTimes ()
		{
			this.emailSendTimes++;
		}

		public void AddRemoteTimes ()
		{
			this.remoteSendTimes++;
		}
	}
}

