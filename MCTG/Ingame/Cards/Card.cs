using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.Ingame.Cards;

namespace MCTG.Ingame.Cards
{
      
    public class Card : ICardInterface
    {
        public readonly string id;
        private readonly double dmg;
        private readonly TYPE elementType;
        private readonly string name; 
        private readonly CARDTYPE cardType;

        public string LogActions(ICardInterface target)
        {
            string log;

            if (cardType == CARDTYPE.SPELL)
            {
                if (target.GetName().Contains("Kraken"))
                {
                    Console.WriteLine("The Kraken is immune to spells!");
                    log = "The Kraken is immune to spells!";
                }
                else
                    log = null;
            }
            else
            {
                bool drowningKinght = (target.GetElementType() == TYPE.WATER && GetName().Contains("Knight") && target.GetCardType() == CARDTYPE.SPELL);
                bool orkVSwizard = (GetName().Contains("Ork") && target.GetName().Contains("Wizard"));
                bool goblinVSdragon = (GetName().Contains("Goblin") && target.GetName().Contains("Dragon"));
                bool dragonVSfireElve = (GetName().Contains("Dragon") && target.GetName().Contains("FireElve"));


                if (drowningKinght)
                {

                    Console.WriteLine("The knight's heavy Armour pulls him down into the depths of the water and he drowns.");
                    log = "The knight's heavy Armour pulls him down into the depths of the water and he drowns.";
                }
                else if (orkVSwizard)
                {
                    Console.WriteLine("The warlock cast a spell on the orcs! They obey him and are now under his command.");
                    log = "The warlock cast a spell on the orcs! They obey him and are now under his command.";
                }
                else if (goblinVSdragon)
                {
                    Console.WriteLine("The golbins are rigid with shock and are destroyed by the raging flame of the dragon!");
                    log = "The golbins are rigid with shock and are destroyed by the raging flame of the dragon!";
                }
                else if (dragonVSfireElve)
                {
                    Console.WriteLine("The fire elves evade the attacks of the dragon and cut off his head.");
                    log = "The fire elves evade the attacks of the dragon and cut off his head.";
                }
                else
                {
                    log = null;
                }
            }
            return log;
        }


        public double Attack(ICardInterface target)
        {
            double actualDmg;

            if (cardType == CARDTYPE.SPELL)
            {
                if (target.GetName().Contains("Kraken"))
                {           
                    actualDmg = 0;
                }
                else
                    actualDmg = GetDmg() * CalcMult(target);
            }
            else
            {
                bool drowningKinght = (target.GetElementType() == TYPE.WATER && GetName().Contains("Knight") && target.GetCardType() == CARDTYPE.SPELL);
                bool orkVSwizard = (GetName().Contains("Ork") && target.GetName().Contains("Wizard"));
                bool goblinVSdragon = (GetName().Contains("Goblin") && target.GetName().Contains("Dragon"));
                bool dragonVSfireElve = (GetName().Contains("Dragon") && target.GetName().Contains("FireElve"));


                if (drowningKinght)
                {                             
                    actualDmg = 0;
                }
                else if (orkVSwizard)
                {
                    actualDmg = 0;
                }
                else if (goblinVSdragon)
                {
                    actualDmg = 0;
                }
                else if (dragonVSfireElve)
                {
                    actualDmg = 0;
                }
                else
                {
                    actualDmg = GetDmg() * CalcMult(target);
                }
            }
            return actualDmg;
        }

        public string GetId()
        {
            return id;
        }

        public double GetDmg()
        {
            return dmg;
        }

        public string GetName()
        {
            return name;
        }

        public TYPE GetElementType()
        {
            return elementType;
        }

        public CARDTYPE GetCardType()
        {
            return cardType;
        }

        public Card (string id, double dmg, string name)
        {
            this.id = id;
            this.dmg = dmg;
            
            this.name = name;
            if (name.Contains("Spell")) this.cardType = CARDTYPE.SPELL;
            else this.cardType = CARDTYPE.MONSTER;
            if (name.Contains("Water")) this.elementType = TYPE.WATER;
            else if (name.Contains("Fire")) this.elementType = TYPE.FIRE;
            else this.elementType = TYPE.NORMAL;
        }

        protected double CalcMult(ICardInterface target) // calculates multiplier for attack function based on element types
        {
            double multiplier = 1;
            if (cardType == CARDTYPE.SPELL || target.GetCardType() == CARDTYPE.SPELL)
            {
                switch (GetElementType())
                {
                    case TYPE.FIRE:
                        switch (target.GetElementType())
                        {
                            case TYPE.WATER:
                                multiplier = 0.5;
                                break;
                            case TYPE.FIRE:
                                multiplier = 1;
                                break;
                            default:
                                multiplier = 2;
                                break;
                        }
                        break;
                    case TYPE.WATER:
                        switch (target.GetElementType())
                        {
                            case TYPE.WATER:
                                multiplier = 1;
                                break;
                            case TYPE.FIRE:
                                multiplier = 2;
                                break;
                            case TYPE.NORMAL:
                                multiplier = 0.5;
                                break;
                        }
                        break;
                    case TYPE.NORMAL:
                        switch (target.GetElementType())
                        {
                            case TYPE.WATER:
                                multiplier = 2;
                                break;
                            case TYPE.FIRE:
                                multiplier = 0.5;
                                break;
                            case TYPE.NORMAL:
                                multiplier = 1;
                                break;
                        }
                        break;
                }
            }
            return multiplier;
        }
    }
}
