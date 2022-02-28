using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTG.DataTransferObjects;
using Npgsql;

namespace MCTG.DataLayer.Repositories
{
    class CardRepository : IRepository<CardDTO>, IDisposable
    {
        NpgsqlConnection npgsqlConnection;
        bool disposedValue;
        private static readonly Lazy<CardRepository> lazy =
        new Lazy<CardRepository>(() => new CardRepository());

        public static CardRepository Instance { get { return lazy.Value; } }

        public CardRepository()
        {
            npgsqlConnection = new("Host=localhost:5432;Username=postgres;Password=postgres;Database=mctg");
            //npgsqlConnection.Open();
        }

        public bool AddPackage(List<CardDTO> newCards, string ownerID)
        {
            npgsqlConnection.Open();
            bool deck = false;
            foreach(CardDTO card in newCards)
            {
                using var command = new NpgsqlCommand("INSERT INTO cards (c_id, c_name, c_dmg, c_deck, c_ownerid) VALUES ((@id), (@name), (@damage),(@deck), (@ownerId))", npgsqlConnection);

                command.Parameters.AddWithValue("id", card.Id);
                command.Parameters.AddWithValue("name", card.Name);
                command.Parameters.AddWithValue("damage", card.Damage);
                command.Parameters.AddWithValue("deck", deck);
                command.Parameters.AddWithValue("ownerId", ownerID);
                command.Prepare();

                if (command.ExecuteNonQuery() == 0) 
                {
                    npgsqlConnection.Close();
                    return false; 
                }
            }
            npgsqlConnection.Close();
            return true;
        }

        public List<CardDTO> getDeck(string token)
        {
            npgsqlConnection.Open();
            bool indeck = true;
            Guid userId = TokenRepository.Instance.GetUidbyToken(token);
            using var command = new NpgsqlCommand("SELECT c_id, c_name, c_dmg FROM cards WHERE c_ownerid = @userId AND c_deck = @deck", npgsqlConnection);
            command.Parameters.AddWithValue("@userId", userId.ToString());
            command.Parameters.AddWithValue("@deck", indeck);
            command.Prepare();
            var reader = command.ExecuteReader();

            List<CardDTO> deck = new List<CardDTO>();
            while (reader.Read())
            {
                var cardId = reader.GetString(reader.GetOrdinal("c_id"));
                var cardDmg = reader.GetDouble(reader.GetOrdinal("c_dmg"));
                var cardName = reader.GetString(reader.GetOrdinal("c_name"));
                deck.Add(new CardDTO(cardId, cardName, cardDmg));
            }

            reader.Close();
            npgsqlConnection.Close();
            return deck;
        }

        
        public void AddtoDeck(string cardId)
        {
            npgsqlConnection.Open();
            bool deck = true;
            using var command = new NpgsqlCommand("UPDATE cards SET c_deck = @indeck WHERE c_id = @cardid", npgsqlConnection);
            command.Parameters.AddWithValue("@indeck", deck);
            command.Parameters.AddWithValue("@cardid", cardId);
            command.Prepare();
            command.ExecuteNonQuery();
            npgsqlConnection.Close();

        }

        public List<CardDTO> getStack(string token)
        {
            npgsqlConnection.Open();
            Guid userId = TokenRepository.Instance.GetUidbyToken(token);
            using var command = new NpgsqlCommand("SELECT c_id, c_name, c_dmg FROM cards WHERE c_ownerid = @userId", npgsqlConnection);
            command.Parameters.AddWithValue("@userId", userId.ToString());
            command.Prepare();
            var reader = command.ExecuteReader();

            List<CardDTO> stack = new List<CardDTO>();
            while (reader.Read())
            {
                var cardId = reader.GetString(reader.GetOrdinal("c_id"));
                var cardDmg = reader.GetDouble(reader.GetOrdinal("c_dmg"));
                var cardName = reader.GetString(reader.GetOrdinal("c_name"));
                
                stack.Add(new CardDTO(cardId, cardName, cardDmg));
            }

            reader.Close();
            npgsqlConnection.Close();
            return stack;
        }

        public CardDTO Add(CardDTO obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(CardDTO obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<CardDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public CardDTO GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public CardDTO Update(CardDTO obj)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                    //npgsqlConnection.Close();
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~CardRepository()
        // {
        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
