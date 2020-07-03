using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XamTrack.Core.Helpers
{
    public static class IoTHelper
    {
        public static string GenerateSasToken(string resourceUri, string key, string policyName, int expiryInSeconds = 3600)
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

        public static string GenerateSymmetricKey(string deviceId, string secret)
        {
            string signature;
            using (var hmac = new HMACSHA256(Convert.FromBase64String(secret)))
            {
                signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(deviceId)));
            }

            return signature;
        }
    }
}
