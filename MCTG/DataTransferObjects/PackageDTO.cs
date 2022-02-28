using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTG.DataTransferObjects
{
    class PackageDTO
    {
        public System.Guid p_pId { get; private set; }
        public object[] Cards { get; private set; }

        public PackageDTO(object[] cards)
        {
            p_pId = System.Guid.NewGuid();
            Cards = cards;
        }
    }
}
