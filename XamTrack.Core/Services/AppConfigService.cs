using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XamTrack.Core.Services
{
    public class AppConfigService: IAppConfigService
    {
        private JObject _secrets;

        private const string Namespace = "XamTrack.Core";
        private const string FileName = "appconfig.json";

        public AppConfigService()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(AppConfigService)).Assembly;
                var stream = assembly.GetManifestResourceStream($"{Namespace}.{FileName}");
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    _secrets = JObject.Parse(json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to load secrets file : {ex}");
            }
        }

        public string DpsGlobalEndpoint => "global.azure-devices-provisioning.net";

        public string IotHubConnectionString => GetValue();

        public string DpsIdScope => GetValue();

        public string DpsSymetricKey => GetValue();

        public string AssignedEndPoint { get; set; }
        

        private string GetValue([CallerMemberName] string propertyname = null)
        {
            return this[propertyname];
        }

        public string this[string name]
        {
            get
            {
                try
                {
                    var path = name.Split(':');

                    JToken node = _secrets[path[0]];
                    for (int index = 1; index < path.Length; index++)
                    {
                        node = node[path[index]];
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to retrieve secret '{name}'");
                    return string.Empty;
                }
            }
        }
    }
}
