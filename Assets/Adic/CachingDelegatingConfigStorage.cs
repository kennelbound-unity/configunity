using System.Collections.Generic;

namespace Configunity.Adic
{
    public class CachingDelegatingConfigStorage : IConfigStorage
    {
        private readonly IConfigStorage _delegate;
        private readonly Dictionary<string, ConfigSetting> _cache = new Dictionary<string, ConfigSetting>();

        public CachingDelegatingConfigStorage(IConfigStorage delegateStorage)
        {
            _delegate = delegateStorage;
        }

        public void Set(ConfigSetting config)
        {
            _delegate
                .Set(
                    config); // Update the delegate before updating our cache so that further calls will get the new value
            if (_cache.ContainsKey(config.Name))
            {
                ConfigSetting cached = _cache[config.Name];
                cached.Value = config.Value; // To update listeners
            }
            else
            {
                // No need to update listeners since nothing is likely to be bound
                _cache[config.Name] = config;
            }
        }

        public ConfigSetting Get(string name)
        {
            ConfigSetting setting = _cache[name];
            if (setting == null)
            {
                setting = _delegate.Get(name);
                _cache[name] = setting;
            }
            return setting;
        }

        public ConfigSetting Init(ConfigSettingType type, string name, object value)
        {
            return _delegate.Init(type, name, value);
        }
    }
}