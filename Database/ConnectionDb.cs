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

        public List<Barang> GetBarangList()
        {
            List<Barang> barangList = new List<Barang>();

            try
            {
                // Query SQL untuk mengambil data barang dari tabel
                string query = "SELECT * FROM barang";

                using (var command = new MySqlCommand(query, _connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Membaca data dari hasil query
                            int id = reader.GetInt32("id");
                            string namaBarang = reader.GetString("nama_barang");
                            int qty = reader.GetInt32("qty");

                            // Membuat objek Barang dan menambahkannya ke daftar barang
                            Barang barang = new Barang { Id = id, nama_barang = namaBarang, Qty = qty };
                            barangList.Add(barang);
                        }
                    }
                }

                Console.WriteLine("Berhasil mengambil data dari database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan saat mengambil data dari database: {ex.Message}");
            }

            return barangList;
        }

        public void Dispose()
        {
            // Menutup koneksi ketika objek dihancurkan
            _connection.Close();
            _connection.Dispose();
        }
    }
}
