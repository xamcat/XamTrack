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
        public void IoCResolvesDependanciesForMainViewModel()
        {
            TinyIoCContainer.Current.Register<IAppConfigService, AppConfigService>();
            TinyIoCContainer.Current.Register<IDeviceInfoService, DeviceInfoService>();
            TinyIoCContainer.Current.Register<IGeolocationService, GeolocationService>();
            TinyIoCContainer.Current.Register<IIoTDeviceClientService, IoTDeviceClientService>();
            TinyIoCContainer.Current.Register<MainViewModel>();

            var sut = TinyIoCContainer.Current.Resolve<MainViewModel>();

            Assert.IsNotNull(sut);
        }
        
        [Test]
        public async Task CurrentLocationSetOnAppearing()
        {
            // Arrange
            bool invoked = false;
            var mocker = new AutoMocker(MockBehavior.Loose);            
            mocker.Use<IGeolocationService>(x => x.GetLastKnownLocationAsync() == Task.FromResult(new Location()));
            var sut = mocker.CreateInstance<MainViewModel>();            
            sut.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals("CurrentLocation"))
                    invoked = true;
            };

            // Act            
            await sut.OnAppearing();

            // Assert
            Assert.True(invoked);            
        }
    }
}
