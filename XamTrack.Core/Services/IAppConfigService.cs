namespace XamTrack.Core.Services
{
    public interface IAppConfigService
    {
        string IotHubConnectionString { get; }
        string DpsIdScope { get; }  
    }
}
