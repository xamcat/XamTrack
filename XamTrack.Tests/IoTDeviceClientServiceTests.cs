﻿using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamTrack.Core.Services;

namespace XamTrack.Tests
{
    class IoTDeviceClientServiceTests
    {

        [Test]
        public async Task CanConnectToHub()
        {
            var mocker = new AutoMocker(MockBehavior.Loose);           

            var sut = mocker.CreateInstance<IoTDeviceClientService>();

            var result = await sut.Connect();

            Assert.True(result);
        }
    }
}