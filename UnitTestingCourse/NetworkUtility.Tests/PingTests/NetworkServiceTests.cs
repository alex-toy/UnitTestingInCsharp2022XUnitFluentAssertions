using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Xunit;

namespace NetworkUtility.Tests.PingTests
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _networkService;
        private readonly IDNS _dns;

        public NetworkServiceTests()
        {
            _dns = A.Fake<IDNS>();
            _networkService = new NetworkService(_dns);
        }

        [Fact]
        public void SendPing_should_return_Ping_When_DNS_success()
        {
            A.CallTo(() => _dns.SendDNS()).Returns(true);
            string result = _networkService.SendPing();

            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Ping", Exactly.Once());
        }

        [Fact]
        public void SendPing_should_not_return_Ping_When_DNS_failed()
        {
            A.CallTo(() => _dns.SendDNS()).Returns(false);
            string result = _networkService.SendPing();

            result.Should().NotBeNullOrEmpty();
            result.Should().NotContain("Ping");
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 1, 3)]
        [InlineData(3, 2, 5)]
        public void PingTimeOut_should_return_correct_result(int a, int b, int result)
        {
            int actualResult = _networkService.PingTimeOut(a, b);

            actualResult.Should().Be(result);
            actualResult.Should().BeGreaterThanOrEqualTo(2);
            actualResult.Should().NotBe(0);
        }

        [Fact]
        public void LastPingDate_should_return_correct_date()
        {
            DateTime result = _networkService.LastPingDate();

            result.Should().BeAfter(1.January(2020));
            result.Should().BeBefore(31.December(2030));
        }

        [Fact]
        public void GetPingOptions_should_return_correct_object()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            PingOptions? result = _networkService.GetPingOptions();

            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.DontFragment.Should().Be(true);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void MostRecentPings_should_return_correct_object()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            IEnumerable<PingOptions>? actual = _networkService.MostRecentPings();

            //actual.Should().BeOfType<IEnumerable<PingOptions>>();
            actual.Should().ContainEquivalentOf(expected);
            actual.Should().Contain(x => x.DontFragment == true);
            actual.ToList().Count.Should().Be(3);
        }
    }
}
