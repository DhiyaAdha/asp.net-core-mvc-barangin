using MySqlConnector;
using barangin.Models;
using System;
using System.Collections.Generic;

namespace barangin.Database
{
    public class ConnectionDb : IDisposable
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public ConnectionDb(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
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

        public void Dispose()
        {
            // Menutup koneksi ketika objek dihancurkan
            _connection.Close();
            _connection.Dispose();
        }

        // Menambahkan metode untuk mendapatkan MySqlCommand
        public MySqlCommand CreateCommand()
        {
            return new MySqlCommand
            {
                Connection = _connection
            };
        }
    }
}
