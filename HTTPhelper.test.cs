using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MCTG;
using Xunit;

namespace MTCG.Test
{
    public class HTTPhelperTest
    {
        [Fact]
        public void Parse_header_Test()
        {
            //Arrange
            string request = "GET /localhost/test HTTP/1.1\nAuthorization: Basic kienboec-mtcgToken\nContent-Type: application/json\n\n''";

            //Act
            HTTPMessage message = HTTPhelper.Parse(request);

            //Assert
            Assert.Equal("Basic kienboec-mtcgToken", message.Header["Authorization"]);
        }

        [Fact]
        public void Parse_Method_Test()
        {
            //Arrange
            string request = "GET /localhost/test HTTP/1.1\nAuthorization: Basic kienboec-mtcgToken\nContent-Type: application/json\n\n''";

            //Act
            HTTPMessage message = HTTPhelper.Parse(request);
            //Assert
            Assert.Equal(HttpMethod.Get, message.Method);
        }

        [Fact]
        public void Parse_Body_Test()
        {
            //Arrange
            string request = "POST /localhost/test HTTP/1.1\nAuthorization: Basic kienboec-mtcgToken\nContent-Type: application/json\n\n{'Hallo':'Eyo', 'Was':'los'}\n";

            //Act
            HTTPMessage message = HTTPhelper.Parse(request);
            //Assert
            Assert.Equal("{'Hallo':'Eyo', 'Was':'los'}", message.Body);
        }
    }
}
