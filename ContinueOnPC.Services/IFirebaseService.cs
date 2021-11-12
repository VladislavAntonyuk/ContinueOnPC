using ContinueOnPC.Models;
using System;
using System.Threading.Tasks;

namespace ContinueOnPC
{
    public interface IFirebaseService
    {
        Task PublishDataAsync(string uri);
        Task<IDisposable> SubscribeDataAsync(Func<LinkInfo, Task> progress);
    }
}