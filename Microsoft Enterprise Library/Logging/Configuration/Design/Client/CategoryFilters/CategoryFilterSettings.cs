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

using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters
{
    /// <summary>
    /// Contains category filter settings.
    /// </summary>
    public struct CategoryFilterSettings
    {
        private CategoryFilterMode categoryFilterMode;

        private CategoryFilterDataCollection categoryFilters;

        /// <summary>
        /// Gets and sets the mode in which to filter.
        /// </summary>
        public CategoryFilterMode CategoryFilterMode
        {
            get { return categoryFilterMode; }
            set { categoryFilterMode = value; }
        }

        /// <summary>
        /// Gets and sets the collection of categories to filter.
        /// </summary>
        public CategoryFilterDataCollection CategoryFilters
        {
            get { return categoryFilters; }
            set { categoryFilters = value; }
        }

        /// <summary>
        /// Creates a new <c>CategoryFilterSettings</c> object with initial settings.
        /// </summary>
        /// <param name="categoryFilterMode">The mode in which to filter.</param>
        /// <param name="categoryFilters">The collection of categories to filter.</param>
        public CategoryFilterSettings(CategoryFilterMode categoryFilterMode, CategoryFilterDataCollection categoryFilters)
        {
            this.categoryFilterMode = categoryFilterMode;
            this.categoryFilters = categoryFilters;
        }

        /// <summary>
        /// Returns a user friendly formatted string which illustrates the filter settings.
        /// </summary>
        /// <returns>See summary.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (CategoryFilterMode == CategoryFilterMode.AllowAllExceptDenied)
            {
                sb.Append(SR.CategoryFilterSummaryAllow);
            }
            else
            {
                sb.Append(SR.CategoryFilterSummaryDeny);
            }
            sb.Append(": ");

            bool first = true;
            foreach (CategoryFilterData categoryFilter in CategoryFilters)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.Append(categoryFilter.Name);
                first = false;
            }

            return sb.ToString();
        }
    }
}