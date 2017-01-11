using System;
using System.Reflection;
using Adic;
using Adic.Container;
using Adic.Exceptions;
using UnityEngine;

namespace Configunity.Adic
{
    public class ConfigurationContext
    {
        private IConfigStorage Storage;

        protected virtual ConfigSetting AddConfigOption(IInjectionContainer container, ConfigSettingType type,
            string settingName, object defaultValue)
        {
            ConfigSetting setting = Storage.Init(type, settingName, defaultValue);
            try
            {
                ConfigSetting bound = container.Resolve<ConfigSetting>(settingName);
                return bound;
            }
            catch (InjectorException ie)
            {
                // no bound instance found, bind this one
                container.Bind<ConfigSetting>().To(setting).As(settingName);
            }
            return setting;
        }

        public virtual void SetupConfigStorage(IInjectionContainer container)
        {
            DictionaryJsonConfigStorage concrete = new DictionaryJsonConfigStorage("mri.settings.json");
            CachingDelegatingConfigStorage caching = new CachingDelegatingConfigStorage(concrete);
            container.Bind<IConfigStorage>().To(caching);
        }

        public virtual void SetupBindings(IInjectionContainer container)
        {
            // Setup the IConfigStorage (writes to file with caching wrapper)
            Storage = container.Resolve<IConfigStorage>();
            AddBindings(container);
        }

        public virtual void AddComponentBinding(Component component, ConfigSetting configSetting, string fieldName)
        {
            FieldInfo field = component.GetType().GetField(fieldName);
            configSetting.Persisted += (sender, args) => { field.SetValue(component, configSetting.Value); };
        }

        public virtual void AddBindings(IInjectionContainer container)
        {
        }
    }
}