using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        public string SendPing()
        {
            return "Ping is sent!";
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
