using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.tcp
{
    class RequestEntity
    {
        public string RequestUri { get; set; }
        public Action<object> ResponseAction { get; set; }
    }
}
