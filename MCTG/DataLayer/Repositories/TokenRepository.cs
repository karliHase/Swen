using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MCTG.DataLayer.Repositories
{
    class TokenRepository : IDisposable
    {
        NpgsqlConnection npgsqlConnection;
        bool disposedValue;
        private static readonly Lazy<TokenRepository> lazy =
        new Lazy<TokenRepository>(() => new TokenRepository());

        public static TokenRepository Instance { get { return lazy.Value; } }





        private TokenRepository()
        {
            npgsqlConnection = new("Host=localhost:5432;Username=postgres;Password=postgres;Database=mctg");
            //npgsqlConnection.Open();
        }

        public Guid GetUidbyToken(string token)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT u_id FROM utoken WHERE token = @tok", npgsqlConnection);
            cmd.Parameters.AddWithValue("@tok", token);
            cmd.Prepare();
            var reader = cmd.ExecuteReader();
            reader.Read();
            Guid u_id = new Guid(reader.GetString(reader.GetOrdinal("u_id")));
            reader.Close();
            npgsqlConnection.Close();
            return u_id;
        }

        public bool Loginuser(string username, string password)
        {
            npgsqlConnection.Open();
            using var command = new NpgsqlCommand("SELECT u_id FROM users WHERE username = @username AND u_password = @u_password", npgsqlConnection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@u_password", password);
            command.Prepare();

            // fetch all the rowsfrom users table
            var dreader = command.ExecuteReader();
            if (dreader.HasRows)
            {
                dreader.Read();
                System.Guid u_id = new Guid(dreader.GetString(dreader.GetOrdinal("u_id")));
                dreader.Close();

                //check if there already is a token               
                //if there is no token create new one
                npgsqlConnection.Close();
                if (!tokenExists(u_id))
                {
                    npgsqlConnection.Open();
                    using var cmd2 = new NpgsqlCommand("INSERT INTO utoken (u_id, token) VALUES (@u_id, @token)", npgsqlConnection);
                    cmd2.Parameters.AddWithValue("@u_id", u_id);
                    cmd2.Parameters.AddWithValue("@token", "Basic " + username + "-mtcgToken");
                    cmd2.Prepare();
                    if (cmd2.ExecuteNonQuery() != 0)
                    {
                        npgsqlConnection.Close();
                        return true;
                    }
                    else
                    {
                        npgsqlConnection.Close();
                        return false;
                    }
                };
                npgsqlConnection.Close();
                return true;
            }
            else
            {
                npgsqlConnection.Close();
                return false;
            }
        }

        private bool tokenExists(System.Guid userId)
        {
            npgsqlConnection.Open();
            using var cmd = new NpgsqlCommand("SELECT t_id FROM utoken WHERE u_id = @id", npgsqlConnection);
            cmd.Parameters.AddWithValue("@id", userId.ToString());
            cmd.Prepare();

            var reader = cmd.ExecuteReader();
            bool existing = reader.Read();

            reader.Close();
            npgsqlConnection.Close();
            return existing;
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
        // ~TokenRepository()
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
