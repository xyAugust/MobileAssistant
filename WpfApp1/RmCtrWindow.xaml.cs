using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using WpfApp1.scrcpy;
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
            

            System.Threading.Thread.Sleep(1000); 
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
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
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

        Form f1;
        IntPtr intptrChild = IntPtr.Zero;
        private void OpenClick(object sender, RoutedEventArgs e)
        {
            //ShowForm1(sender, e); 
            ShowForm3(sender, e); 
        }

        private void ShowForm3(object sender, RoutedEventArgs e)
        {
            IntPtr intptrParent = myUCParent.PanlParent.Handle;
            intptrChild = EmbeddedApp.FindWindow(null, "rk3399-mid"); 
            // 
            Thread tt = new Thread(() =>
            {
                while (true)
                {
                    if (intptrChild != IntPtr.Zero)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            EmbeddedApp.SetParent(intptrChild, intptrParent);
                            EmbeddedApp.MoveWindow(intptrChild, 0, 0, myUCParent.PanlParent.Width, myUCParent.PanlParent.Height, true);
                            EmbeddedApp.ShowWindow(intptrChild, 5);
                        }));

                        break;
                    }

                }
            });

            tt.IsBackground = true;
            tt.Start();
        }
         
        private void ShowForm1(object sender, RoutedEventArgs e)
        {
            IntPtr intptrParent = myUCParent.PanlParent.Handle;
            //
            f1 = new FormMain();
            f1.Show();
            intptrChild = f1.Handle;
            // 
            Thread tt = new Thread(() =>
            {
                while (true)
                {
                    if (intptrChild != IntPtr.Zero)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            EmbeddedApp.SetParent(intptrChild, intptrParent);
                            EmbeddedApp.MoveWindow(intptrChild, 0, 0, myUCParent.PanlParent.Width, myUCParent.PanlParent.Height, true);
                            EmbeddedApp.ShowWindow(intptrChild, 5);
                        }));

                        break;
                    }

                }
            });

            tt.IsBackground = true;
            tt.Start();
        }

        private void WindowsFormsHost1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (f1 == null) return;
            EmbeddedApp.MoveWindow(f1.Handle, 0, 0, myUCParent.PanlParent.Width, myUCParent.PanlParent.Height, true);
        }
    }
}
