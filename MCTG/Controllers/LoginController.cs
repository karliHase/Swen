using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTG.DataTransferObjects;
using MCTG.DataLayer.Repositories;

namespace MCTG.Controllers
{
    public class LoginController
    {
        public static HTTPResponse Handlelogin(string body)
        {
            UserDTO? dto;
            try
            {
                //deserialize HTTP message body and create RegisterDto object             
                dto = JsonSerializer.Deserialize<UserDTO>(body);
                if (dto == null) throw new InvalidCastException("failed to parse Json");
                var login = TokenRepository.Instance.Loginuser(dto.Username, dto.Password);
                if (!login)
                {
                    Console.WriteLine($"User {dto.Username} login failed\n");
                    return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Login failed", false);
                    
                }
                else
                {
                    Console.WriteLine($"User {dto.Username} logged in successfully\n");
                    return new HTTPResponse(System.Net.HttpStatusCode.OK, "You are now logged in", false);
                }

            }
            catch (InvalidCastException i)
            {
                Console.WriteLine($"{i.Message}");
                return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Login failed", false);
            }

        }
    }
}
