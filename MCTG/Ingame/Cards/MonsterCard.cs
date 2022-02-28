using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.Ingame.Cards;

namespace MCTG.Ingame.Cards
{
    /*public class MonsterCard : CardBaseClass
    {
        //private readonly bool heavyArmour;

        public override double Attack(ICardInterface target)
        {
            double actualDmg;

            bool drowningKinght = (target.GetElementType() == TYPE.WATER && GetName().Contains("KNIGHT") && target.GetCardType() == CARDTYPE.SPELL);
            bool orkVSwizard = (GetName().Contains("ORK") && target.GetName().Contains("WIZARD"));
            bool goblinVSdragon = (GetName().Contains("GOBLIN") && target.GetName().Contains("DRAGON"));
            bool dragonVSfireElve = (GetName().Contains("DRAGON") && target.GetName().Contains("FIRE ELVE"));

            
            if(drowningKinght)
            {
                Console.WriteLine("The knight's heavy Armour pulls him down into the depths of the water and he drowns.");
                actualDmg = 0;
            }
            else if(orkVSwizard){
                Console.WriteLine("The warlock cast a spell on the orcs! They obey him and are now under his command.");
                actualDmg = 0;
            }
            else if (goblinVSdragon)
            {
                Console.WriteLine("The golbins are rigid with shock and are destroyed by the raging flame of the dragon!");
                actualDmg = 0;
            }
            else if (dragonVSfireElve)
            {
                Console.WriteLine("The fire elves evade the attacks of the dragon and cut off his head.");
                actualDmg = 0;
            }
            else
            {
                actualDmg = GetDmg() * CalcMult(target);
            }
            return actualDmg;
        }

        public MonsterCard(double dmg, TYPE type, string name, CARDTYPE cardType) : base(dmg, type, name, cardType)
        {

        }
    }*/
}
