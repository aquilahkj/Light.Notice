using System;
using System.Configuration;
using System.Collections.Generic;

namespace Light.Notice
{
	public class NoticeSetting:ConfigurationSection,INoticeSetting
	{
		public NoticeSetting ()
		{
		}

		[ConfigurationProperty ("noticeConfigs")]
		public NoticeConfigCollection NoticeConfigs {
			get {
				return (NoticeConfigCollection)this ["noticeConfigs"];
			}
		}

		#region INoticeSetting implementation

		IEnumerable<INoticeConfig> INoticeSetting.NoticeConfigs {
			get {
				return this.NoticeConfigs;
			}
		}

		#endregion
	}
}

