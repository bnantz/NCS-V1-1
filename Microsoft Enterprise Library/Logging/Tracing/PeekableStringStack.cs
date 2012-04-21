//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tracing
{
	internal class PeekableStringStack : ArrayList
	{
		public string PeekAt(int index)
		{
			if (index < base.Count && index >= 0)
			{
				return (string) base[index];
			}
			else
			{
				return null;
			}
		}

		public string Peek()
		{
			if (base.Count > 0)
			{
				return (string) base[Count - 1];
			}
			else
			{
				return null;
			}
		}

		public void Pop()
		{
			if (base.Count > 0)
			{
				base.RemoveAt(base.Count - 1);
			}
		}

		public void Push(string category)
		{
			base.Add(category);
		}
	}
}