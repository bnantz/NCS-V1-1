//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using System.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestFixture]
    public class ExtendedFormatFixture
    {
        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void CreateBadFormTest()
        {
            ExtendedFormat format = new ExtendedFormat("5 * * *");
            Assert.IsNull(format);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeFormatTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * -3 * *");
            Assert.IsNull(format);
        }

        [Test]
        public void MinutesTest()
        {
            ExtendedFormat format = new ExtendedFormat("5 * * * *");
            Assert.AreEqual(1, format.Minutes.Length);
            Assert.AreEqual(5, format.Minutes[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MinutesOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("61 * * * *");
            Assert.IsNull(format);
        }

        [Test]
        public void MultiMinutesTest()
        {
            ExtendedFormat format = new ExtendedFormat("5,1 * * * *");
            Assert.AreEqual(2, format.Minutes.Length);
            Assert.AreEqual(5, format.Minutes[0]);
            Assert.AreEqual(1, format.Minutes[1]);
        }

        [Test]
        public void HoursTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 5 * * *");
            Assert.AreEqual(1, format.Hours.Length);
            Assert.AreEqual(5, format.Hours[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HoursOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 25 * * *");
            Assert.IsNull(format);
        }

        [Test]
        public void MultiHoursTest()
        {
            ExtendedFormat format = new ExtendedFormat("* 5,1 * * *");
            Assert.AreEqual(2, format.Hours.Length);
            Assert.AreEqual(5, format.Hours[0]);
            Assert.AreEqual(1, format.Hours[1]);
        }

        [Test]
        public void DaysTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 5 * *");
            Assert.AreEqual(1, format.Days.Length);
            Assert.AreEqual(5, format.Days[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DaysOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 32 * *");
            Assert.IsNull(format);
        }

        [Test]
        public void MultiDaysTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * 5,1 * *");
            Assert.AreEqual(2, format.Days.Length);
            Assert.AreEqual(5, format.Days[0]);
            Assert.AreEqual(1, format.Days[1]);
        }

        [Test]
        public void MonthsTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 5 *");
            Assert.AreEqual(1, format.Months.Length);
            Assert.AreEqual(5, format.Months[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MonthsOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 13 *");
            Assert.IsNull(format);
        }

        [Test]
        public void MultiMonthsTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * 5,1 *");
            Assert.AreEqual(2, format.Months.Length);
            Assert.AreEqual(5, format.Months[0]);
            Assert.AreEqual(1, format.Months[1]);
        }

        [Test]
        public void DaysOfWeekTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 5");
            Assert.AreEqual(1, format.DaysOfWeek.Length);
            Assert.AreEqual(5, format.DaysOfWeek[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DaysOfWeekOutOfRangeTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 8");
            Assert.IsNull(format);
        }

        [Test]
        public void MultiDaysOfWeekTest()
        {
            ExtendedFormat format = new ExtendedFormat("* * * * 5,1");
            Assert.AreEqual(2, format.DaysOfWeek.Length);
            Assert.AreEqual(5, format.DaysOfWeek[0]);
            Assert.AreEqual(1, format.DaysOfWeek[1]);
        }

        [Test]
        public void IsExpiredTest()
        {
            ExtendedFormat format = new ExtendedFormat("1 * * * *");
            bool expired = format.IsExpired(DateTime.Now.Subtract(new TimeSpan(1, 1, 2)), DateTime.Now);
            Assert.IsTrue(expired);
        }

        [Test]
        public void TestForBug353()
        {
            ExtendedFormat format = new ExtendedFormat("* * 29 * *");
            bool expired = format.IsExpired(new DateTime(2003, 2, 10, 10, 10, 0), new DateTime(2003, 3, 10, 10, 10, 0));
            Assert.IsTrue(expired, "Should have expired at end of month, even though there is no Feb. 29th");
        }

        [Test]
        public void TestForBug352()
        {
            ExtendedFormat format = new ExtendedFormat("0 0 1 4 *");
            bool expired = format.IsExpired(new DateTime(2001, 4, 01, 00, 05, 0), new DateTime(2002, 3, 31, 23, 59, 0));
            Assert.IsFalse(expired, "Has not hit midnight, April 1st of next year yet");
        }

        [Test]
        public void ExpirationWithAllWildCardsWillExpireAfterOneMinute()
        {
            ExtendedFormat oneMinuteExpiration = new ExtendedFormat("* * * * *");
            DateTime baseTime = DateTime.Parse("5/1/2004 12:00:00");
            DateTime expirationTime = DateTime.Parse("5/1/2004 12:01:00");
            Assert.IsTrue(oneMinuteExpiration.IsExpired(baseTime, expirationTime));
        }

        [Test]
        public void SecondsAreRoundedOutBeforePerformingPerMinuteExpirations()
        {
            ExtendedFormat oneMinuteExpiration = new ExtendedFormat("* * * * *");
            DateTime baseTime = DateTime.Parse("5/1/2004 12:00:59");
            DateTime expirationTime = DateTime.Parse("5/1/2004 12:01:01");
            Assert.IsTrue(oneMinuteExpiration.IsExpired(baseTime, expirationTime));
        }
    }
}

#endif