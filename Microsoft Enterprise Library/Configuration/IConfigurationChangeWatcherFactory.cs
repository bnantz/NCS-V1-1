using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// Responsible for defining the interface to be supported by classes that are going to 
    /// create <see cref="IConfigurationChangeWatcher"></see>s
    /// </summary>
    public interface IConfigurationChangeWatcherFactory
    {
        /// <summary>
        /// <para>
        /// When implemented by a class, creates an object that is responsible for watching for
        /// changes in the underlying storage mechanism for configuration persistence. When a change
        /// occurs, this object must raise its ConfigurationChange event.
        /// </para>
        /// </summary>
        /// <returns>An initialized object that will watch for configuration changes.</returns>
        IConfigurationChangeWatcher CreateConfigurationChangeWatcher();
    }
}