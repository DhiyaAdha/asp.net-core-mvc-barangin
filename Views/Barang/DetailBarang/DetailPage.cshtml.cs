using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace barangin.Views.Barang.DetailBarang
{
    public class DetailPage : PageModel
    {
        private readonly ILogger<DetailPage> _logger;

        public DetailPage(ILogger<DetailPage> logger)
        {
            _logger = logger;
        }

        // public string Title { get; set; }

        public void OnGet()
        {
            // Title = "Detail Halaman Detail dari Model";
        }
    }
}