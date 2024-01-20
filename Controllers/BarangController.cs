using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using barangin.Models;
using barangin.Database;


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

                    // Mengambil data dari database dan mengisi properti kelas BarangList
                    BarangList = dbConnection.GetBarangList();

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