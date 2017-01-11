using System;

namespace Configunity
{
    public delegate void ConfigSettingValueChangedEventHandler(ConfigSetting sender, EventArgs e);

    public delegate void ConfigSettingValuePersistedEventHandler(ConfigSetting sender, EventArgs e);

    [Serializable]
    public class ConfigSetting
    {
        public string Name;
        private ConfigSettingType _type;

        public ConfigSettingType Type
        {
            get { return _type; }
        }

        private object _value;

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnChanged();
            }
        }

        public ConfigSetting(ConfigSettingType type, string name = "", object value = null)
        {
            this._type = type;
            this.Name = name;
            this._value = value;
        }

        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ConfigSettingValueChangedEventHandler Changed;

        // Notification when the value has not only been changed, but persisted
        public event ConfigSettingValuePersistedEventHandler Persisted;

        // Invoke the Changed event; called whenever list changes
        public virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        // Invoke the Changed event; called whenever list changes
        public virtual void OnPersisted()
        {
            Persisted?.Invoke(this, EventArgs.Empty);
        }
    }
}