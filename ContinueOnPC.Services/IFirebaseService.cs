using ContinueOnPC.Models;

namespace ContinueOnPC;

public interface IFirebaseService
{
    Task PublishDataAsync(string uri);
    Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> progress);
}