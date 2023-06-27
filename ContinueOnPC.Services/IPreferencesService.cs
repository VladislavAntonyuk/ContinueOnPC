namespace ContinueOnPC;

public interface IPreferencesService
{
	string Get(string key, string defaultValue = "");
	void Save(string key, string value);
}