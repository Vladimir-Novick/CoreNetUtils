using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace CoreNetUtils
{
    public class PingStatus
    {
        /// <summary>
        ///    Reports the status of sending an Internet Control Message Protocol (ICMP) echo  message to a computer.
        /// </summary>
        public IPStatus Status;

        /// <summary>
        ///    Gets the address of the host that sends the Internet Control Message Protocol
        ///    
        /// </summary>
        public string Address { set; get; } = "";
        /// <summary>
        ///    Gets the number of milliseconds taken to send an Internet Control Message Protocol
        /// </summary>
        public long RoundTrip {  set; get; }
        /// <summary>
        ///   Gets or sets the number of routing nodes that can forward the System.Net.NetworkInformation.Ping
        ///   An System.Int32 value that specifies the number of times the System.Net.NetworkInformation.Ping 
        ///   data packets can be forwarded. The default is 128.
        /// </summary>
        public int Ttl {  set; get; }
        public bool DontFragment {  set; get; }
        public int Length {  set; get; }
        /// <summary>
        ///    Gets the address of the host that sends the Internet Control Message Protocol
        /// </summary>
        public string Host { get; set; } = "";
    }
}
