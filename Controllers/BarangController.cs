using System;
using Microsoft.AspNetCore.Mvc;
using barangin.Models;
using barangin.Database;
using MySqlConnector;




namespace barangin.Controllers
{
    [Route("Barang")]
    public class BarangController : Controller
    {
        private readonly IConfiguration _configuration;

        public BarangController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Barang> BarangList { get; set; } = new List<Barang>();

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            try
            {
                // Mendapatkan nilai ConnectionStrings dari appsettings.json
                string connectionString = _configuration.GetConnectionString("Default");

                // Membuat instance dari ConnectionDb
                using (var dbConnection = new ConnectionDb(connectionString))
                {
                    // Menggunakan fungsi TestConnection untuk memastikan koneksi ke database
                    dbConnection.TestConnection();

                    // Query SQL untuk mengambil data barang dari tabel
                    string query = "SELECT * FROM barang";

                    // Membuat MySqlCommand
                    using (var command = new MySqlCommand(query, dbConnection.CreateCommand().Connection))
                    {
                        // Eksekusi query dan membaca hasil
                        using (var reader = command.ExecuteReader())
                        {
                            List<Barang> BarangList = new List<Barang>();

                            while (reader.Read())
                            {
                                // Membaca data dari hasil query
                                int id = reader.GetInt32("id");
                                string namaBarang = reader.GetString("nama_barang");
                                int qty = reader.GetInt32("qty");

                                // Membuat objek Barang dan menambahkannya ke daftar barang
                                Barang barang = new Barang { Id = id, nama_barang = namaBarang, Qty = qty };
                                BarangList.Add(barang);
                            }

                            // Menampilkan data di console
                            foreach (var barang in BarangList)
                            {
                                Console.WriteLine($"ID: {barang.Id}, Nama Barang: {barang.nama_barang}, Qty: {barang.Qty}");
                            }

                            // Mengirim data ke tampilan (view)
                            ViewBag.BarangList = BarangList;
                            return View("Main");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan: {ex.Message}");
                return View("Error");
            }
        }

        [Route("BarangJson")]
        public IActionResult BarangJson()
        {
            Barang emp = new Barang()
            {
                Id = 1,
                nama_barang = "Kopi",
                Qty = 10
            };
            return Json(emp);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View("CreateBarang/Create");
        }

        [HttpPost]
        [Route("SubmitCreate")]
        public IActionResult SubmitCreate(Barang barang)
        {
            try
            {
                barang.Timestamp = DateTime.Now;

                // Menggunakan koneksi database yang sama dengan yang digunakan dalam Index
                using (var connectionDb = new ConnectionDb(_configuration.GetConnectionString("Default")))
                {
                    // Buat command menggunakan koneksi dari ConnectionDb
                    using (var cmd = connectionDb.CreateCommand())
                    {
                        // Definisikan pernyataan SQL INSERT
                        cmd.CommandText = "INSERT INTO barang (nama_barang, qty, timestamp) VALUES (@nama_barang, @Qty, @Timestamp)";

                        // Parameterisasi nilai-nilai
                        cmd.Parameters.AddWithValue("@nama_barang", barang.nama_barang);
                        cmd.Parameters.AddWithValue("@Qty", barang.Qty);
                        cmd.Parameters.AddWithValue("@Timestamp", barang.Timestamp);

                        // Eksekusi pernyataan SQL
                        cmd.ExecuteNonQuery();
                    }
                }

                // Menampilkan pesan ke console
                Console.WriteLine($"Data Barang berhasil disimpan pada {barang.Timestamp}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan saat menyimpan data: {ex.Message}");
                // Kembali ke halaman "Create"
                return View("CreateBarang/Create");
            }
        }

        //get data and show by id
        [HttpGet]
        [Route("Detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                // Mendapatkan nilai ConnectionStrings dari appsettings.json
                string connectionString = _configuration.GetConnectionString("Default");

                // Membuat instance dari ConnectionDb
                using (var dbConnection = new ConnectionDb(connectionString))
                {
                    // Menggunakan fungsi TestConnection untuk memastikan koneksi ke database
                    dbConnection.TestConnection();

                    // Query SQL untuk mengambil data barang dari tabel berdasarkan ID
                    string query = $"SELECT * FROM barang WHERE id = {id}";

                    // Membuat MySqlCommand
                    using (var command = new MySqlCommand(query, dbConnection.CreateCommand().Connection))
                    {
                        // Eksekusi query dan membaca hasil
                        using (var reader = command.ExecuteReader())
                        {
                            // Mengecek apakah data ditemukan
                            if (reader.Read())
                            {
                                // Membaca data dari hasil query
                                int barangId = reader.GetInt32("id");
                                string namaBarang = reader.GetString("nama_barang");
                                int qty = reader.GetInt32("qty");

                                // Membuat objek Barang
                                Barang barang = new Barang { Id = barangId, nama_barang = namaBarang, Qty = qty };

                                // Menampilkan data di console
                                Console.WriteLine($"ID: {barang.Id}, Nama Barang: {barang.nama_barang}, Qty: {barang.Qty}");

                                // Mengirim data ke tampilan (view)
                                ViewBag.BarangDetail = barang;
                                return View("DetailBarang/DetailPage");
                            }
                            else
                            {
                                // Jika data tidak ditemukan, kembalikan ke halaman Index
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan: {ex.Message}");
                return View("Index");
            }
        }

        // show by id
        [HttpGet]
        [Route("Update/{id}")]
        public IActionResult Update(int id)
        {
            try
            {
                // Mendapatkan nilai ConnectionStrings dari appsettings.json
                string connectionString = _configuration.GetConnectionString("Default");

                // Membuat instance dari ConnectionDb
                using (var dbConnection = new ConnectionDb(connectionString))
                {
                    // Menggunakan fungsi TestConnection untuk memastikan koneksi ke database
                    dbConnection.TestConnection();

                    // Query SQL untuk mengambil data barang dari tabel berdasarkan ID
                    string query = $"SELECT * FROM barang WHERE id = {id}";

                    // Membuat MySqlCommand
                    using (var command = new MySqlCommand(query, dbConnection.CreateCommand().Connection))
                    {
                        // Eksekusi query dan membaca hasil
                        using (var reader = command.ExecuteReader())
                        {
                            // Mengecek apakah data ditemukan
                            if (reader.Read())
                            {
                                // Membaca data dari hasil query
                                int barangId = reader.GetInt32("id");
                                string namaBarang = reader.GetString("nama_barang");
                                int qty = reader.GetInt32("qty");

                                // Membuat objek Barang
                                Barang barang = new Barang { Id = barangId, nama_barang = namaBarang, Qty = qty };

                                // Menampilkan data di console
                                Console.WriteLine($"ID: {barang.Id}, Nama Barang: {barang.nama_barang}, Qty: {barang.Qty}");

                                // Mengirim data ke tampilan (view)
                                ViewBag.BarangDetail = barang;
                                return View("UpdateBarang/Update");
                            }
                            else
                            {
                                // Jika data tidak ditemukan, kembalikan ke halaman Index
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan: {ex.Message}");
                return View("Index");
            }
        }

        // update data by id
        [HttpPost]
        [Route("SubmitUpdate/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitUpdate(int id, Barang barang)
        {
            try
            {
                // Menggunakan koneksi database yang sama dengan yang digunakan dalam Index
                using (var connectionDb = new ConnectionDb(_configuration.GetConnectionString("Default")))
                {
                    // Buat command menggunakan koneksi dari ConnectionDb
                    using (var cmd = connectionDb.CreateCommand())
                    {
                        // Definisikan pernyataan SQL UPDATE
                        cmd.CommandText = "UPDATE barang SET nama_barang = @nama_barang, qty = @Qty WHERE id = @Id";

                        // Parameterisasi nilai-nilai
                        cmd.Parameters.AddWithValue("@Id", id); // Gunakan nilai ID dari parameter route
                        cmd.Parameters.AddWithValue("@nama_barang", barang.nama_barang);
                        cmd.Parameters.AddWithValue("@Qty", barang.Qty);

                        // Eksekusi pernyataan SQL
                        cmd.ExecuteNonQuery();
                    }
                }

                // Menampilkan pesan ke console
                Console.WriteLine($"Data Barang dengan ID {id} berhasil diupdate pada {DateTime.Now}");

                // Redirect ke halaman Index setelah update
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan saat menyimpan data: {ex.Message}");
                // Kembali ke halaman "Update"
                return View("UpdateBarang/Update", barang);
            }
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Mendapatkan nilai ConnectionStrings dari appsettings.json
                string connectionString = _configuration.GetConnectionString("Default");

                // Membuat instance dari ConnectionDb
                using (var dbConnection = new ConnectionDb(connectionString))
                {
                    // Menggunakan fungsi TestConnection untuk memastikan koneksi ke database
                    dbConnection.TestConnection();

                    // Query SQL untuk menghapus data barang dari tabel berdasarkan ID
                    string query = $"DELETE FROM barang WHERE id = {id}";

                    // Membuat MySqlCommand
                    using (var command = new MySqlCommand(query, dbConnection.CreateCommand().Connection))
                    {
                        // Eksekusi pernyataan SQL DELETE
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Data barang dengan ID {id} berhasil dihapus");
                            return Json(new { success = true, message = "Data berhasil dihapus." });
                        }
                        else
                        {
                            Console.WriteLine($"Data barang dengan ID {id} tidak ditemukan");
                            return Json(new { success = false, message = "Data tidak ditemukan." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kesalahan: {ex.Message}");
                return Json(new { success = false, message = "Terjadi kesalahan dalam menghapus data." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}