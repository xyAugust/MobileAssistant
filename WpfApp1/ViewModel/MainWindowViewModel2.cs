using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel2 : NotificationBase
    {

        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand MinCommand { get; set; }
        public DelegateCommand MaxCommand { get; set; }

        public DelegateCommand HomeCommand { get; set; }
        public DelegateCommand StartZDCommand { get; set; }
        public DelegateCommand StopZDCommand { get; set; }
        public DelegateCommand UninstallZDCommand { get; set; }

        public AdbOutputModel AdbOutputModel { get; set; }


        public MainWindowViewModel2()
        {
            this.HomeCommand = new DelegateCommand();
            this.StartZDCommand = new DelegateCommand();
            this.StopZDCommand = new DelegateCommand();
            this.UninstallZDCommand = new DelegateCommand();
            
            this.HomeCommand.ExecuteCommand = new Action<object>(Home);
            this.StartZDCommand.ExecuteCommand = new Action<object>(StartZD);
            this.StopZDCommand.ExecuteCommand = new Action<object>(StopZD);
            this.UninstallZDCommand.ExecuteCommand = new Action<object>(UninstallZD);

            this.AdbOutputModel = new AdbOutputModel();
        }
         

        private void Home(object obj)
        {
            string cmd = "adb shell am start -n 'com.android.launcher3/com.android.launcher3.Launcher'";
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd); 
        }

        private void StartZD(object obj)
        {
            string cmd = "adb shell am start -n 'com.commonrail.mft.decoder/com.commonrail.mft.decoder.activity.SplashActivity'";
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void StopZD(object obj)
        {
            string cmd = "adb shell am force-stop com.commonrail.mft.decoder";
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void UninstallZD(object obj)
        {
            string cmd = "adb uninstall com.commonrail.mft.decoder";
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void AdbExe(object obj )
        {
            string cmd = (string)obj;
            string output = CmdHelper.RunCmd(cmd);
            AdbOutputModel.StdOutPut = output;
            Console.WriteLine(output);
        }

        public void InstallApk(string file)
        { 
            string cmd = "adb install -r -t " + file;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }
        
    }
}
