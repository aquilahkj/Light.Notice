using System;
using System.Configuration;
using System.Collections.Generic;

namespace Light.Notice
{
	public class ReceiverCollection:ConfigurationElementCollection,IEnumerable<IReceiver>
	{
		public ReceiverCollection ()
		{
		}

		#region implemented abstract members of ConfigurationElementCollection

		protected override ConfigurationElement CreateNewElement ()
		{
			return new Receiver ();
		}

		protected override object GetElementKey (ConfigurationElement element)
		{
			return ((Receiver)element).Address;
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
				return "receiver";
			}
		}

		public Receiver this[int index]
		{
			get
			{
				return (Receiver)BaseGet(index);
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

		IEnumerator<IReceiver> IEnumerable<IReceiver>.GetEnumerator ()
		{
			foreach(object item in this)
			{
				yield return (IReceiver)item;
			}
		}
	}
}

