using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.tcp;
using WpfApp1.utils;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel : NotificationBase
    {

        Window CtrWindow;
        CommandUtils CmdUtils;
        public Action<object> AAction;
        public DelegateCommand SelectItemChangedCommand { get; set; }
        public DelegateCommand RemoteCtrWindowCommand { get; set; }
        public DelegateCommand StartZDCommand { get; set; }
        public DelegateCommand StopZDCommand { get; set; }
        public DelegateCommand UninstallZDCommand { get; set; }
        public AdbOutputModel AdbOutputModel { get; set; }

        public AppInfo SelectItemData { set; get; }
        private ObservableCollection<AppInfo> appInfos;
        public ObservableCollection<AppInfo> AppInfos
        {
            get { return appInfos; }
            set { appInfos = value; }
        }

        public MainWindowViewModel()
        {
            CmdUtils = new CommandUtils();
            this.AdbOutputModel = new AdbOutputModel();
            SelectItemData = new AppInfo();
            appInfos = new ObservableCollection<AppInfo>();

            this.StartZDCommand = new DelegateCommand
            {
                ExecuteCommand = new Action<object>(StartZD)
            };
            this.StopZDCommand = new DelegateCommand
            {
                ExecuteCommand = new Action<object>(StopZD)
            };
            this.UninstallZDCommand = new DelegateCommand
            {
                ExecuteCommand = new Action<object>(UninstallZD)
            }; 
            SelectItemChangedCommand = new DelegateCommand
            {
                ExecuteCommand = new Action<object>(SelectItem)
            };
            RemoteCtrWindowCommand = new DelegateCommand {
                ExecuteCommand = new Action<object>(RemoteCtrWindow)
            };


            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestList));
        }

        private void RemoteCtrWindow(object obj)
        {
            if (CtrWindow == null || CtrWindow.IsVisible == false)
            {
                CtrWindow = new RmCtrWindow();
                CtrWindow.Show();
            }
            else
            {
                CtrWindow.Activate();
                CtrWindow.WindowState = WindowState.Normal;
            }
        }

        private void ForwardTcpProt(object obj)
        {
            string cmd = "adb forward tcp:12306 tcp:12307";
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void StartZD(object obj)
        {
            string cmd = "adb shell am start -n " + SelectItemData.PackageName + "/" + SelectItemData.LauncherName;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void StopZD(object obj)
        {
            string cmd = "adb shell am force-stop " + SelectItemData.PackageName;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        private void UninstallZD(object obj)
        {
            string cmd = "adb uninstall " + SelectItemData.PackageName;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }

        string OutputMsg;
        private void AdbExe(object obj)
        {
            OutputMsg = "";
            AdbOutputModel.StdOutPut = OutputMsg;
            string cmd = (string)obj;
            CmdUtils.RunCmd(cmd, new Action<string>((output) => {
                OutputMsg += output;
                AdbOutputModel.StdOutPut = OutputMsg;
                Console.WriteLine(output);
            })); 
        }

        private void SelectItem(object obj)
        {
            ListView lv = obj as ListView;
            AppInfo appInfo = lv.SelectedItem as AppInfo;
            SelectItemData.AppName = appInfo.AppName;
            SelectItemData.Icon = appInfo.Icon;
            SelectItemData.VersionName = appInfo.VersionName;
            SelectItemData.VersionCode = appInfo.VersionCode;
            SelectItemData.PackageName = appInfo.PackageName;
            SelectItemData.LauncherName = appInfo.LauncherName;
        }

        private void RequestList(object obj)
        {
            ForwardTcpProt(null); 

            new Client().Request(Constant.APP_INFO_LIST, UpdateList);
        }

        private void UpdateList(object obj)
        {
            string str = (string)obj;
            List<AppInfo> AppInfoList = JsonConvert.DeserializeObject<List<AppInfo>>(str);
            AddItem(AppInfoList);

            SelectItemChangedCommand = new DelegateCommand
            {
                ExecuteCommand = new Action<object>((p) =>
                {
                    ListView lv = p as ListView;
                    AppInfo appInfo = lv.SelectedItem as AppInfo;
                })
            };
        }

        private void AddItem(List<AppInfo> list)
        {
            if (list == null)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (AppInfo appInfo in list)
                {
                    appInfos.Add(appInfo);
                }
            }));
            RequestIcon(list);
        }

        private void RequestIcon(List<AppInfo> list)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestIconCallback));
        }

        private int i = 0;
        private void RequestIconCallback(object obj)
        {
            if (i >= AppInfos.Count)
            {
                return;
            }
            AppInfo ai = AppInfos[i];
            AAction = new Action<object>((p) =>
            {
                byte[] bytes = (byte[])p;
                UpdateIcon(ai, bytes);
                i++;
                RequestIconCallback(null);
            });
            string uri = Constant.APP_ICON + ai.PackageName;
            new Client().Request(uri, AAction);
        }

        private void UpdateIcon(AppInfo ai, byte[] bytes)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ai.Icon = ImageUtils.ByteArrayToBitmapImage(bytes);
            }));
        }

        public void InstallApk(string file)
        {
            string cmd = "adb install -r -t " + file;
            ThreadPool.QueueUserWorkItem(new WaitCallback(AdbExe), cmd);
        }
    }
}
