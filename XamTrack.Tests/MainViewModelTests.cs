using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System.Threading.Tasks;
using TinyIoC;
using TinyMvvm.IoC;
using Xamarin.Essentials;
using XamTrack.Core.Services;
using XamTrack.Core.ViewModels;

namespace XamTrack.Tests
{
    public class MainViewModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCurrent()
        {
            TinyIoCContainer.Current.Register<IAppConfigService, AppConfigService>();
            TinyIoCContainer.Current.Register<IDeviceInfoService, DeviceInfoService>();
            TinyIoCContainer.Current.Register<IGeolocationService, GeolocationService>();
            TinyIoCContainer.Current.Register<IIoTDeviceClientService, IoTDeviceClientService>();
            TinyIoCContainer.Current.Register<ILocationTrackerService, LocationTrackerService>();

            TinyIoCContainer.Current.Register<MainViewModel>();

      //      var resolver = new TinyIoCResolver();

            var sut = TinyIoCContainer.Current.Resolve<MainViewModel>();

            Assert.IsNotNull(sut);
           // var yest = resolver.Resolve<MainViewModel>();
            //Resolver.SetResolver(resolver);

        }
        

            [Test]
        public async Task CurrentLocationSetOnInitialisation()
        {
            var mocker = new AutoMocker(MockBehavior.Loose);
            
            mocker.Use<IGeolocationService>(x => x.GetLastKnownLocationAsync() == Task.FromResult(new Location()));

            var sut = mocker.CreateInstance<MainViewModel>();

            sut.ConnectCommand.Execute(null);

            bool invoked = false;

            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("CurrentLocation"))
                    invoked = true;
            };
            
            await sut.Initialize();

            Assert.True(invoked);            
        }

    }
}
