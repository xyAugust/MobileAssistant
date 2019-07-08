using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.ViewModel
{
    class AppInfo : NotificationBase
    {
        private string _AppName;
        public string AppName
        {
            get
            {
                return _AppName;
            }
            set
            {
                _AppName = value;
                this.RaisePropertyChanged("AppName");
            }
        }

        private string _LauncherName;
        public string LauncherName
        {
            get
            {
                return _LauncherName;
            }
            set
            {
                _LauncherName = value;
                this.RaisePropertyChanged("LauncherName");
            }
        }


        private string _PackageName;
        public string PackageName
        {
            get
            {
                return _PackageName;
            }
            set
            {
                _PackageName = value;
                this.RaisePropertyChanged("PackageName");
            }
        }

        private int _VersionCode;
        public int VersionCode
        {
            get
            {
                return _VersionCode;
            }
            set
            {
                _VersionCode = value;
                this.RaisePropertyChanged("VersionCode");
            }
        }

        private string _VersionName;
        public string VersionName
        {
            get
            {
                return _VersionName;
            }
            set
            {
                _VersionName = value;
                this.RaisePropertyChanged("VersionName");
            }
        }

        private BitmapImage _Icon;
        public BitmapImage Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
                this.RaisePropertyChanged("Icon");
            }
        }
    }
}
