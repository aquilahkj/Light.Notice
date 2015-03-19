using System;
using System.Collections.Generic;

namespace Light.Notice
{
	public interface INoticeSetting
	{
		IEnumerable<INoticeConfig> NoticeConfigs {
			get;
		}
	}
}

