using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private DeviceClient _deviceClient;
        private IAppConfigService _appConfigService;
        private IDeviceInfoService _deviceInfoService;
        private CancellationTokenSource _cancellationTokenSource;

        #region IIoTDeviceClientService
        public ConnectionStatus LastKnownConnectionStatus { get; set; }

        public ConnectionStatusChangeReason LastKnownConnectionChangeReason { get; set; }

        public string ConnectionStatus => LastKnownConnectionStatus.ToString();
        
        public event EventHandler<string> ConnectionStatusChanged;

        public IoTDeviceClientService(IAppConfigService appConfigService, IDeviceInfoService deviceInfoService)
        {
            _appConfigService = appConfigService;
            _deviceInfoService = deviceInfoService;

            _cancellationTokenSource = new CancellationTokenSource();

            LastKnownConnectionStatus = Microsoft.Azure.Devices.Client.ConnectionStatus.Disconnected;
        }

        private void ConnectionStatusChangesHandler(ConnectionStatus status, ConnectionStatusChangeReason reason)
        {            
            LastKnownConnectionStatus = status;
            LastKnownConnectionChangeReason = reason;
            ConnectionStatusChanged?.Invoke(this, status.ToString());
        }

        public async Task<bool> Connect()
        {            
            var deviceId = _deviceInfoService.GetDeviceId();

            if (string.IsNullOrEmpty(_appConfigService.AssignedEndPoint))
            {

                await Provision();
            }
  
            if (_deviceClient != null)
            {
                _cancellationTokenSource?.Cancel();
                await _deviceClient?.CloseAsync();
                _deviceClient.Dispose();
                _deviceClient = null;
            }
           
            var symetricKey = GenerateSymmetricKey(deviceId, _appConfigService.DpsSymetricKey);

            var sasToken = GenerateSasToken(_appConfigService.AssignedEndPoint, symetricKey, null);
            
            _deviceClient = DeviceClient.Create(_appConfigService.AssignedEndPoint, 
                new DeviceAuthenticationWithToken(deviceId, sasToken), 
                TransportType.Mqtt_WebSocket_Only);
            
            _deviceClient.SetConnectionStatusChangesHandler(ConnectionStatusChangesHandler);
            
            await _deviceClient.OpenAsync(_cancellationTokenSource.Token);

            // await _deviceClient.SetDesiredPropertyUpdateCallbackAsync(DesiredPropertyUpdateCallback, null, _iotHubCancellationTokenSource.Token);
            return true;
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
            if (LastKnownConnectionStatus == Microsoft.Azure.Devices.Client.ConnectionStatus.Connected)
            {
                var msg = new Message(Encoding.ASCII.GetBytes(message));

                await _deviceClient.SendEventAsync(msg);
            }
        }
        #endregion

        private async Task<bool> Provision()
        {
            var dpsGlobalEndpoint = _appConfigService.DpsGlobalEndpoint;
            var dpsIdScope = _appConfigService.DpsIdScope;
            var deviceId = _deviceInfoService.GetDeviceId();
            var dpsSymetricKey = GenerateSymmetricKey(deviceId, _appConfigService.DpsSymetricKey);
           

            using (var security = new SecurityProviderSymmetricKey(deviceId, dpsSymetricKey, dpsSymetricKey))
            {           
                using (var transport = new ProvisioningTransportHandlerHttp())
                {
                    var provisioningClient = ProvisioningDeviceClient.Create(dpsGlobalEndpoint, dpsIdScope, security, transport);


                    var regResult = await provisioningClient.RegisterAsync(_cancellationTokenSource.Token);

                    if (regResult.Status == ProvisioningRegistrationStatusType.Assigned)
                    {
                        _appConfigService.AssignedEndPoint = regResult.AssignedHub;
                    }
                    return true;
                }
            }
        }


        private string CreateSasToken(string resourceUri, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var week = 60 * 60 * 24 * 7;
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + week);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry);//, keyName);
            return sasToken;
        }

        public string GenerateSasToken(string resourceUri, string key, string policyName, int expiryInSeconds = 3600)
        {
            var fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = $"{fromEpochStart.TotalSeconds + expiryInSeconds}";

            var stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;

            var hmac = new HMACSHA256(Convert.FromBase64String(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            var token = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}", HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry);

            if (!String.IsNullOrEmpty(policyName))
            {
                token += "&skn=" + policyName;
            }

            return token;
        }

        private string GenerateSymmetricKey(string deviceId, string secret)
        {
            string signature;
            using (var hmac = new HMACSHA256(Convert.FromBase64String(secret)))
            {
                signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(deviceId)));
            }

            return signature;
        }

        public bool IsSasTokenValid(string token)
        {
            try
            {
                var tokenExpiry = int.Parse(Regex.Matches(token, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value)["se"]);

                return DateTimeOffset.FromUnixTimeSeconds(tokenExpiry) > DateTime.UtcNow.AddMinutes(-10);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
