using Xamarin.Essentials.Interfaces;

namespace ContinueOnPC
{
    public class PreferencesService : IPreferencesService
    {
        private readonly IPreferences preferences;

        public PreferencesService(IPreferences preferences)
        {
            this.preferences = preferences;
        }
        public void Save(string key, string value)
        {
            preferences.Set(key, value, "group.com.vladislavantonyuk.ContinueOnPC");
        }

        public string Get(string key)
        {
            return preferences.Get(key, "", "group.com.vladislavantonyuk.ContinueOnPC");
        }
    }
}

