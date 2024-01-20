using MySqlConnector;
using System;

namespace barangin.Database
{
    public class ConnectionDb
    {
        private readonly string _connectionString;

        public ConnectionDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void TestConnection()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database berhasil terkoneksi!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kesalahan koneksi database: {ex.Message}");
                }
            }
        }
    }
}
