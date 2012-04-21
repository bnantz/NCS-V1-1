//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    [TestFixture]
    public class LogFilterFixture : ConfigurationContextFixtureBase
    {
        private LoggingSettings settings;
        private LogFilter filter;
        private LogEntry log;
        private int originalMinPriority;

        [SetUp]
        public void SetUp()
        {
            settings = (LoggingSettings)Context.GetConfiguration(LoggingSettings.SectionName);
            originalMinPriority = settings.MinimumPriority;
            SetDenyAllExceptAllowedMode();

            CategoryFilterData cat = new CategoryFilterData();
            cat.Name = "foo";
            settings.CategoryFilters.Add(cat);

            filter = new LogFilter(new LoggingConfigurationView(Context));
            log = CommonUtil.GetDefaultLogEntry();
        }

        [TearDown]
        public void TearDown()
        {
            SetAllowAllExceptDenied();
            this.settings.MinimumPriority = originalMinPriority;
            this.settings.CategoryFilters.Remove("foo");
        }

        [Test]
        public void CategoryAllowFilterWithCategoryInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "foo";

            Assert.IsTrue(filter.CheckFilters(log));
        }

        [Test]
        public void CategoryAllowFilterWithCategoryNotInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "bar"; // unlisted category

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void CategoryDenyFilterWithCategoryInDenyList()
        {
            SetAllowAllExceptDenied();
            log.Category = "foo";
            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void CategoryDenyFilterWithCategoryNotInDenyList()
        {
            SetAllowAllExceptDenied();
            log.Category = "bar"; // unlisted category
            Assert.IsTrue(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityAboveMinAndCategoryInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "foo";
            log.Priority = settings.MinimumPriority + 1;

            Assert.IsTrue(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityAboveMinAndCategoryInDenyList()
        {
            SetAllowAllExceptDenied();
            log.Category = "foo";
            log.Priority = settings.MinimumPriority + 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityBelowMinAndCategoryInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "foo";
            log.Priority = settings.MinimumPriority - 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityBelowMinAndCategoryInDenyList()
        {
            settings.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            log.Category = "foo";
            log.Priority = settings.MinimumPriority - 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityBelowMinAndCategoryNotInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "bar";
            log.Priority = settings.MinimumPriority - 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityBelowMinAndCategoryNotInDenyList()
        {
            SetAllowAllExceptDenied();
            log.Category = "bar";
            log.Priority = settings.MinimumPriority - 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityAboveMinAndCategoryNotInAllowList()
        {
            SetDenyAllExceptAllowedMode();
            log.Category = "bar"; // unlisted category
            log.Priority = settings.MinimumPriority + 1;

            Assert.IsFalse(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityAboveMinAndCategoryNotInDenyList()
        {
            SetAllowAllExceptDenied();
            log.Category = "bar"; // unlisted category
            log.Priority = settings.MinimumPriority + 1;

            Assert.IsTrue(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityFilterWithExactlyTheMinimumPriority()
        {
            SetDenyAllExceptAllowedMode();
            log.Priority = settings.MinimumPriority;
            log.Category = "foo";
            Assert.IsTrue(filter.CheckFilters(log));
        }

        [Test]
        public void PriorityFilterWithZeroPriority()
        {
            SetDenyAllExceptAllowedMode();
            log.Priority = 0;
            log.Category = "foo";
            Assert.IsFalse(filter.CheckFilters(log));
        }

        private void SetAllowAllExceptDenied()
        {
            this.settings.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
        }

        private void SetDenyAllExceptAllowedMode()
        {
            this.settings.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
        }
    }
}

#endif