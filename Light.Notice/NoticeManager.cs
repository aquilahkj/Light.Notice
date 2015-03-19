using System;
using System.Collections.Generic;
using System.Configuration;

namespace Light.Notice
{
	public static class NoticeManager
	{
		static Dictionary<string,NoticeExecutor> dict = new Dictionary<string, NoticeExecutor> ();

		static object objlock = new object ();

		static  bool isLoadConfig = false;

		public static NoticeExecutor GetNoticeSender (string name)
		{
			InnerLoad ();
			return dict [name];
		}

		public static NoticeExecutor CreateNoticeSender (INoticeConfig config)
		{
			NoticeExecutor sender = new NoticeExecutor (config);
			return sender;
		}

		static void InnerLoad ()
		{
			if (!isLoadConfig) {
				lock (objlock) {
					if (!isLoadConfig) {
						INoticeSetting setting = (NoticeSetting)ConfigurationManager.GetSection ("noticeSetting");
						foreach (INoticeConfig config in setting.NoticeConfigs) {
							NoticeExecutor sender = new NoticeExecutor (config);
							dict.Add (config.Name, sender);
						}
						isLoadConfig = true;
					}
				}
			}
		}

	}
}

