using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System.Threading.Tasks;
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
        public async Task CurrentLocationSetOnInitialisation()
        {
            var mocker = new AutoMocker(MockBehavior.Loose);
            
            // mock.Mock<IGeolocationService>().Setup(x => x.GetLastKnownLocationAsync()).ReturnsAsync(new Location());
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
