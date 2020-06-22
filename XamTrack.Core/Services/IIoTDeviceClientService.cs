using System;
using System.Threading;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public interface IIoTDeviceClientService
    {        
        event EventHandler<string> ConnectionStatusChanged;
        string ConnectionStatus { get; }

        Task<bool> Connect();

        Task<bool> Disconnect();

        Task<bool> InitialiseAsync(string IotHubEndpoint);

        Task SendEventAsync(string message, CancellationToken cancellationToken);
    }
}
