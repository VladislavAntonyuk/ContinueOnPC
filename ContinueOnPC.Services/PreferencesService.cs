namespace ContinueOnPC;

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

	public string Get(string key, string defaultValue = "")
	{
		return preferences.Get(key, defaultValue, "group.com.vladislavantonyuk.ContinueOnPC");
	}
}