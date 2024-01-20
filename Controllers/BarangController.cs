using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using barangin.Models;


namespace barangin.Controllers
{
    [Route("Barang")]
    public class BarangController : Controller
    {
        private readonly ILogger<BarangController> _logger;

        public BarangController(ILogger<BarangController> logger)
        {
            _logger = logger;
        }

        [Route("Main")]
        public IActionResult Index()
        {
            return new ContentResult()
            {
                Content = "<html><body><h1>Halaman Barang</h1></body></html>",
            ContentType = "text/html",
            StatusCode = 200
            };
            // return View();
        }

        [Route("BarangJson")]
        public IActionResult BarangJson()
        {
            Barang emp = new Barang()
            {
                Id = 1,
                Name = "Kopi",
                Qty = 10
            };
            return Json(emp);
        }

        [Route("Create")]     
        public IActionResult Create()
        {   
            return View();
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}