using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XamTrack.Core.Services
{
    public class IoTDeviceClientService : IIoTDeviceClientService
    {
        DeviceClient _deviceClient;

        #region IIoTDeviceClientService
        public ConnectionStatus LastKnownConnectionStatus => throw new NotImplementedException();

        public ConnectionStatusChangeReason LastKnownConnectionChangeReason => throw new NotImplementedException();

        public event ConnectionStatusChangesHandler ConnectionStatusChange;

        public Task<bool> Connect()
        {
            return Task.FromResult(true);
        }

        public Task<bool> InitialiseAsync(string IotHubEndpoint)
        {
            throw new NotImplementedException();
        }

        public Task SendEventAsync(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        private async Task<bool> Provision()
        {
            var iotDpsGlobalEndpoint = AppConfigService.Settings["DpsGlobalEndpoint"];
            return true;
            /*var iotDpsIdScope = _appConfigProvider.AppConfig.DpsScopeId;
            var provisioningSharedKey = _appConfigProvider.AppConfig.DpsSymKeySecret;
            var dpsSymmKey = _symmetricKeyProvider.GenerateSymmetricKey(IotDeviceId, provisioningSharedKey);

            var provisioningClient = ProvisioningDeviceClient.Create(iotDpsGlobalEndpoint, iotDpsIdScope,
                new SecurityProviderSymmetricKey(IotDeviceId, dpsSymmKey, dpsSymmKey),
                new ProvisioningTransportHandlerHttp());

            var regResult = await provisioningClient.RegisterAsync();

            if (regResult.Status == ProvisioningRegistrationStatusType.Assigned)
            {
                _iotHubEndpoint = regResult.AssignedHub;
                await _iotHubSecretsRepository.SetIotHubEndpointAsync(_iotHubEndpoint);
                await _iotHubSecretsRepository.SetSymmetricKeyAsync(dpsSymmKey);
            }

            return await InitialiseAsync();*/
        }

    }
}
