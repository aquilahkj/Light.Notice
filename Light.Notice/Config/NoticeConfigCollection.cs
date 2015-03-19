using System;
using System.Configuration;
using System.Collections.Generic;

namespace Light.Notice
{
	public class NoticeConfigCollection:ConfigurationElementCollection,IEnumerable<INoticeConfig>
	{
		public NoticeConfigCollection ()
		{
		}


		#region implemented abstract members of ConfigurationElementCollection

		protected override ConfigurationElement CreateNewElement ()
		{
			return new NoticeConfig ();
		}

		protected override object GetElementKey (ConfigurationElement element)
		{
			return ((NoticeConfig)element).Name;
		}

		#endregion

		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		protected override string ElementName
		{
			get
			{
				return "noticeConfig";
			}
		}

		public NoticeConfig this[int index]
		{
			get
			{
				return (NoticeConfig)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		IEnumerator<INoticeConfig> IEnumerable<INoticeConfig>.GetEnumerator ()
		{
			foreach(object item in this)
			{
				yield return (INoticeConfig)item;
			}
		}
	}
}

