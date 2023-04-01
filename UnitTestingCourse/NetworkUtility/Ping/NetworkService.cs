using NetworkUtility.DNS;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        private readonly IDNS _dns;

        public NetworkService(IDNS dns)
        {
            _dns = dns;
        }

        public string SendPing()
        {
            bool dnsSuccess = _dns.SendDNS();

            if (dnsSuccess) return "Ping is sent!";

            return "not sent";
        }

        public int PingTimeOut(int a, int b)
        {
            return a + b;
        }

        public DateTime LastPingDate()
        {
            return DateTime.Now;
        }

        public PingOptions GetPingOptions()
        {
            return new PingOptions()
            {
                DontFragment = true,
                Ttl = 1,
            };
        }

        public IEnumerable<PingOptions> MostRecentPings()
        {
            IEnumerable<PingOptions> pings = new[]
            {
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1,
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1,
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1,
                },
            };

            return pings;
        }
    }
}
