using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MCTG
{
    public class HTTPMessage
    {
        public Dictionary<string, string> Header { get; private set; }
        public string URL { get; set; }
        public string Body { get; private set; }
        public HttpMethod Method { get; private set; }
        public string Version { get; private set; }

        public HTTPMessage(Dictionary <string, string> header, string url, string body, HttpMethod method, string version)
        {
            Header = header;
            URL = url;
            Body = body;
            Method = method;
            Version = version;
        }




    }
}
