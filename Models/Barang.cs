using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace barangin.Models
{
    public class Barang
    {
        public int Id { get; set; }
        public string nama_barang { get; set; }
        public int Qty { get; set; }
        public DateTime Timestamp { get; set; }

    }
}