using NUnit.Framework;
using System;
using System.Threading;

namespace Light.Notice.Test
{
	[TestFixture ()]
	public class FlagLimiterTest
	{
		[Test ()]
		public void TestCase1 ()
		{
			FlagLimiter flagLimiter = new FlagLimiter (1, 3);

			string flag = "abc";

			Assert.IsTrue (flagLimiter.Check (flag));

			Thread.Sleep (1000);

			Assert.IsFalse (flagLimiter.Check (flag));

			Thread.Sleep (1000);

			Assert.IsFalse (flagLimiter.Check (flag));
		}

		[Test ()]
		public void TestCase2 ()
		{
			FlagLimiter flagLimiter = new FlagLimiter (1, 3);

			string flag = "abc";

			Assert.IsTrue (flagLimiter.Check (flag));

			Thread.Sleep (1000);

			Assert.IsFalse (flagLimiter.Check (flag));

			Thread.Sleep (4000);

			Assert.IsTrue (flagLimiter.Check (flag));
		}

		[Test ()]
		public void TestCase3 ()
		{
			FlagLimiter flagLimiter = new FlagLimiter (2, 10);

			string flag1 = "abc";

			string flag2 = "efg";

			Assert.IsTrue (flagLimiter.Check (flag1));

			Thread.Sleep (1000);

			Assert.IsTrue (flagLimiter.Check (flag2));

			Thread.Sleep (1000);

			Assert.IsTrue (flagLimiter.Check (flag1));

			Thread.Sleep (1000);

			Assert.IsTrue (flagLimiter.Check (flag2));

			Thread.Sleep (1100);

			Assert.IsFalse (flagLimiter.Check (flag1));

			Thread.Sleep (1100);

			Assert.IsFalse (flagLimiter.Check (flag2));
		}

	}
}

