using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class StockChange
    {
        public DateTime? Date { get; set; }

        public decimal? Change { get; set; }
    }
}
