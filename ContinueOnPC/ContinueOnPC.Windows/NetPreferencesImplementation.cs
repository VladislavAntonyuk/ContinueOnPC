using System;
using System.Configuration;
using System.Globalization;
using Xamarin.Essentials.Interfaces;

namespace ContinueOnPC.Windows;

internal class NetPreferencesImplementation : IPreferences
{
    private readonly Configuration configuration;
    private readonly KeyValueConfigurationCollection settings;

    public NetPreferencesImplementation()
    {
        var roaming = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
        var fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = roaming.FilePath
        };
        configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        settings = configuration.AppSettings.Settings;
    }

    public bool ContainsKey(string key)
    {
        return ContainsKey(key, "");
    }

    public bool ContainsKey(string key, string sharedName)
    {
        return settings[key] != null;
    }


    public DateTime Get(string key, DateTime defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : DateTime.Parse(settings[key].Value);
    }


    public string Get(string key, string defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public bool Get(string key, bool defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public int Get(string key, int defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public double Get(string key, double defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public float Get(string key, float defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public long Get(string key, long defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public string Get(string key, string defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value) ? defaultValue : settings[key].Value;
    }

    public bool Get(string key, bool defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : Convert.ToBoolean(settings[key].Value);
    }

    public int Get(string key, int defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : Convert.ToInt32(settings[key].Value);
    }

    public double Get(string key, double defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : Convert.ToDouble(settings[key]?.Value);
    }

    public float Get(string key, float defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : Convert.ToSingle(settings[key]?.Value);
    }

    public long Get(string key, long defaultValue, string sharedName)
    {
        return string.IsNullOrWhiteSpace(settings[key]?.Value)
            ? defaultValue
            : Convert.ToInt64(settings[key]?.Value);
    }

    public DateTime Get(string key, DateTime defaultValue)
    {
        return Get(key, defaultValue, "");
    }

    public void Set(string key, string value)
    {
        Set(key, value, "");
    }

    public void Set(string key, bool value)
    {
        Set(key, value, "");
    }

    public void Set(string key, int value)
    {
        Set(key, value, "");
    }

    public void Set(string key, double value)
    {
        Set(key, value, "");
    }

    public void Set(string key, float value)
    {
        Set(key, value, "");
    }

    public void Set(string key, long value)
    {
        Set(key, value, "");
    }

    public void Set(string key, DateTime value, string sharedName)
    {
        SetAndSave(key, value.ToLongDateString());
    }

    public void Set(string key, string value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Set(string key, bool value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Set(string key, int value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Set(string key, double value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Set(string key, float value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }

    public void Set(string key, DateTime value)
    {
        Set(key, value, "");
    }

    public void Set(string key, long value, string sharedName)
    {
        SetAndSave(key, value.ToString(CultureInfo.InvariantCulture));
    }


    public void Remove(string key, string sharedName)
    {
        settings.Remove(key);
    }

    public void Remove(string key)
    {
        Remove(key, "");
    }

    public void Clear()
    {
        Clear("");
    }

    public void Clear(string sharedName)
    {
        settings.Clear();
    }

    private void SetAndSave(string key, string value)
    {
        if (ContainsKey(key, ""))
        {
            settings[key].Value = value;
        }
        else
        {
            settings.Add(key, value);
        }

        configuration.Save(ConfigurationSaveMode.Full, true);
    }
}