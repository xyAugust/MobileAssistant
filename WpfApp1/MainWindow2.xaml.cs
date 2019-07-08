using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// MainWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : MetroWindow
    {
        private MainWindowViewModel2 mW;
        public MainWindow2()
        {
            InitializeComponent();
            mW = new MainWindowViewModel2();
            this.DataContext = mW;
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            string msg = "Drop";
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                msg = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            }
            mW.InstallApk(msg);
            MessageBox.Show(msg);
        }
    }
}
