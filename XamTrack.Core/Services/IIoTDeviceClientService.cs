using System;
using System.Threading;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public interface IIoTDeviceClientService
    {        
        event EventHandler<string> ConnectionStatusChanged;
        string ConnectionStatus { get; }

        Task<bool> ConnectAsync();

        Task<bool> DisconnectAsync();

        Task SendEventAsync(string message);
    }
}
