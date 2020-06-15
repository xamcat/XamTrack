using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IGeolocationService>().Setup(x => x.GetLastKnownLocationAsync()).ReturnsAsync(new Location());
                

                var sut = mock.Create<MainViewModel>();
                sut.ConnectCommand.Execute(null);

                bool invoked = false;

                sut.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName.Equals("CurrentLocation"))
                        invoked = true;
                };
                //var order = await orderService.GetOrderAsync(1, GlobalSetting.Instance.AuthToken);
                //await orderViewModel.InitializeAsync(order);
                await sut.Initialize();

                Assert.True(invoked);
            }
        }
    }
}
