using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTG.DataLayer.Repositories;
using MCTG.DataTransferObjects;

namespace MCTG.Controllers
{
    class CardController
    {
        public static HTTPResponse showStack(string token)
        {
            List<CardDTO> stack = CardRepository.Instance.getStack(token);

            if(stack.Count > 0)
            {
                return new HTTPResponse(System.Net.HttpStatusCode.OK, JsonSerializer.Serialize(stack), true);
            }
            else
            {
                return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "You have no cards.", false);
            }

        }

        public static HTTPResponse ShowDeck(string token)
        {
            List<CardDTO> deck = CardRepository.Instance.getDeck(token);
            if(deck.Count < 1) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Deck not configured.", false);
            else return new HTTPResponse(System.Net.HttpStatusCode.OK, JsonSerializer.Serialize(deck), true);

        }

        internal static HTTPResponse DeckPlain(string authToken)
        {
            List<CardDTO> deck = CardRepository.Instance.getDeck(authToken);
            if (deck.Count < 1) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "Deck not configured.", false);
            StringBuilder plain = new();
            plain.AppendLine("Your deck consists of the following Cards:");
            foreach(CardDTO card in deck)
            {
                plain.AppendLine($"+ {card.Name}");
            }
            return new HTTPResponse(System.Net.HttpStatusCode.OK, plain.ToString(), false);
        }

        public static HTTPResponse ConfigureDeck(string body, string token)
        {
            string[] IDs = JsonSerializer.Deserialize<string[]>(body);

            if (IDs.Count() != 4) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "You have to choose exactly 4 cards.", false);

            Guid userId = TokenRepository.Instance.GetUidbyToken(token);

            List<CardDTO> stack = CardRepository.Instance.getStack(token);


            foreach(string id in IDs)
            {
                bool containsID = false;
                //check if sender is real owner of the card
                foreach(CardDTO card in stack)
                {
                    if (card.Id.Contains(id))
                    {
                        containsID = true;
                        break; 
                    }
                    else containsID = false;
                }
                if(containsID == false) return new HTTPResponse(System.Net.HttpStatusCode.BadRequest, "These are not your cards.", false);
            }

            foreach(string id in IDs)
            {
                CardRepository.Instance.AddtoDeck(id);
            }
            return new HTTPResponse(System.Net.HttpStatusCode.OK, "Your deck has been created. You are now ready for battle.", false);
        }

        
    }
}
