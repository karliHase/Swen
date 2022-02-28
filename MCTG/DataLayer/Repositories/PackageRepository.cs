using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MCTG.DataTransferObjects;
using Npgsql;

namespace MCTG.DataLayer.Repositories
{
    class PackageRepository : IDisposable
    {
        NpgsqlConnection npgsqlConnection;
        bool disposedValue;
        private static readonly Lazy<PackageRepository> lazy =
        new Lazy<PackageRepository>(() => new PackageRepository());

        public static PackageRepository Instance { get { return lazy.Value; } }

        private PackageRepository()
        {
            npgsqlConnection = new("Host=localhost:5432;Username=postgres;Password=postgres;Database=mctg");
            //npgsqlConnection.Open();
        }

        public void Delete(string packageID)
        {
            npgsqlConnection.Open();
            const string sql = "DELETE FROM packages WHERE p_pid = @packageID;";
            var cmd = new NpgsqlCommand(sql, npgsqlConnection);
            cmd.Parameters.AddWithValue("@packageID", packageID);
            cmd.ExecuteNonQuery();
            npgsqlConnection.Close();
        }

        public Guid? GetPackageIdOfFirstCard()
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT p_pid FROM packages LIMIT 1;", npgsqlConnection);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                npgsqlConnection.Close();
                return null; 
            }
            Guid p_pid = new (reader.GetString(reader.GetOrdinal("p_pid")));
            reader.Close();
            npgsqlConnection.Close();
            return p_pid;

        }

        public List<CardDTO> GetPackage(Guid? packageID)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT p_cardid, p_cardname, p_carddmg FROM packages where p_pid = @pId;", npgsqlConnection);
            cmd.Parameters.AddWithValue("@pid", packageID.ToString());
            List<CardDTO> cards = new List<CardDTO>();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string p_cardname = reader.GetString(reader.GetOrdinal("p_cardname"));
                string p_cardid = reader.GetString(reader.GetOrdinal("p_cardid"));
                double p_carddmg = reader.GetDouble(reader.GetOrdinal("p_carddmg"));
                cards.Add(new CardDTO(p_cardid, p_cardname, p_carddmg));
            }
            reader.Close();
            npgsqlConnection.Close();

            return cards;
        }

        public void Create(PackageDTO package)
        {
            npgsqlConnection.Open();
            foreach (var card in package.Cards) //convert each Card in package into CardDTO and create Database Record in "packages" table
            {
                CardDTO newCard = JsonSerializer.Deserialize<CardDTO>(card.ToString());
                const string sql = "INSERT INTO packages (p_cardid, p_pid, p_cardname, p_carddmg)" +
                               "VALUES ((@Id), (@p_pId), (@Name), (@Damage))";
                var cmd = new NpgsqlCommand(sql, npgsqlConnection);
                cmd.Parameters.AddWithValue("@Id", newCard.Id);
                cmd.Parameters.AddWithValue("@p_pId", package.p_pId);
                cmd.Parameters.AddWithValue("@Name", newCard.Name);
                cmd.Parameters.AddWithValue("@Damage", newCard.Damage);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            npgsqlConnection.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~PackageRepository()
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
