using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.Ingame.Cards;

namespace MCTG.Ingame.Cards
{
    public interface ICardInterface
    {
        public double Attack(ICardInterface target);
        public string GetName();
        public double GetDmg();
        public TYPE GetElementType();
        public CARDTYPE GetCardType();
    }
}
