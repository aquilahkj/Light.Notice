using System;

namespace Light.Notice
{
	public class NoticeExecutor
	{
		ConfigStatus status;

		public ConfigStatus Status {
			get {
				return status;
			}
		}

		string name;

		public string Name {
			get {
				return name;
			}
		}

		EmailSender emailSender = null;

		RemoteSender remoteSender = null;

		internal NoticeExecutor (INoticeConfig config)
		{
			this.name = config.Name;
			this.status = config.Status;
			if (config.EmailConfig != null && config.EmailConfig.Enable) {
				this.emailSender = new EmailSender (config.Name, config.EmailConfig);
				this.emailSender.OnNoticeEvent += HandleOnNoticeEvent;
			}
			if (config.RemoteConfig != null && config.RemoteConfig.Enable) {
				this.remoteSender = new RemoteSender (config.Name, config.RemoteConfig);
				this.remoteSender.OnNoticeEvent += HandleOnNoticeEvent;
			}
		}

		void HandleOnNoticeEvent (object sender, NoticeEventArgs args)
		{
			if (args.Result == NoticeResult.Failed) {
				NoticeData data = args.Data;
				if (data.DataType == NoticeDataType.Normal) {
					data.HighLevel = true;
					if (sender is EmailSender) {
						if (this.emailSender.RetryTimes > args.Data.EmailSendTimes) {
							this.emailSender.Send (data);
						}
						else if (this.remoteSender != null) {
							string title = string.Format ("email sender send data error [{0}:{1}]", Environment.MachineName, this.name);
							NoticeData errorData = NoticeData.CreateErrorData (title, args.Message);
							this.remoteSender.Send (errorData);
							if (this.status == ConfigStatus.EmailFirst && data.RemoteSendTimes == 0) {
								this.remoteSender.Send (data);
							}
						}

					}
					else if (sender is RemoteSender) {
						if (this.remoteSender.RetryTimes > args.Data.RemoteSendTimes) {
							this.remoteSender.Send (data);
						}
						else if (this.emailSender != null) {
							string title = string.Format ("remote sender send data error [{0}:{1}]", Environment.MachineName, this.name);
							NoticeData errorData = NoticeData.CreateErrorData (title, args.Message);
							this.remoteSender.Send (errorData);
							if (this.status == ConfigStatus.RemoteFirst && data.EmailSendTimes == 0) {
								this.emailSender.Send (data);
							}
						}
					}
				}
			}
		}

		public bool SendData (string title, string content, string flag, bool isHtmlContent, bool addSource)
		{
//			if (addSource) {
//				title = string.Format ("{0} [{1}:{2}]", title, Environment.MachineName, this.name);
//			}
			NoticeData data = NoticeData.CreateNoticeData (title, content, flag ?? string.Empty, isHtmlContent);
			data.AddSource = addSource;
			bool result = false;
			if (this.status == ConfigStatus.Both) {
				if (this.emailSender != null) {
					this.emailSender.Send (data);
					result = true;
				}
				if (this.remoteSender != null) {
					this.remoteSender.Send (data);
					result = true;
				}
			}
			else if (this.status == ConfigStatus.EmailFirst) {
				if (this.emailSender != null) {
					this.emailSender.Send (data);
					result = true;
				}
				else if (this.remoteSender != null) {
					this.remoteSender.Send (data);
					result = true;
				}

			}
			else if (this.status == ConfigStatus.RemoteFirst) {
				if (this.remoteSender != null) {
					this.remoteSender.Send (data);
					result = true;
				}
				else if (this.emailSender != null) {
					this.emailSender.Send (data);
					result = true;
				}
			}
			return result;
		}

		public bool SendData (string title, string content, string flag, bool isHtmlContent)
		{
			return SendData (title, content, flag, isHtmlContent, true);
		}


		public bool SendData (string title, string content, string flag)
		{
			return SendData (title, content, flag, false, true);
		}
	}
}

