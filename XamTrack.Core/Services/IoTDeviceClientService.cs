using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace XamTrack.Core.Services
{
    public class IoTDeviceClientService : IIoTDeviceClientService
    {
        DeviceClient _deviceClient;

        #region IIoTDeviceClientService
        public ConnectionStatus LastKnownConnectionStatus => throw new NotImplementedException();

        public ConnectionStatusChangeReason LastKnownConnectionChangeReason => throw new NotImplementedException();

        public event ConnectionStatusChangesHandler ConnectionStatusChange;


        public IoTDeviceClientService()
        {
            _deviceClient = DeviceClient.CreateFromConnectionString($"HostName=xt-iothub.azure-devices.net;DeviceId=BenDevice123;SharedAccessKey=4ilFd8aBNjU9OmpjPVtdYrlKtTLDv3NBogpNep41+P0=");
        }

        public Task<bool> Connect()
        {
            _deviceClient = DeviceClient.CreateFromConnectionString($"HostName=xt-iothub.azure-devices.net;DeviceId=BenDevice123;SharedAccessKey=4ilFd8aBNjU9OmpjPVtdYrlKtTLDv3NBogpNep41+P0=");
            return Task.FromResult(true);
        }

        public async Task<bool> Disconnect()
        {
            await _deviceClient.CloseAsync();
            return true;
        }

        public Task<bool> InitialiseAsync(string IotHubEndpoint)
        {
            throw new NotImplementedException();
        }

        public async Task SendEventAsync(string message, CancellationToken cancellationToken)
        {
            var msg = new Message(Encoding.ASCII.GetBytes(message));

            await _deviceClient.SendEventAsync(msg);
        }
        #endregion

        private async Task<bool> Provision()
        {
            var iotDpsGlobalEndpoint = AppConfigService.Settings["DpsGlobalEndpoint"];            
            var iotDpsIdScope = AppConfigService.Settings["DpsIdScope"]; ;
            /* var provisioningSharedKey = _appConfigProvider.AppConfig.DpsSymKeySecret;
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
            return true;
        }


        private string CreateSasToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
            return sasToken;
        }

    }


}
