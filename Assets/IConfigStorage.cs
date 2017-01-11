using System.Collections.Generic;

namespace Configunity
{
    public interface IConfigStorage
    {
        ConfigSetting Init(ConfigSettingType type, string name, object value);
        void Set(ConfigSetting config);
        ConfigSetting Get(string name);
    }
}