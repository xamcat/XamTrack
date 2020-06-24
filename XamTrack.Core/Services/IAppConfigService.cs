namespace XamTrack.Core.Services
{
    public interface IAppConfigService
    {
        string DpsGlobalEndpoint { get; }
        string IotHubConnectionString { get; }
        string DpsIdScope { get; }  

        string DpsSymetricKey { get; }

        string AssignedEndPoint { get; set; }
    }
}
