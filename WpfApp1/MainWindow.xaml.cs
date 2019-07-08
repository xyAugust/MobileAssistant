using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.ViewModel;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using MahApps.Metro.Controls;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel mW;
        public MainWindow()
        {
            InitializeComponent();
            mW = ViewModelBss.Instance.MainVM;
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
