using System;
using System.Timers;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Light.Notice
{
	public class EmailSender
	{
		//		const int DEFAULT_LIMIT_COUNT = 10;
		//
		//		const int DEFAULT_LIMIT_TIME = 300;

		const int MAX_QUEUE_COUNT = 1000;

		MailAddress sender;

		//		bool flagLimitEnable;
		//
		//		int flagLimitCount;
		//
		//		int flagLimitTime;

		FlagLimiter flagLimiter;

		int retryTimes;

		List<MailAddress> receivers;

		Timer timer = null;

		public event NoticeEventHandle OnNoticeEvent;

		Queue<NoticeData> queue = new Queue<NoticeData> ();

		//		Dictionary<string, FlagLimitStatus> dict = new Dictionary<string, FlagLimitStatus> ();

		SmtpClient client;

		//		string name;

		//		public string Name {
		//			get {
		//				return name;
		//			}
		//		}
		//
		public int RetryTimes {
			get {
				return retryTimes;
			}
		}

		string name;

		public string Name {
			get {
				return name;
			}
		}

		public EmailSender (string name, IEmailConfig config)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
//			if (string.IsNullOrEmpty (config.Sender)) {
//				throw new ArgumentNullException ("config sender");
//			}
			if (string.IsNullOrEmpty (config.UserName)) {
				throw new ArgumentNullException ("config username");
			}
			if (string.IsNullOrEmpty (config.Password)) {
				throw new ArgumentNullException ("config password");
			}
			if (string.IsNullOrEmpty (config.Host)) {
				throw new ArgumentNullException ("config host");
			}
			if (config.Receivers == null) {
				throw new ArgumentNullException ("config receivers");
			}
			this.name = name;
			if (config.RetryTimes > 0) {
				this.retryTimes = config.RetryTimes;
			}

			this.client = new SmtpClient (); 
			if (config.SendTimeout > 0) {
				this.client.Timeout = config.SendTimeout;
			}

			this.client.Host = config.Host;
			if (config.Port > 0 && config.Port < 65535) {
				this.client.Port = config.Port;
			}
			this.client.EnableSsl = config.UseSSL;
			this.client.Credentials = new System.Net.NetworkCredential (config.UserName, config.Password);

			string sender;
			string senderName;
			if (!string.IsNullOrEmpty (config.Sender)) {
				sender = config.Sender;
			}
			else {
				sender = config.UserName;
			}
			if (!string.IsNullOrEmpty (config.SenderName)) {
				senderName = config.SenderName;
			}
			else {
				senderName = sender;
			}

			try {
				this.sender = new MailAddress (sender, senderName);
			}
			catch (Exception ex) {
				throw new Exception (string.Format ("sender account {0} error,{1}", sender, ex.Message));
			}

			this.receivers = new List<MailAddress> ();
			foreach (IReceiver receiver in config.Receivers) {
				try {
					MailAddress address = new MailAddress (receiver.Address, receiver.Name);
					this.receivers.Add (address);
				}
				catch (Exception ex) {
					throw new Exception (string.Format ("receiver account {0} error,{1}", receiver.Address, ex.Message));
				}
			}
			if (this.receivers.Count == 0) {
				throw new ArgumentException ("config receivers count is 0");
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
//			if (this.flagLimiter != null) {
//				this.flagLimiter.Clear ();
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
						if (this.flagLimiter != null && data.Flag != null && !data.HighLevel && data.DataType == NoticeDataType.Normal) {
//							FlagLimitStatus status;
//							if (!this.dict.TryGetValue (data.Flag, out status)) {
//								status = new FlagLimitStatus ();
//								status.TimeStamp = DateTime.Now;
//								status.SendTimes = 0;
//							}
//							if (status.SendTimes < this.flagLimitCount) {
//								status.SendTimes++;
//								this.dict [data.Flag] = status;
//								SendMail (data);
//								result = NoticeResult.Success;
//							}
//							else {
//								result = NoticeResult.NoProcess;
//								message = "message limited";
//							}
							if (this.flagLimiter.Check (data.Flag)) {
								SendMail (data);
								result = NoticeResult.Success;
							}
							else {
								result = NoticeResult.NoProcess;
								message = "message limited";
							}
						}
						else {
							SendMail (data);
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

		void SendMail (NoticeData data)
		{
			data.AddEmailTimes ();
			MailMessage msg = new MailMessage ();
			msg.From = this.sender;
			foreach (MailAddress receiver in this.receivers) {
				msg.To.Add (receiver);
			}
			string subject = data.Title;

			if (!string.IsNullOrEmpty (data.Flag)) {
				subject += string.Format ("[FLAG:{0}]", data.Flag);
			}
			if (data.AddSource) {
				subject += string.Format ("[{0}:{1}]", Environment.MachineName, this.name);
			}
			msg.Subject = subject;
			msg.SubjectEncoding = Encoding.UTF8;
			msg.Body = data.Content;
			msg.BodyEncoding = Encoding.UTF8;
			msg.IsBodyHtml = data.IsHtmlContent;
			this.client.Send (msg);
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

