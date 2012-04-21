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
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>
    /// Provides a facade to configuration settings from defined storage in a specified configuration section. 
    /// </para>
    /// </summary> 
    internal class ConfigurationBuilder : IDisposable
    {
        private const string MachineConfigFilename = "machine.config";
        private const string MachineConfigSubdirectory = "Config";

        private static string machineConfigFile;

        internal string currentConfigFileName = string.Empty;

        private ConfigurationSettings configurationSettings;
        private ConfigurationFile configFile;

        private IConfigurationChangeWatcher metaConfigChangeWatcher;

        private ConfigurationSections sections;

        /// <summary>
        /// <para>Occurs before configuration is changed.</para>
        /// </summary>
        public event ConfigurationChangingEventHandler ConfigurationChanging;

        /// <summary>
        /// <para>Occurs after configuration is changed.</para>
        /// </summary>
        public event ConfigurationChangedEventHandler ConfigurationChanged;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="ConfigurationManager"/> class with a configuration file.
        /// </para>
        /// </summary>
        /// <param name="configurationFile">
        /// <para>
        /// The file where the configuration settings are defined.
        /// </para>
        /// </param>
        public ConfigurationBuilder(string configurationFile) 
        {
            ArgumentValidation.CheckForNullReference(configurationFile, "configurationFile");
            ArgumentValidation.CheckForEmptyString(configurationFile, "configurationFile");

            LoadMetaConfiguration(configurationFile);
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationManager"/> class with the preloaded configuration.</para>
        /// </summary>
        /// <param name="configurationSettings">
        /// <para>The preloaded configuration data to initialize the manager.</para>
        /// </param>
        public ConfigurationBuilder(ConfigurationSettings configurationSettings) 
        {
            ArgumentValidation.CheckForNullReference(configurationSettings, "configurationSettings");

            InitializeConfiguration(configurationSettings);
        }

        internal ConfigurationBuilder(ConfigurationDictionary dictionary) 
        {
            ArgumentValidation.CheckForNullReference(dictionary, "dictionary");

            this.configurationSettings = new ConfigurationSettings();
            
            InitializeBuilderCaches(dictionary);
            CloneConfigurationSettingsFromContextDictionary(dictionary);
            AddKeysToConfigurationSettingsSoTheyValidate(dictionary);
        }

        internal ConfigurationBuilder()
        {
            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                configurationFile = ConfigurationBuilder.MachineConfigurationFile;
            }
            LoadMetaConfiguration(configurationFile);
        }
        

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != configFile)
                {
                    configFile.Dispose();
                }
                if (null != metaConfigChangeWatcher)
                {
                    metaConfigChangeWatcher.Dispose();
                    metaConfigChangeWatcher = null;
                }
                if (null != sections)
                {
                    sections.Dispose();
                }
            }
        }

        internal ConfigurationSettings ConfigurationSettings
        {
            get { return this.configurationSettings; }
        }

        internal ConfigurationSections ConfigurationSections
        {
            get { return this.sections; }
        }

        /// <devdoc>
        /// Get the directory for current NDP
        /// </devdoc>
        private static string MsCorLibDirectory
        {
            get
            {
                // location of mscorlib.dll
                string filename = Assembly.GetAssembly(typeof(object)).Location.Replace('/', '\\');
                return Path.GetDirectoryName(filename);
            }
        }

        /// <devdoc>
        /// Get the machine configuration file.
        /// </devdoc>
        internal static string MachineConfigurationFile
        {
            get
            {
                if (null == machineConfigFile)
                {
                    machineConfigFile = Path.Combine(Path.Combine(MsCorLibDirectory, MachineConfigSubdirectory), MachineConfigFilename);
                }
                return machineConfigFile;
            }
        }

        /// <summary>
        /// <para>
        /// Reads configuration settings for a user-defined configuration section.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The configuration section to read.</para>
        /// </param>
        /// <returns>
        /// <para>The configuration settings for <paramref name="sectionName"/>.</para>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="sectionName"/>can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ConfigurationException">
        /// <para><paramref name="sectionName"/> is not valid for this configuration.</para>
        /// </exception>
        public object ReadConfiguration(string sectionName)
        {
            ValidateSection(sectionName);

            object configurationSection = sections.GetSection(sectionName);
            if (IsConfigurationSectionCached(configurationSection))
            {
                return configurationSection;
            }

            IStorageProviderReader storageProviderReader = CreateStorageProvider(sectionName);

            object configurationSettings = storageProviderReader.Read();
            if (configurationSettings == null)
            {
                return null;
            }

            ITransformer transformer = CreateTransformer(sectionName);
            if (transformer != null)
            {
                configurationSection = transformer.Deserialize(configurationSettings);
            }
            else
            {
                configurationSection = configurationSettings;
            }
  
            ConfigurationChangedEventHandler changed = new ConfigurationChangedEventHandler(OnExternalConfigurationChanged);
            sections.AddSection(sectionName, configurationSection, changed, storageProviderReader);
      
            return configurationSection;
        }

        /// <summary>
        /// <para>
        /// Write configuration for a section to storage.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section for the configuration data.</para>
        /// </param>
        /// <param name="configValue">
        /// <para>The configuration value to store.</para>
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para><paramref name="sectionName"/> is not valid section for this configuration.</para>
        /// <para>- or -</para>
        /// <para>The section data is read only.</para>
        /// <para>- or -</para>
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        public void WriteConfiguration(string sectionName, object configValue)
        {
            ValidateSection(sectionName);
            ConfigurationChangingEventArgs args = CreateConfigurationChangingEventArgs(sectionName, configValue);
            OnConfigurationChanging(args);
            if (!args.Cancel)
            {
                IStorageProviderWriter configStorageWriter = GetConfigurationStorageWriter(sectionName);
                object writeData = GetSerializedDataToWrite(sectionName, configValue);
                ConfigurationWriterActionCommand writerActionCommand = new ConfigurationWriterActionCommand(configStorageWriter, writeData);
                sections.UpdateSection(sectionName, writerActionCommand, configValue);

                ConfigurationChangedEventArgs changedArgs = new ConfigurationChangedEventArgs(configFile.FileName, sectionName);
                OnConfigurationChanged(changedArgs);
            }
        }

        /// <summary>
        /// <para>
        /// Write the meta configuration for the configuration manager to the configuration file.
        /// </para>
        /// </summary>
        /// <param name="configurationSettings">
        /// The meta configuration to write to configuration.
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        public void WriteMetaConfiguration(ConfigurationSettings configurationSettings)
        {
            if (UserAcceptsMetaConfigurationChange(configurationSettings) == false)
            {
                return;
            }
            SaveMetaConfigurationChanges(configurationSettings);
        }

        /// <summary>
        /// <para>
        /// Write the meta configuration for the configuration manager to the configuration file.
        /// </para>
        /// </summary>
        /// <param name="configurationSectionData">
        /// The meta configuration to write to configuration.
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>The configuration section already exists.</para>
        /// </exception>
        public void WriteMetaConfig(ConfigurationSectionData configurationSectionData)
        {
            configurationSettings.ConfigurationSections.Add(configurationSectionData);
            if (UserAcceptsMetaConfigurationChange(configurationSettings) == true)
            {
                SaveMetaConfigurationChanges(configurationSettings);
            }
            else
            {
                configurationSettings.ConfigurationSections.Remove(configurationSectionData.Name);
            }
        }

        /// <summary>
        /// <para>
        /// Read the meta configuration for the configuration manager.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// The meta configuration for the configuration manager or null if no entries exists.
        /// </para>
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the meta configuration.</para>
        /// </exception>
        public ConfigurationSettings ReadMetaConfiguration()
        {
            if (this.configFile != null)
            {
                return this.configFile.GetConfig(ConfigurationSettings.SectionName) as ConfigurationSettings;
            }
            else
            {
                return this.configurationSettings;
            }
        }

        /// <summary>
        /// <para>
        /// Read the meta configuration for the configuration manager.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the configuration section to retrieve.    </para>
        /// </param>
        /// <returns>
        /// <para>
        /// The meta configuration for the configuration manager or null if no entries exists.
        /// </para>
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the meta configuration.</para>
        /// </exception>
        public ConfigurationSectionData ReadMetaConfiguration(string sectionName)
        {
            ConfigurationSettings settings = ReadMetaConfiguration();
            if (null == settings)
            {
                return null;
            }
            return settings.ConfigurationSections[sectionName];
        }

        /// <summary>
        /// <para>
        /// Gets a value indicating whether a section in configuration is read-only.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>
        /// The section in the configuration.
        /// </para>
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the configuration section is read-only; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> can not be <see langword="null"/>.</para>
        /// </exception>		
        public bool IsReadOnly(string sectionName)
        {
            ValidateSectionNameNotNull(sectionName);
            IStorageProviderReader configProvider = CreateStorageProvider(sectionName);
            if (configProvider is IStorageProviderWriter)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// <para>Determines if a section name is valid in the current configuration file.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to validate.</para>
        /// </param>
        /// <returns><see langword="true"/> if the section name is valid; otherwise, <see langword="false"/></returns>
        public bool IsValidSection(string sectionName)
        {
            if (this.configurationSettings == null)
            {
                return false;
            }
            ConfigurationSectionData configurationSection = this.configurationSettings.ConfigurationSections[sectionName];
            return (configurationSection != null);
        }

        /// <summary>
        /// <para>Removes a section from the internal cache.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The section name to remove.</para>
        /// </param>
        public void ClearSectionCache(string sectionName)
        {
            sections.Remove(sectionName);
        }

        /// <summary>
        /// <para>Removes all sections from the internal cache.</para>
        /// </summary>
        public void ClearSectionCache()
        {
            sections.ClearAll();
        }

        /// <summary>
        /// <para>Raises the <seealso cref="ConfigurationChanging"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ConfigurationChangingEventArgs"/> that contains the event data.</para>
        /// </param>
        protected virtual void OnConfigurationChanging(ConfigurationChangingEventArgs e)
        {
            if (null != ConfigurationChanging)
            {
                ConfigurationChanging(this, e);
            }
        }

        /// <summary>
        /// <para>Raises the <seealso cref="ConfigurationChanged"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>A <see cref="ConfigurationChangedEventArgs"/> that contains the event data.</para>
        /// </param>
        protected virtual void OnConfigurationChanged(ConfigurationChangedEventArgs e)
        {
            if (null != ConfigurationChanged)
            {
                ConfigurationChanged(this, e);
            }
        }

        /// <devdoc>
        /// Determines if the cache contains a section. Used by tests
        /// </devdoc>
        internal bool CacheContains(string sectionName)
        {
            return sections.ContainsSection(sectionName);
        }
        
        /// <summary>
        /// <para>Gets the <see cref="ConfigurationContext"/> for the current <see cref="ConfigurationManager"/>.</para>
        /// </summary>
        /// <returns><para>A <see cref="ConfigurationContext"/>.</para></returns>
        private ConfigurationContext CreateContext()
        {
            return new ConfigurationContext(new NonDisposingWrapper(this));
        }

        private ITransformer CreateTransformer(string sectionName)
        {
            using (ConfigurationContext configurationContext = CreateContext())
            {
                TransformerFactory factory = new TransformerFactory(configurationContext);
                return factory.Create(sectionName);
            }
        }

        private IStorageProviderReader CreateStorageProvider(string sectionName)
        {
            using (ConfigurationContext configurationContext = CreateContext())
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                return factory.Create(sectionName);
            }
        }
        
        /// <devdoc>
        /// Serialize the block configuration into an XmlNode.
        /// </devdoc>
        private XmlNode Serialize(ConfigurationSettings configurationSettings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationSettings), GetXmlIncludeTypes(configurationSettings));
            using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(sw, configurationSettings);
                XmlTextReader reader = new XmlTextReader(new StringReader(sw.ToString()));
                reader.MoveToContent();
                XmlDocument doc = new XmlDocument();
                return doc.ReadNode(reader);
            }
        }

        private static Type[] GetXmlIncludeTypes(ConfigurationSettings configurationSettings)
        {
            Type[] types = new Type[configurationSettings.XmlIncludeTypes.Count];
            int index = 0;
            foreach (XmlIncludeTypeData includeType in configurationSettings.XmlIncludeTypes)
            {
                types[index++] = Type.GetType(includeType.TypeName, true);
            }
            return types;
        }

        private IStorageProviderWriter GetConfigurationStorageWriter(string sectionName)
        {
            IStorageProviderReader configStorageReader = CreateStorageProvider(sectionName);
            IStorageProviderWriter configStorageWriter = configStorageReader as IStorageProviderWriter;
            if (configStorageWriter == null)
            {
                throw new ConfigurationException(SR.ExceptionHandlerNotWritable(sectionName));
            }
            return configStorageWriter;
        }

        private object GetSerializedDataToWrite(string sectionName, object configValue)
        {
            object writeData = configValue;
            ITransformer transformer = CreateTransformer(sectionName);
            if (null != transformer)
            {
                writeData = transformer.Serialize(configValue);
            }
            return writeData;
        }

        private ConfigurationChangingEventArgs CreateConfigurationChangingEventArgs(string sectionName, object configValue)
        {
            object oldValue = null;
            if (IsValidSection(sectionName))
            {
                // the value may have not been written yet.
                try
                {
                    oldValue = ReadConfiguration(sectionName);
                }
                catch(ConfigurationException)
                {}
                catch (CryptographicException)
                {}
            }
            return new ConfigurationChangingEventArgs(this.configFile.FileName, sectionName, oldValue, configValue);
        }

        private void LoadMetaConfiguration(string configurationFile)
        {
            configFile = new ConfigurationFile();
            string machineFilename = MachineConfigurationFile;
            // make sure we don't load the machine file twice
            if (string.Compare(configurationFile, machineFilename, true, CultureInfo.InvariantCulture) != 0)
            {
                ConfigurationFile machineConfigFile = new ConfigurationFile();
                bool machineFileExists = machineConfigFile.Load(machineFilename);
                if (machineFileExists)
                {
                    configFile = new ConfigurationFile(machineConfigFile);
                }
            }

            configFile.Load(configurationFile);

            CreateMetaConfigChangeWatcher();

            this.currentConfigFileName = configurationFile;
            InitializeConfiguration(ReadMetaConfiguration());
        }

        private void CreateMetaConfigChangeWatcher()
        {
            if (metaConfigChangeWatcher != null)
            {
                metaConfigChangeWatcher.Dispose();
            }
            metaConfigChangeWatcher = configFile.CreateFileWatcher();
            metaConfigChangeWatcher.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMetaConfigurationChanged);
            metaConfigChangeWatcher.StartWatching();
        }

        private void ValidateSection(string sectionName)
        {
            if (!IsValidSection(sectionName))
            {
                InvalidSectionExceptionBuilder builder = new InvalidSectionExceptionBuilder(sectionName, this.currentConfigFileName);
                throw builder.ThrowException();
            }
        }

        private static void ValidateSectionNameNotNull(string sectionName)
        {
            ArgumentValidation.CheckForNullReference(sectionName, "sectionName");
            ArgumentValidation.CheckForEmptyString(sectionName, "sectionName");
        }

        private void InitializeConfiguration(ConfigurationSettings configurationSettings)
        {
            this.configurationSettings = configurationSettings;

            InitializeBuilderCaches(null);
        }

        private void InitializeBuilderCaches(ConfigurationDictionary dictionary)
        {
            if (sections == null)
            {
                sections = new ConfigurationSections(dictionary);
            }
            else
            {
                sections.ClearAll();
            }
        }

        private void SaveMetaConfigurationChanges(ConfigurationSettings configurationSettings)
        {
            if (configFile != null)
            {
                AddConfigurationToConfigFile(configurationSettings);
            }
            OnConfigurationChanged(new ConfigurationChangedEventArgs(this.configFile != null ? this.configFile.FileName : string.Empty, ConfigurationSettings.SectionName));
            // reload the data
            InitializeConfiguration(configurationSettings);
        }

        private void AddConfigurationToConfigFile(ConfigurationSettings configurationSettings)
        {
            metaConfigChangeWatcher.StopWatching();
            using (new ConfigurationChangeWatcherPauser(metaConfigChangeWatcher))
            {
                XmlNode configNode = this.Serialize(configurationSettings);
                this.configFile.AppendSection(ConfigurationSettings.SectionName, typeof(ConfigurationManagerSectionHandler).AssemblyQualifiedName, configNode);
            }
        }

        private bool UserAcceptsMetaConfigurationChange(ConfigurationSettings configurationSettings)
        {
            ConfigurationChangingEventArgs args = new ConfigurationChangingEventArgs(this.configFile.FileName, ConfigurationSettings.SectionName, this.configurationSettings, configurationSettings);
            OnConfigurationChanging(args);
            return args.Cancel == false;
        }

        private void OnExternalConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            sections.Remove(args.SectionName);
            OnConfigurationChanged(sender, args);
        }

        private void OnMetaConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            LoadMetaConfiguration(args.ConfigurationFile);
            OnConfigurationChanged(sender, args);
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            if (null != ConfigurationChanged)
            {
                ConfigurationChanged(sender, args);
            }
        }

        private bool IsConfigurationSectionCached(object configurationSection)
        {
            return configurationSection != null;
        }

        private void AddKeysToConfigurationSettingsSoTheyValidate(ConfigurationDictionary dictionary)
        {
            foreach (string key in dictionary.Keys)
            {
                if (string.Compare(key, ConfigurationSettings.SectionName, true, CultureInfo.InvariantCulture) != 0)
                {
                    if (!configurationSettings.ConfigurationSections.Contains(key))
                    {
                        configurationSettings.ConfigurationSections.Add(new ConfigurationSectionData(key));
                    }
                }
            }
        }

        private void CloneConfigurationSettingsFromContextDictionary(ConfigurationDictionary dictionary)
        {
            // we need to make sure to check if the meta config is in the dictionary and clone it
            // so we get our own copy of the data
            if (dictionary.Contains(ConfigurationSettings.SectionName))
            {
                ConfigurationSettings settings = dictionary[ConfigurationSettings.SectionName] as ConfigurationSettings;
                configurationSettings.KeyAlgorithmPairStorageProviderData = (KeyAlgorithmPairStorageProviderData)((null != settings.KeyAlgorithmPairStorageProviderData) ? settings.KeyAlgorithmPairStorageProviderData.Clone() : null);
                foreach (ConfigurationSectionData data in settings.ConfigurationSections)
                {
                    ConfigurationSectionData cloneData = new ConfigurationSectionData(data.Name, data.Encrypt,
                        (null != data.StorageProvider) ? (StorageProviderData)data.StorageProvider.Clone() : null,
                        (null != data.Transformer) ? (TransformerData)data.Transformer.Clone() : null);
                    configurationSettings.ConfigurationSections.Add(cloneData);
                }
                sections.Remove(ConfigurationSettings.SectionName);
            }
        }

        private class ConfigurationWriterActionCommand : IActionCommand
        {
            private readonly object configurationData;
            private readonly IStorageProviderWriter writer;

            public ConfigurationWriterActionCommand(IStorageProviderWriter writer, object configurationData)
            {
                this.writer = writer;
                this.configurationData = configurationData;
            }

            public void Execute()
            {
                writer.Write(configurationData);
            }
        }
    }
}