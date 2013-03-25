/*
 * Copyright 2011 John Sample
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using MetroLog;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;

namespace CoinRT.Discovery
{
    /// <summary>
    /// Supports peer discovery through DNS.
    /// </summary>
    /// <remarks>
    /// This class does not support the testnet as currently there are no DNS servers providing testnet hosts.
    /// If this class is being used for testnet you must specify the hostnames to use.<p/>
    /// Failure to resolve individual host names will not cause an Exception to be thrown.
    /// However, if all hosts passed fail to resolve a PeerDiscoveryException will be thrown during getPeers().
    /// </remarks>
    public class DnsDiscovery : IPeerDiscovery
    {
        private static readonly ILogger Log = Common.Logger.GetLoggerForDeclaringType();

        private readonly string[] _hostNames;
        private readonly NetworkParameters _netParams;

        public static readonly string[] DefaultHosts =
            new[]
            {
                "dnsseed.bluematt.me", // Auto generated
                "bitseed.xf2.org", // Static
                "bitseed.bitcoin.org.uk" // Static
            };

        /// <summary>
        /// Supports finding peers through DNS A records. Community run DNS entry points will be used.
        /// </summary>
        /// <param name="netParams">Network parameters to be used for port information.</param>
        public DnsDiscovery(NetworkParameters netParams)
            : this(DefaultHostNames, netParams)
        {
        }

        /// <summary>
        /// Supports finding peers through DNS A records.
        /// </summary>
        /// <param name="hostNames">Host names to be examined for seed addresses.</param>
        /// <param name="netParams">Network parameters to be used for port information.</param>
        public DnsDiscovery(string[] hostNames, NetworkParameters netParams)
        {
            _hostNames = hostNames;
            _netParams = netParams;
        }

        /// <exception cref="PeerDiscoveryException"/>
        public async Task<IEnumerable<IPEndPoint>> GetPeers()
        {
            ICollection<IPEndPoint> addresses = new HashSet<IPEndPoint>();

            /*
             * Keep track of how many lookups failed vs. succeeded.
             * We'll throw an exception only if all the lookups fail.
             * We don't want to throw an exception if only one of many lookups fails.
             */
            var failedLookups = 0;

            foreach (var hostName in _hostNames)
            {
                try
                {
                    var hostAddresses = await DatagramSocket.GetEndpointPairsAsync(new HostName(hostName), "0");

                    foreach (var inetAddress in hostAddresses)
                    {
                        // DNS isn't going to provide us with the port.
                        // Grab the port from the specified NetworkParameters.

                        IPAddress ip = ParseIP(inetAddress.RemoteHostName.RawName);
                        var socketAddress = new IPEndPoint(ip, _netParams.Port);

                        // Only add the new address if it's not already in the combined list.
                        if (!addresses.Contains(socketAddress))
                        {
                            addresses.Add(socketAddress);
                        }
                    }
                }
                catch (Exception e)
                {
                    failedLookups++;
                    Log.Info("DNS lookup for {0} failed.", hostName);

                    if (failedLookups == _hostNames.Length)
                    {
                        // All the lookups failed.
                        // Throw the discovery exception and include the last inner exception.
                        throw new PeerDiscoveryException("DNS resolution for all hosts failed.", e);
                    }
                }
            }

            return addresses;
        }

        private IPAddress ParseIP(string ip)
        {
            var bytes = from part in ip.Split('.')
                        select byte.Parse(part);
            return new IPAddress(bytes.ToArray());

        }

        /// <summary>
        /// Returns the well known discovery host names on the production network.
        /// </summary>
        public static string[] DefaultHostNames
        {
            get { return DefaultHosts; }
        }
    }
}