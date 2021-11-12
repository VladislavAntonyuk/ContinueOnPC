namespace ContinueOnPC
{
    public interface IPreferencesService
    {
        string Get(string key);
        void Save(string key, string value);
    }
}