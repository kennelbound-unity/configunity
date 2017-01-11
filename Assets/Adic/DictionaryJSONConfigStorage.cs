using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Configunity.Adic
{
    public class DictionaryJsonConfigStorage : BaseConfigStorage
    {
        private readonly string _filename;

        public DictionaryJsonConfigStorage(string filename)
        {
            _filename = filename;
            try
            {
                _values = JsonConvert
                    .DeserializeObject<Dictionary<string, ConfigSetting>>(File.ReadAllText(_filename));
            }
            catch (Exception e)
            {
                // File does not exist or trouble reading.  Go with default values.
                _values = new Dictionary<string, ConfigSetting>();
            }
        }

        public void Set(ConfigSetting config)
        {
            _values[config.Name] = config;
            Save(config);
        }

        public ConfigSetting Get(string name)
        {
            return _values[name];
        }

        protected override void SaveAll()
        {
            File.WriteAllText(_filename, JsonConvert.SerializeObject(_values));
        }

        protected override void Save(ConfigSetting setting)
        {
            // TODO: Figure out how to persist just a single unit
            SaveAll();
        }
    }
}