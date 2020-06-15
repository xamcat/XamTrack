using Microsoft.Azure.Devices.Client;
using System.Threading;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public interface IIoTDeviceClientService
    {
        event ConnectionStatusChangesHandler ConnectionStatusChange;

        ConnectionStatus LastKnownConnectionStatus { get; }

        ConnectionStatusChangeReason LastKnownConnectionChangeReason { get; }

        Task<bool> Connect();


        Task<bool> InitialiseAsync(string IotHubEndpoint);

        Task SendEventAsync(Message message, CancellationToken cancellationToken);
    }
}
