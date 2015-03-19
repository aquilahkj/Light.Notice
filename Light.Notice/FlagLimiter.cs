using System;
using System.Collections.Generic;

namespace Light.Notice
{
	public class FlagLimiter
	{
		const int DEFAULT_LIMIT_COUNT = 10;

		const int DEFAULT_LIMIT_TIME = 300;

		int limitCount;

		int limitTime;

		Dictionary<string, FlagLimitStatus> dict = new Dictionary<string, FlagLimitStatus> ();

		public int FlagCount{
			get {
				return this.dict.Count;
			}
		}

		public FlagLimiter (int limitCount, int limitTime)
		{
			if (limitCount > 0) {
				this.limitCount = limitCount;
			}
			else {
				this.limitCount = DEFAULT_LIMIT_COUNT;
			}
			if (limitTime > 0) {
				this.limitTime = limitTime;
			}
			else {
				this.limitTime = DEFAULT_LIMIT_TIME;
			}
		}

		public void Clear ()
		{
			lock (this) {
				if (this.dict.Count > 0) {
					List<string> removeList = new List<string> ();
					DateTime dt = DateTime.Now;
					foreach (KeyValuePair<string, FlagLimitStatus> kv in this.dict) {
						if ((dt - kv.Value.TimeStamp).TotalSeconds > this.limitTime) {
							removeList.Add (kv.Key);
						}
					}
					if (removeList.Count > 0) {
						foreach (string key in removeList) {
							this.dict.Remove (key);
						}
					}
				}
			}
		}

		public bool Check (string flag)
		{
			lock (this) {
				FlagLimitStatus status;
				if (!this.dict.TryGetValue (flag, out status)) {
					status = new FlagLimitStatus ();
					status.TimeStamp = DateTime.Now;
					status.SendTimes = 1;
					this.dict [flag] = status;
					return true;
				}
				else {
					if ((DateTime.Now - status.TimeStamp).TotalSeconds > this.limitTime) {
						status.TimeStamp = DateTime.Now;
						status.SendTimes = 1;
						return true;
					}
					else if (status.SendTimes < this.limitCount) {
						status.SendTimes++;
						return true;
					}
					else {
						return false;
					}
				}
			}
		}
	}
}

