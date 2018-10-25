using System;
using System.IO;
using System.Net;

namespace CoreNetUtils
{
    /// <summary>
    ///    ping http with basic Network Credential
    /// </summary>
    public class PingHttp
    {
        /// <summary>
        ///  Costructor
        /// </summary>
        /// <param name="InterfaceURL"></param>
        /// <param name="WebUserName"></param>
        /// <param name="WebPassword"></param>
        public PingHttp(String InterfaceURL, String WebUserName = null , string WebPassword = null)
        {
            _InterfaceURL = InterfaceURL;
            _WebUserName = WebUserName;
            _WebPassword = WebPassword;
        }

        private string _InterfaceURL { get;  set; }
        private string _WebUserName { get; set; }
        private string _WebPassword { get; set; }

        public SecurityProtocolType SecurityProtocolType  { get; set; } = SecurityProtocolType.Tls12;

        /// <summary>
        ///   Set ping timeout
        /// </summary>
        public int Timeout { get; set; } = 5350000;

        private CredentialCache GetCredential()
        {
            string url = _InterfaceURL;
            ServicePointManager.SecurityProtocol = SecurityProtocolType;
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new System.Uri(url), "Basic", new NetworkCredential(_WebUserName,
                 _WebPassword));
            return credentialCache;
        }

        /// <summary>
        ///   ping specific url
        /// </summary>
        /// <param name="sUrl"></param>
        /// <returns></returns>
        public string Ping(string sUrl)
        {
            var encoding = System.Text.Encoding.UTF8;

            string lcHtml = null;

            try
            {

                var loHttp = (HttpWebRequest)WebRequest.Create(sUrl);

                loHttp.Timeout = Timeout;

                if (_WebUserName != null)
                {
                    loHttp.Credentials = GetCredential();
                    loHttp.PreAuthenticate = true;
                } else
                {
                    loHttp.Credentials = CredentialCache.DefaultCredentials;
                }


                    using (var loWebResponse = (HttpWebResponse)loHttp.GetResponseAsync().GetAwaiter().GetResult())
                    {
                        using (var responseStream = loWebResponse.GetResponseStream())
                        {

                            using (var loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), encoding))
                            {

                                lcHtml = loResponseStream.ReadToEnd();
                                loResponseStream.Close();

                            }
                        }
                    }
            } catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lcHtml;

        }
    }
}
