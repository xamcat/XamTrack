using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamTrack.Core.Services;
using XamTrack.Core.ViewModels;

namespace XamTrack.Tests
{
    public class LocationTrackerServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task StartTrackingSendUpdateAfterTimerEvent()
        {
            var mocker = new AutoMocker(MockBehavior.Loose);
            
            mocker.Use<IGeolocationService>(x => x.GetLastKnownLocationAsync() == Task.FromResult(new Location()));

            var sut = mocker.CreateInstance<LocationTrackerService>();

            var result = await sut.StartTrackingAsync();

            Assert.True(result);            
        }
    }
}
