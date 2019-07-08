using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel
{
    class ViewModelBss
    {
        private ViewModelBss() { }

        private static ViewModelBss instance;

        public static ViewModelBss Instance  {
            get
            {
                if (instance == null)
                {
                    instance = new ViewModelBss();
                }
                return instance;
            }
        }


        private MainWindowViewModel _MainVM;
        public MainWindowViewModel MainVM
        {
            get
            {
                if (_MainVM == null)
                {
                    _MainVM = new MainWindowViewModel();
                }
                return _MainVM;
            }
            set
            {
                _MainVM = value;
            }
        }

        private MainWindowViewModel2 _MainVM2;
        public MainWindowViewModel2 MainVM2
        {
            get
            {
                if (_MainVM2 == null)
                {
                    _MainVM2 = new MainWindowViewModel2();
                }
                return _MainVM2;
            }
            set
            {
                _MainVM2 = value;
            }
        }


    }
}
