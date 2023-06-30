namespace ContinueOnPC.Services;

using ContinueOnPC.Models;

public interface IFirebaseService
{
	Task<bool> ValidateConnection();
	Task<bool> PublishDataAsync(string uri);
	Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> progress);
}