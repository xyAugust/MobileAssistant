using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    /// <summary>
    /// RmCtrWindown.xaml 的交互逻辑
    /// </summary>
    public partial class RmCtrWindow : MetroWindow
    {
        bool open;
        public RmCtrWindow()
        {
            this.DataContext = ViewModelBss.Instance.MainVM2;
            InitializeComponent();
            ThreadPool.QueueUserWorkItem(new WaitCallback(Cap));
        }

        public void ShowImage()
        {
            if (tmp == null)
            {
                return;
            }
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.  
            bi.BeginInit();
            bi.UriSource = new Uri(@tmp, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            img.Source = bi;
        }

        string tmp;
        private void Cap(object obj)
        {
            open = true;
            for (int i = 0; ; i++)
            {
                if (!open)
                {
                    break;
                }
                string cmd = "adb shell screencap -p /sdcard/123/tmp.png";
                tmp = Environment.CurrentDirectory + "//tmp.png" + i;
                string cmd2 = "adb pull /sdcard/123/tmp.png " + tmp;
                AdbExe(cmd);
                AdbExe(cmd2);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ShowImage();
                }));
            }
             
        }

        private void AdbExe(object obj)
        {
            string cmd = (string)obj;
            string output = CmdHelper.RunCmd(cmd);
            Console.WriteLine(output);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            open = false;
        }
    }
}
