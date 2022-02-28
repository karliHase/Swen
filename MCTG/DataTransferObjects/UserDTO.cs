using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MCTG.DataTransferObjects
{
    class UserDTO
    {
        [JsonPropertyName("Username")]
        public string Username { get; private set; } // falls nicht funktioniert setter public machen
        [JsonPropertyName("Password")]
        public string Password { get; private set; }
        
        [JsonConstructor]
        public UserDTO(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
