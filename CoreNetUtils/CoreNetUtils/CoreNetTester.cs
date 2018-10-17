using System.Net.NetworkInformation;
using System.Linq;
using System.Text;
using System.Threading;
using System;

namespace CoreNetUtils
{
/*

		Copyright (C) 2018 by Vladimir Novick http://www.linkedin.com/in/vladimirnovick , 

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in
		all copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
		THE SOFTWARE. 
		
*/	
	
    public class CoreNetTester
    {
        /// <summary>
        ///  1) recognizes changes related to Internet adapters
        ///  2) check all adapters -- filter by opstatus and activity
        /// </summary>
        /// <returns></returns>
        public static bool IsAvailableNetworkActive()
        {
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    return (from face in interfaces
                            where face.OperationalStatus == OperationalStatus.Up
                            where (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) && (face.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                            select face.GetIPv4Statistics()).Any(statistics => (statistics.BytesReceived > 0) && (statistics.BytesSent > 0));
                }
            }
            catch (Exception) { }

            return false;
        }

        /// <summary>
        ///     Ping address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static PingStatus Ping(string Address)
        {

            PingStatus ret = new PingStatus();
            ret.Host = Address;
            ret.Status = IPStatus.BadOption;

            try
            {

                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(Address, timeout, buffer, options);
                ret.Status = reply.Status;
                if (reply.Status == IPStatus.Success)
                {
                    ret.Address = reply.Address.ToString();
                    ret.RoundTrip = reply.RoundtripTime;
                    ret.Ttl = reply.Options.Ttl;
                    ret.DontFragment = reply.Options.DontFragment;
                    ret.Length = reply.Buffer.Length;
                }
            }
            catch (Exception) { }
            return ret;
        }
        /// <summary>
        ///   Waiting while Network unreachable
        /// </summary>
        /// <param name="deltaTime"> default: 10 sec ( 10000 ) </param>
        /// 
        /// <param name="maxTime"> max time (ms) default: 0 - not exist  </param>
        /// <returns>true - OK</returns>
        public static bool WaitUnreachable(string host,int deltaTime = 10000, int maxTime = 0)
        {
            DateTime startTime = DateTime.Now;

            while (true)
            {
                bool ret = IsAvailableNetworkActive();
                if (ret)
                {
                    var reply = Ping(host);
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }

                Thread.Sleep(deltaTime);

                if (maxTime != 0 )
                {
                    DateTime currentTime = DateTime.Now;

                    var delta = currentTime - startTime;
                    if (delta.TotalMilliseconds > maxTime)
                    {
                        return false;
                    }
                }

            }
        }
    }
}
