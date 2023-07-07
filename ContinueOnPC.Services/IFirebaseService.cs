namespace ContinueOnPC.Services;

using Models;

public interface IFirebaseService
{
	Task<HasErrorResult<bool>> PublishDataAsync(string uri);
	Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> onReceived, Func<Exception, Task> onError);
}