using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTG.DataTransferObjects
{
    class UserInfoDTO
    {
        public string Bio { get; }
        public string Name { get; }
       
        public string Image { get; }

        public UserInfoDTO(string name, string bio, string image)
        {
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
