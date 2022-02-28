using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MCTG.DataLayer.Repositories
{
    class UserRepository : IRepository<User>, IDisposable
    {
        NpgsqlConnection npgsqlConnection;
        bool disposedValue;
        private static readonly Lazy<UserRepository> lazy =
        new Lazy<UserRepository>(() => new UserRepository());

        public static UserRepository Instance { get { return lazy.Value; } }

        private UserRepository()
        {
            npgsqlConnection = new("Host=localhost:5432;Username=postgres;Password=postgres;Database=mctg");
            //npgsqlConnection.Open();
        }

        
        public void  reduceGold(Guid userId, int goldUser)
        {
            npgsqlConnection.Open();
            goldUser -= 5;
            string sql = "UPDATE users SET gold = @gold WHERE u_id = @userId";
            var cmd = new NpgsqlCommand(sql, npgsqlConnection);
            cmd.Parameters.AddWithValue("@gold", goldUser);
            cmd.Parameters.AddWithValue("@userId", userId.ToString());
            cmd.ExecuteNonQuery();
            npgsqlConnection.Close();
        }

        public bool ExistingUser(string username)
        {
            npgsqlConnection.Open();
            var cmd = new NpgsqlCommand("SELECT username FROM users WHERE username = @username", npgsqlConnection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            var result = reader.Read();

            if (result) Console.WriteLine($"User {username} already exists");
            reader.Close();
            npgsqlConnection.Close();
            return result;
        }

        public User Add(User obj)
        {
            npgsqlConnection.Open();
            using var command = new NpgsqlCommand("INSERT INTO users (u_id, username, u_password, elo, gamesPlayed, wins, losses, gold) VALUES ((@uID), (@Username), (@Password), (@elo), (@games), (@wins),(@losses), (@gold))", npgsqlConnection);

            command.Parameters.AddWithValue("uID", obj.uID);
            command.Parameters.AddWithValue("Username", obj.Username);
            command.Parameters.AddWithValue("Password", obj.Password);
            command.Parameters.AddWithValue("elo", obj.Elo);
            command.Parameters.AddWithValue("games", obj.GamesPlayed);
            command.Parameters.AddWithValue("wins", obj.Wins);
            command.Parameters.AddWithValue("losses", obj.Losses);
            command.Parameters.AddWithValue("gold", obj.Gold);
            command.Prepare();
            
            if(command.ExecuteNonQuery() != 0)
            {
                npgsqlConnection.Close();
                return obj;
            }
            else
            {
                npgsqlConnection.Close();
                return null;
            }
        }

        public bool AlterUserInfo(string name, string bio, string image, string userid)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("UPDATE users SET realname = @name, bio = @bio, image = @image WHERE u_id = @userid", npgsqlConnection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@bio", bio);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@userid", userid);
            cmd.Prepare();

            if (cmd.ExecuteNonQuery() != 0)
            {
                npgsqlConnection.Close();
                return true;
            }
            else 
            {
                npgsqlConnection.Close();
                return false; 
            }
        }

        public string GetScores()
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT username, elo, gamesplayed, losses, wins FROM users ORDER BY elo desc", npgsqlConnection);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            StringBuilder builder = new();
            while (reader.Read())
            {
                string username = reader.GetString(reader.GetOrdinal("username"));
                int elo = reader.GetInt32(reader.GetOrdinal("elo"));
                int gamesplayed = reader.GetInt32(reader.GetOrdinal("gamesplayed"));
                int losses = reader.GetInt32(reader.GetOrdinal("losses"));
                int wins = reader.GetInt32(reader.GetOrdinal("wins"));

                builder.AppendLine($"{username}:");
                builder.AppendLine($"Elo: {elo}, Games played: {gamesplayed}, Losses: {losses}, Wins: {wins}\n");
            }
         
            npgsqlConnection.Close();
            return builder.ToString();
        }

        public string GetStats(string userId)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT username, elo, gamesplayed, losses, wins FROM users WHERE u_id = @id", npgsqlConnection);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            reader.Read();
            string username = reader.GetString(reader.GetOrdinal("username"));
            int elo = reader.GetInt32(reader.GetOrdinal("elo"));
            int gamesplayed = reader.GetInt32(reader.GetOrdinal("gamesplayed"));
            int losses = reader.GetInt32(reader.GetOrdinal("losses"));
            int wins = reader.GetInt32(reader.GetOrdinal("wins"));

            StringBuilder builder = new();
            builder.AppendLine($"{username}:");
            builder.AppendLine($"Elo: {elo}, Games played: {gamesplayed}, Losses: {losses}, Wins: {wins}\n");

            npgsqlConnection.Close();
            return builder.ToString();
        }

        public string GetData(string userId)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT username, gold, elo, gamesplayed, losses, wins, image, realname FROM users WHERE u_id = @id", npgsqlConnection);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            reader.Read();
            string username = reader.GetString(reader.GetOrdinal("username"));
            int gold = reader.GetInt32(reader.GetOrdinal("gold"));
            int elo = reader.GetInt32(reader.GetOrdinal("elo"));
            int gamesplayed = reader.GetInt32(reader.GetOrdinal("gamesplayed"));
            int losses = reader.GetInt32(reader.GetOrdinal("losses"));
            int wins = reader.GetInt32(reader.GetOrdinal("wins"));
            string realname = "";
            string image = "";

            StringBuilder builder = new();
            builder.AppendLine($"{username}:");
            if (!reader.IsDBNull(reader.GetOrdinal("image")))
            {
                image = reader.GetString(reader.GetOrdinal("image"));
            }
            if (!reader.IsDBNull(reader.GetOrdinal("realname")))
            {
                realname = reader.GetString(reader.GetOrdinal("realname"));
            }
                         
            builder.AppendLine($"Gold: {gold}, Elo: {elo}, Games played: {gamesplayed}, Losses: {losses}, Wins: {wins}, Image: {image}, Realname: {realname}");
     
            npgsqlConnection.Close();

            return builder.ToString();
        }
        public int GetGoldByUID(Guid userId)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT gold FROM users WHERE u_id = @id", npgsqlConnection);
            cmd.Parameters.AddWithValue("@id", userId.ToString());
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            reader.Read();
            int gold = reader.GetInt32(reader.GetOrdinal("gold"));
            npgsqlConnection.Close();
            return gold;
        }
        
        public bool Delete(User obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User Update(User obj)
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
                    npgsqlConnection.Close();
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~UserRepository()
        // {
        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
