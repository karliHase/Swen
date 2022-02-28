using System.Collections.Generic;
using System.Text.Json.Serialization;
using MCTG.Ingame.Cards;

namespace MCTG
{
    class User
    {   
        [JsonPropertyName("u_ID")]
        public System.Guid uID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Gold { get; set; }
        public int Elo { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public List<ICardInterface> Stack { get; set; }
        public List<ICardInterface> Deck { get; set; }


        public User(string username, string password)
        {
            uID = System.Guid.NewGuid();
            this.Username = username;
            this.Password = password;
            this.Gold = 20;
            this.Elo = 100;
            this.GamesPlayed = 0;
            this.Wins = 0;
            this.Losses = 0;
            this.Stack = new();
            this.Deck = new();
        }

        public void AddCardToDeck(ICardInterface cardWon)
        {
            Deck.Add(cardWon);
        }

        public void RemoveCardFromDeck(ICardInterface lostCard)
        {
            Deck.Remove(lostCard);
        }

        public int GetDeckCount()
        {
            return Deck.Count;
        }
        
    }
}
