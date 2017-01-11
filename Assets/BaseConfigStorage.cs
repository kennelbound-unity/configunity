using System;
using System.Collections.Generic;
using System.IO;
using Adic;
using Configunity;
using Newtonsoft.Json;

namespace Configunity
{
    public abstract class BaseConfigStorage : IConfigStorage
    {
        protected Dictionary<string, ConfigSetting> _values;

        public virtual ConfigSetting Init(ConfigSettingType type, string name, object value)
        {
            if (!_values.ContainsKey(name))
            {
                _values[name] = new ConfigSetting(type, name, value);
            }
            return _values[name];
        }

        public virtual void Set(ConfigSetting config)
        {
            _values[config.Name] = config;
            Save(config);
            config.OnPersisted();
        }

        public virtual ConfigSetting Get(string name)
        {
            return _values[name];
        }

        protected abstract void SaveAll();

        protected abstract void Save(ConfigSetting setting);
    }
}