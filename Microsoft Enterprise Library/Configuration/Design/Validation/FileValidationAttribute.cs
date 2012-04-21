//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>Validates that a file can be created or is writable.</para>
    /// <remarks>
    /// <para>This validation assumes that the property is a file.</para>
    /// </remarks>
    /// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
	public sealed class FileValidationAttribute : ValidationAttribute
	{
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileValidationAttribute"/> class.</para>
        /// </summary>
		public FileValidationAttribute()
		{
		}

        /// <summary>
        /// <para>Validate the ranige data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.</para>
        /// </summary>
        /// <param name="instance">
        /// <para>The instance to validate.</para>
        /// </param>
        /// <param name="propertyInfo">
        /// <para>The property contaning the value to validate.</para>
        /// </param>
        /// <param name="errors">
        /// <para>The collection to add any errors that occur durring the validation.</para>
        /// </param>
        public override void Validate(object instance, PropertyInfo propertyInfo, ValidationErrorCollection errors)
        {
            ArgumentValidation.CheckForNullReference(instance, "instance");
            ArgumentValidation.CheckForNullReference(propertyInfo, "propertyInfo");
            ArgumentValidation.CheckForNullReference(errors, "errors");
            
            object propertyValue = propertyInfo.GetValue(instance, null);
            string fileName = propertyValue as string;
            // I have to assume this is ok
            if ((fileName == null) || (fileName.Length == 0)) return;


            DetermineIfCanCreateFile(fileName, errors, instance, propertyInfo);
            DetermineIfCanWriteFile(fileName, errors, instance, propertyInfo);
        }

	    private void DetermineIfCanWriteFile(string fileName, ValidationErrorCollection errors, object instance, PropertyInfo info)
	    {
            if (File.Exists(fileName))
            {
                try
                {
                    using(File.OpenWrite(fileName))
                    {}
                }
                catch (IOException e)
                {
                    errors.Add(new ValidationError(instance, info.Name, e.Message));
                }
                catch (UnauthorizedAccessException e)
                {
                    errors.Add(new ValidationError(instance, info.Name, e.Message));
                }
                catch (ArgumentException e)
                {
                    errors.Add(new ValidationError(instance, info.Name, e.Message));
                }
            }
	    }

	    private static void DetermineIfCanCreateFile(string fileName, ValidationErrorCollection errors, object instance, PropertyInfo info)
	    {
	        if (!File.Exists(fileName))
	        {
                bool createdDirectory = false;
                string directory = string.Empty;
	            try
	            {
                    directory = Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    using(File.Create(fileName))
                    {}
                }
	            catch (IOException e)
	            {
	                errors.Add(new ValidationError(instance, info.Name, e.Message));
	            }
                catch (UnauthorizedAccessException e)
                {
                    errors.Add(new ValidationError(instance, info.Name, e.Message));
                }
                catch (ArgumentException e)
                {
                    errors.Add(new ValidationError(instance, info.Name, e.Message));
                }
	            finally
	            {
	                try
	                {
	                    File.Delete(fileName);
                        if (createdDirectory)
                        {
                            Directory.Delete(directory);        
                        }
	                }
	                catch(IOException) {}
                    catch (UnauthorizedAccessException) {}
	            }
	        }
	    }
	}
}
