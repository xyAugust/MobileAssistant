using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    /// <summary>
    /// RmCtrWindown.xaml 的交互逻辑
    /// </summary>
    public partial class RmCtrWindow : MetroWindow
    {
        public RmCtrWindow()
        {
            this.DataContext = ViewModelBss.Instance.MainVM2;
            InitializeComponent();

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {

            }));
        }
         
         
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
         
        IntPtr intptrChild = IntPtr.Zero;
        bool IsStart = false;
        private void OpenClick(object sender, RoutedEventArgs e)
        {
            if (IsStart)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartSubWindow));
            IntPtr intptrParent = MyFormParent.PanlParent.Handle;
            ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateSubWindow), intptrParent);  
        }

        private void StartSubWindow(object state)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"D:\Programs\scrcpy-win64\scrcpy.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.StartInfo.UseShellExecute = false;        //关闭Shell的使用
            p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
            p.Start();
            StreamReader sr = p.StandardOutput;
            string line = sr.ReadLine();
            Console.WriteLine("scrcpy");
            Console.WriteLine(line);

        }

        private void UpdateSubWindow(object obj)
        {
            IntPtr intptrParent = (IntPtr)obj;
            while (true)
            {
                if (intptrChild != IntPtr.Zero)
                {
                    IsStart = true;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        long oldstyle = EmbeddedApp.GetWindowLong(intptrChild, EmbeddedApp.GWL_STYLE);
                        long style = oldstyle & (~(EmbeddedApp.WS_CAPTION | EmbeddedApp.WS_CAPTION_2));
                        EmbeddedApp.SetWindowLong(intptrChild, EmbeddedApp.GWL_STYLE, (UInt32)style);
                        EmbeddedApp.SetParent(intptrChild, intptrParent);
                        EmbeddedApp.MoveWindow(intptrChild, 0, 0, MyFormParent.PanlParent.Width, MyFormParent.PanlParent.Height, true);
                        EmbeddedApp.ShowWindow(intptrChild, 5);
                    }));

                    break;
                }
                else
                {
                    Thread.Sleep(100);
                    //intptrChild = Process.GetProcessesByName("scrcpy")[0].MainWindowHandle;
                    intptrChild = EmbeddedApp.FindWindow(null, "rk3399-mid");
                    Console.WriteLine("GetProcessesByName");
                }

            }
        }

        private void WindowsFormsHost1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (intptrChild == IntPtr.Zero) return;
            EmbeddedApp.MoveWindow(intptrChild, 0, 0, MyFormParent.PanlParent.Width, MyFormParent.PanlParent.Height, true);
        }
    }
}
