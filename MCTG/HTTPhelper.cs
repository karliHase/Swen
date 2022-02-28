using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MCTG
{
    public static class HTTPhelper
    {
        public static HTTPMessage Parse(string message)
        {
            List<string> lines = message.Split("\n").ToList();
           
            
            
            string firstLine = lines[0];
            var urlparts = firstLine.Split(" "); //create aray for URL parts
            string url = urlparts[1];
            string HTTPversion = urlparts[2];
            HttpMethod method = new(urlparts[0]);
            lines.RemoveAt(0);

            Dictionary<string, string> header = new();
            bool bodyflag = false;
            string body = "";

            try
            {
                //loop to differ between header and body and store them in header and body variable
                foreach (var line in lines)
                {
                    if (line == "") bodyflag = true;
                    if (!bodyflag)
                    {
                        var headerEntry = line.Split(": ");
                        header.Add(headerEntry[0], headerEntry[1]);
                    }
                    else
                    {
                        body += line;
                    }                    
                }

                if(header.Count > 0)
                {
                    return new HTTPMessage(header, url, body, method, HTTPversion);
                }
                else
                {
                    throw new Exception("Header Count was 0");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unexcpected exception: {e.Message}");
            }

            return null;

        }
    }
}
