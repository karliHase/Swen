using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCTG.DataTransferObjects
{
    class CardDTO
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Damage { get; private set; }

        public CardDTO(string Id, string Name, double Damage)
        {
            this.Id = Id;
            this.Damage = Damage;
            this.Name = Name;
        }
    }
}
