using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.tcp
{
    class ResponseEntity
    {
        public string DataHead { get; set; }
        public string DataTail { get; set; }
        public string StringData { get; set; }
        public List<byte> BytesData { get; set; }
        public int ContentType { get; set; }
        public int TotalLength { get; set; }
        public int CurrentLength { get; set; }
    }
}
