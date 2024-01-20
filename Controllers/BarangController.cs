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
            return RedirectToAction("Index");
        }

        [Route("Detail")]
        public IActionResult Detail()
        {
            return View("DetailBarang/DetailPage");
        }

        [Route("Update")]
        public IActionResult Update()
        {
            return View("UpdateBarang/Update");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}