using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTG.DataTransferObjects;
using MCTG.DataLayer.Repositories;
using Newtonsoft.Json.Linq;

namespace MCTG.Controllers
{
    public class UserController
    {

        
        public static HTTPResponse Register(string body)
        {
            UserDTO? dto;
            try
            {   
                //deserialize HTTP message body and create RegisterDto object             
                dto = JsonSerializer.Deserialize<UserDTO>(body); 
                if (dto == null) throw new InvalidCastException("failed to parse Json");

                //check if user exists
                if (UserRepository.Instance.ExistingUser(dto.Username)) return new HTTPResponse(System.Net.HttpStatusCode.Conflict, $"User {dto.Username} already exists", false);
                var user = UserRepository.Instance.Add(new User(dto.Username, dto.Password));
                if (user == null) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Registration failed", false);
                Console.WriteLine("New user added.");
                return new HTTPResponse(System.Net.HttpStatusCode.OK, JsonSerializer.Serialize(user), true);
            }
            catch (InvalidCastException i)
            {
                Console.WriteLine($"{i.Message}");
                return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Registration failed", false);
            }

        }

        public static HTTPResponse GetUserData(string authToken)
        {
            Guid userid = TokenRepository.Instance.GetUidbyToken(authToken);
            string stats = UserRepository.Instance.GetData(userid.ToString());
            return new HTTPResponse(System.Net.HttpStatusCode.OK, stats, false);
        }

        public static HTTPResponse AlterUserData(string body, string authToken)
        {
            Guid userid = TokenRepository.Instance.GetUidbyToken(authToken);
            UserInfoDTO info = JsonSerializer.Deserialize<UserInfoDTO>(body);
            bool succesful = UserRepository.Instance.AlterUserInfo(info.Name, info.Bio, info.Image, userid.ToString());
            if(succesful) return new HTTPResponse(System.Net.HttpStatusCode.OK, $"user Table altered for {authToken}", false);
            else return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, $"Failed to alter user table for {authToken}", false);
        }

        public static HTTPResponse GetStats(string authToken)
        {
            Guid userid = TokenRepository.Instance.GetUidbyToken(authToken);
            string resp = UserRepository.Instance.GetStats(userid.ToString());
            return new HTTPResponse(System.Net.HttpStatusCode.OK, resp, false);

        }

        public static HTTPResponse GetScores()
        {
            string resp = UserRepository.Instance.GetScores();
            return new HTTPResponse(System.Net.HttpStatusCode.OK, resp, false);
        }
    }
}
