using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class XYPair<Tx, Ty>
    {
        public Tx x { get; set; }
        public Ty y { get; set; }
        public string tooltip { get; set; }
    }

    public class DataPoint
    {
        public DataPoint()
        {
            data = new List<XYPair<object, object>>();
        }
        public List<XYPair<object, object>> data { get; set; }
    }
}
