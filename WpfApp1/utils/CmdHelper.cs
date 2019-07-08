using System;
using System.Diagnostics;
using System.IO;

namespace WpfApp1.ViewModel
{
    class CmdHelper
    {
        public static string RunCmd(string command)
        {
            try
            {
                //实例一个Process类，启动一个独立进程
                Process p = new Process();

                //Process类有一个StartInfo属性，这个是ProcessStartInfo类，包括了一些属性和方法，下面我们用到了他的几个属性：

                p.StartInfo.FileName = "cmd.exe";           //设定程序名
                p.StartInfo.Arguments = "/c " + command;    //设定程式执行参数
                p.StartInfo.UseShellExecute = false;        //关闭Shell的使用
                p.StartInfo.RedirectStandardInput = true;   //重定向标准输入
                p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
                p.StartInfo.RedirectStandardError = true;   //重定向错误输出
                p.StartInfo.CreateNoWindow = true;          //设置不显示窗口

                p.Start();   //启动

                StreamReader sr;
                sr = p.StandardOutput;

                p.StandardInput.WriteLine("exit");        //不过要记得加上Exit要不然下一行程式执行的时候会当机

                return p.StandardOutput.ReadToEnd();        //从输出流取得命令执行结果
            }
            catch (Exception ex)
            {
                return ex.Message + "\r\n;" + ex.StackTrace;
            }

        }
    }
}
