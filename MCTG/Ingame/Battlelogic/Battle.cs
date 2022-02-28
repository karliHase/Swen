using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.Ingame.Cards;

namespace MCTG.Ingame.Battlelogic
{
    public class Battle
    {
        private User player1;
        private User player2;
        public string battlelog;

        public string startBattle()
        {
            Random rand = new Random();
            User battleWinner = null;
            ICardInterface player1Card;
            ICardInterface player2Card;
            double player1Dmg;
            double player2Dmg;
            int roundCounter = 1;

            while (roundCounter <= 100 && battleWinner == null)
            {
                //get random card for each player
                player1Card = player1.Deck[rand.Next(0, player1.GetDeckCount())];
                player2Card = player2.Deck[rand.Next(0, player2.GetDeckCount())];

                //calculate damage
                player1Dmg = player1Card.Attack(player2Card);
                player2Dmg = player2Card.Attack(player1Card);

                if(player1Dmg > player2Dmg)
                {
                    battlelog += "\n------------------------------------------------------------------------\nRound: " + roundCounter + "\n";
                    battlelog += player1.Username + ":\tCard played: " + player1Card.GetName() + "\tDamage: " + player1Dmg + "\n";
                    battlelog += player2.Username + ":\tCard played: " + player2Card.GetName() + "\tDamage: " + player2Dmg + "\n";
                    battlelog += player1.Username + " won this round.";
                    battlelog += "\n------------------------------------------------------------------------\n";
                    player1.AddCardToDeck(player2Card);
                    player2.RemoveCardFromDeck(player2Card);
                }
                else if(player1Dmg < player2Dmg)
                {
                    battlelog += "\n------------------------------------------------------------------------\nRound: " + roundCounter + "\n";
                    battlelog += player1.Username + ":\tCard played: " + player1Card.GetName() + "\tDamage: " + player1Dmg + "\n";
                    battlelog += player2.Username + ":\tCard played: " + player2Card.GetName() + "\tDamage: " + player2Dmg + "\n";
                    battlelog += player2.Username + " won this round.";
                    battlelog += "\n------------------------------------------------------------------------\n";
                    player2.AddCardToDeck(player1Card);
                    player1.RemoveCardFromDeck(player1Card);
                }
                else
                {
                    battlelog += "\n------------------------------------------------------------------------\nRound: " + roundCounter + "\n";
                    battlelog += player1.Username + ":\tCard played: " + player1Card.GetName() + "\tDamage: " + player1Dmg + "\n";
                    battlelog += player2.Username + ":\tCard played: " + player2Card.GetName() + "\tDamage: " + player2Dmg + "\n";
                    battlelog += "Draw, nothing happend";
                    battlelog += "\n------------------------------------------------------------------------\n";
                }


                roundCounter++;
                if (player1.GetDeckCount() == 0)
                {
                    battleWinner = player2;
                } 
                else if(player2.GetDeckCount() == 0)
                {
                    battleWinner = player1;
                }
            }

            battlelog += battleWinner.Username + " has won the game!\n";
            /////////////////////////////////////////////////////////////////////////////////////////////
            //update player stats
            ////////////////////////////////////////////////////////////////////////////////////////////
            return battlelog;

        }

    }
}
