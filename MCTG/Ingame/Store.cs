using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.Ingame.Cards;

namespace MCTG
{
    class Store
    {
        int packageCosts = 5;

        string[] monsterType = new string[7] { "GOBLIN", "DRAGON", "WIZARD", "ORK", "KNIGHT", "KRAKEN", "ELVE"};

        //creates and returns a package of 5 random cards
        /*CardBaseClass[] CreatePackage()
        {
            CardBaseClass[] package = new CardBaseClass[5];
            Random rand = new Random();

            int randMonster;

            for (int i = 0; i < 5; i++)
            {

                int randCardtype = rand.Next(10);
                CARDTYPE finalCardtype;

                TYPE randElement = (TYPE) rand.Next(3);

                string elementName;
                string cardName;
                switch (randElement)
                {
                    case TYPE.WATER:
                        elementName = "Water";
                        break;
                    case TYPE.FIRE:
                        elementName = "Fire";
                        break;
                    default:
                        elementName = "Normal";
                        break;
                }

                cardName = elementName;
                double dmg = (double) rand.Next(10, 100);

                if (randCardtype <= (int)PROBABILITY.SPELLPROB)
                {
                    finalCardtype = CARDTYPE.SPELL;
                    cardName = cardName + " " + "SPELL";
                    package[i] = new SpellCard(dmg, randElement, cardName, finalCardtype);
                }
                else
                {
                    finalCardtype = CARDTYPE.MONSTER;
                    randMonster = rand.Next(7);
                    cardName = cardName + " " + monsterType[randMonster];
                    package[i] = new SpellCard(dmg, randElement, cardName, finalCardtype);
                }

            }
            return package;
        }*/
    }
}
