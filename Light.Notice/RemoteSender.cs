using System;
using System.Timers;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Web;

namespace Light.Notice
{
	public class RemoteSender
	{
		//		const int DEFAULT_LIMIT_COUNT = 10;
		//
		//		const int DEFAULT_LIMIT_TIME = 300;
		//
		const int MAX_QUEUE_COUNT = 1000;
		//
		//		bool flagLimitEnable;
		//
		//		int flagLimitCount;
		//
		//		int flagLimitTime;

		FlagLimiter flagLimiter;

		int retryTimes;

		public int RetryTimes {
			get {
				return retryTimes;
			}
		}

		string servicePath;

		int timeout;

		Timer timer = null;

		public event NoticeEventHandle OnNoticeEvent;

		Queue<NoticeData> queue = new Queue<NoticeData> ();

		//		Dictionary<string, FlagLimitStatus> dict = new Dictionary<string, FlagLimitStatus> ();

		string name;

		public string Name {
			get {
				return name;
			}
		}

		public RemoteSender (string name, IRemoteConfig config)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (string.IsNullOrEmpty (config.ServicePath)) {
				throw new ArgumentNullException ("config service path");
			}
			if (!config.ServicePath.StartsWith ("http://")) {
				throw new ArgumentException ("config service path");
			}
			this.name = name;
			if (config.RetryTimes > 0) {
				this.retryTimes = config.RetryTimes;
			}
			this.servicePath = config.ServicePath;
			if (config.SendTimeout > 0) {
				this.timeout = config.SendTimeout;
			}

//			this.flagLimitEnable = config.FlagLimitEnable;
//			if (this.flagLimitEnable) {
//				if (config.FlagLimitCount >= 0 && config.FlagLimitCount < int.MaxValue) {
//					this.flagLimitCount = config.FlagLimitCount;
//				}
//				else {
//					this.flagLimitCount = DEFAULT_LIMIT_COUNT;
//				}
//				if (config.FlagLimitTime >= 0 && config.FlagLimitTime < int.MaxValue) {
//					this.flagLimitTime = config.FlagLimitTime;
//				}
//				else {
//					this.flagLimitTime = DEFAULT_LIMIT_TIME;
//				}
//			}

			if (config.FlagLimitEnable) {
				this.flagLimiter = new FlagLimiter (config.FlagLimitCount, config.FlagLimitTime);
			}

			this.timer = new Timer ();
			this.timer.AutoReset = false;
			this.timer.Interval = 1000;
			this.timer.Elapsed += HandleElapsed;
			this.timer.Start ();
		}

		void Process ()
		{
//			if (this.flagLimitEnable && this.dict.Count > 0) {
//				List<string> removeList = new List<string> ();
//				DateTime dt = DateTime.Now;
//				foreach (KeyValuePair<string, FlagLimitStatus> kv in this.dict) {
//					if ((dt - kv.Value.TimeStamp).TotalSeconds > this.flagLimitTime) {
//						removeList.Add (kv.Key);
//					}
//				}
//				if (removeList.Count > 0) {
//					foreach (string key in removeList) {
//						this.dict.Remove (key);
//					}
//				}
//			}
			while (true) {
				if (this.queue.Count > 0) {
					NoticeData data;
					lock (this.queue) {
						data = this.queue.Dequeue ();
					}
					NoticeResult result = NoticeResult.NoProcess;
					string message = null;
					try {
//						if (this.flagLimitEnable && !data.HighLevel && data.Flag != null && data.DataType == NoticeDataType.Normal) {
//							FlagLimitStatus status;
//							if (!this.dict.TryGetValue (data.Flag, out status)) {
//								status = new FlagLimitStatus ();
//								status.TimeStamp = DateTime.Now;
//								status.SendTimes = 0;
//							}
//							if (status.SendTimes < this.flagLimitCount) {
//								status.SendTimes++;
//								this.dict [data.Flag] = status;
//								SendRequest (data);
//								result = NoticeResult.Success;
//							}
//							else {
//								result = NoticeResult.NoProcess;
//								message = "message limited";
//							}
//						}
						if (this.flagLimiter != null && data.Flag != null && !data.HighLevel && data.DataType == NoticeDataType.Normal) {
							if (this.flagLimiter.Check (data.Flag)) {
								SendRequest (data);
								result = NoticeResult.Success;
							}
							else {
								result = NoticeResult.NoProcess;
								message = "message limited";
							}
						}
						else {
							SendRequest (data);
							result = NoticeResult.Success;
						}
					}
					catch (Exception ex) {
						result = NoticeResult.Failed;
						message = ex.Message;
					}
					finally {
						if (OnNoticeEvent != null) {
							NoticeEventArgs args = new NoticeEventArgs (data, result, message);
							OnNoticeEvent.BeginInvoke (this, args, null, null);
						}
					}
				}
				else {
					break;
				}
			}
		}

		void HandleElapsed (object sender, ElapsedEventArgs e)
		{
			Process ();
			this.timer.Start ();
		}

		void SendRequest (NoticeData data)
		{
			string subject = HttpUtility.HtmlEncode (data.Title);
//			if (!string.IsNullOrEmpty (data.Flag)) {
//				subject = HttpUtility.HtmlEncode (string.Format ("{0}[FLAG:{1}]", data.Title, data.Flag));
//			}
//			else {
//				subject = HttpUtility.HtmlEncode (data.Title);
//			}

			string url = string.Format ("{0}?subject={1}&html={2}", this.servicePath, subject, data.IsHtmlContent ? 1 : 0, Environment.MachineName);
			if (!string.IsNullOrEmpty (data.Flag)) {
				url += string.Format ("&flag={0}", HttpUtility.HtmlEncode (data.Flag));
			}
			if (data.AddSource) {
				url += string.Format ("&server={0}&name={1}", HttpUtility.HtmlEncode (Environment.MachineName), HttpUtility.HtmlEncode (this.name));
			}
			HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create (url);
			if (this.timeout > 0) {
				httpReq.Timeout = this.timeout;
			}
			httpReq.Method = "POST";
			using (StreamWriter writer = new StreamWriter (httpReq.GetRequestStream (), Encoding.UTF8)) {
				writer.Write (data.Content);
			}

			using (HttpWebResponse response = (HttpWebResponse)httpReq.GetResponse ()) {
				if (response.StatusCode == HttpStatusCode.OK) {
					using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
						string content = reader.ReadToEnd ();
						if (content != "1") {
							throw new Exception ("remote response error,response content:\r" + content);
						}
					}
				}
			}

		}

		public void Send (NoticeData data)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			if (this.queue.Count <= MAX_QUEUE_COUNT) {
				lock (this.queue) {
					this.queue.Enqueue (data);
				}
			}
			else {
				if (OnNoticeEvent != null) {
					NoticeEventArgs args = new NoticeEventArgs (data, NoticeResult.NoProcess, "queue is full");
					OnNoticeEvent.BeginInvoke (this, args, null, null);
				}
			}
		}
	}
}

