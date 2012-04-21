using System;
using System.Collections;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    internal class ConfigurationSections : IDisposable
    {
        private Hashtable cache;
        private Hashtable configurationWatcherCache;
        private object lockMe = new object();

        private Hashtable Cache
        {
            get { return cache; }
            set { cache = value; }
        }

        private Hashtable ConfigurationWatcherCache
        {
            get { return configurationWatcherCache; }
            set { configurationWatcherCache = value; }
        }

        public delegate void ActionTakenWhilePaused();

        public ConfigurationSections(ConfigurationDictionary dictionary)
        {
            if (dictionary == null)
            {
                Cache = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
            }
            else
            {
                Cache = new Hashtable(dictionary, new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
            }
            ConfigurationWatcherCache = new Hashtable(new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
        }

        ~ConfigurationSections()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (IConfigurationChangeWatcher watcher in ConfigurationWatcherCache.Values)
            {
                watcher.Dispose();
            }

            ConfigurationWatcherCache.Clear();
        }

        public void Remove(string sectionName)
        {
            if (null == sectionName)
            {
                return;
            }

            lock (lockMe)
            {
                Cache.Remove(sectionName);
                IConfigurationChangeWatcher watcher = (IConfigurationChangeWatcher)ConfigurationWatcherCache[sectionName];
                if (watcher != null)
                {
                    ConfigurationWatcherCache.Remove(sectionName);
                    watcher.Dispose();
                }
            }
        }

        public void ClearAll()
        {
            lock (lockMe)
            {
                Cache.Clear();
                foreach (IConfigurationChangeWatcher changeWatcher in ConfigurationWatcherCache.Values)
                {
                    changeWatcher.Dispose();
                }
                ConfigurationWatcherCache.Clear();
            }
        }

        public object GetSection(string sectionName)
        {
            lock (lockMe)
            {
                return Cache[sectionName];
            }
        }

        public void AddSection(string sectionName, object configurationSection, ConfigurationChangedEventHandler changed, IConfigurationChangeWatcherFactory factory)
        {
            lock (lockMe)
            {
                if (DoesWatcherExistFor(sectionName) == false)
                {
                    IConfigurationChangeWatcher watcher = factory.CreateConfigurationChangeWatcher();
                    AddConfigurationWatcherForSection(changed, watcher);
                }

                AddSection(sectionName, configurationSection);
            }
        }

        public bool ContainsSection(string sectionName)
        {
            lock (lockMe)
            {
                return Cache.Contains(sectionName);
            }
        }

        public void UpdateSection(string sectionName, IActionCommand actionCommand, object configValue)
        {
            lock (lockMe)
            {
                ExecuteWhilePaused(sectionName, new ActionTakenWhilePaused(actionCommand.Execute));
                AddSection(sectionName, configValue);
            }
        }

        private void ExecuteWhilePaused(string sectionName, ActionTakenWhilePaused action)
        {
            IConfigurationChangeWatcher sectionWatcher = (IConfigurationChangeWatcher)configurationWatcherCache[sectionName];
            using(new ConfigurationChangeWatcherPauser(sectionWatcher))
            {
                action();
            }
        }

        private void AddSection(string sectionName, object configurationSectionSettings)
        {
            Cache[sectionName] = configurationSectionSettings;
        }

        private bool DoesWatcherExistFor(string sectionName)
        {
            return ConfigurationWatcherCache.Contains(sectionName);
        }

        private void AddConfigurationWatcherForSection(ConfigurationChangedEventHandler changed, IConfigurationChangeWatcher watcher)
        {
            watcher.ConfigurationChanged += changed;
            watcher.StartWatching();
            ConfigurationWatcherCache.Add(watcher.SectionName, watcher);
        }
    }
}
