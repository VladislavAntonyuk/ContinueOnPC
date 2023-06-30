namespace ContinueOnPC.Services;

public interface IPreferencesService
{
	string Get(string key, string defaultValue = "");
	void Save(string key, string value);
}