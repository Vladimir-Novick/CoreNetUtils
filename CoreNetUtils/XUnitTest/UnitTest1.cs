using CoreNetUtils;
using System;
using System.Net.NetworkInformation;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void IsNetworkExist()
        {
           bool ret =  CoreNetTester.IsAvailableNetworkActive();
            Assert.True(ret);
        }

        [Fact]
        public void PingGoogle()
        {
            var reply = CoreNetTester.Ping("8.8.8.8");
            Assert.True(reply.Status == IPStatus.Success);
        }

        [Fact]
        public void PingLocalIP4()
        {
            var reply = CoreNetTester.Ping("10.0.0.10");
            Assert.True(reply.Status == IPStatus.Success);
        }

        [Fact]
        public void PingNotLocalIP4()
        {
            var reply = CoreNetTester.Ping("10.0.0.100");
            Assert.True(reply.Status != IPStatus.Success);
        }

        [Fact]
        public void WaitUnreachable()
        {
            var reply = CoreNetTester.WaitUnreachable("10.0.0.10",5000,100000);
            Assert.True(reply);
        }
    }
}

