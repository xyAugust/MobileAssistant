using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    class AdbOutputModel : NotificationBase
    {

        private string _StdOutPut;

        public string StdOutPut
        {
            get
            {
                return _StdOutPut;
            }
            set
            {
                _StdOutPut = value;
                this.RaisePropertyChanged("StdOutPut");
            }

        }
    }
}
