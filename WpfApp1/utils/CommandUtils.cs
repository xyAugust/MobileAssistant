using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.utils
{
    class CommandUtils
    {

        Process p;
        public CommandUtils()
        {
            Init();
        }
        private void Init()
        {

            //实例一个Process类，启动一个独立进程
            p = new Process();

            //Process类有一个StartInfo属性，这个是ProcessStartInfo类，包括了一些属性和方法，下面我们用到了他的几个属性：

            p.StartInfo.FileName = "cmd.exe";           //设定程序名
            p.StartInfo.UseShellExecute = false;        //关闭Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向标准输入
            p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
            p.StartInfo.RedirectStandardError = true;   //重定向错误输出
            p.StartInfo.CreateNoWindow = false;         //设置不显示窗口
            p.Start();   //启动
            p.StartInfo.Arguments = "/c ";              //设定程式执行参数 

            ThreadPool.QueueUserWorkItem(new WaitCallback(StdOutput));
            ThreadPool.QueueUserWorkItem(new WaitCallback(StdErrOutput));
            p.StandardInput.WriteLine("echo off");
        }

        public void RunCmd(string cmd, Action<string> action)
        {
            p.StandardInput.WriteLine(cmd);
            Stdoutput = action;
        }
        Action<string> Stdoutput { get; set; }

        private void StdOutput(object obj)
        {
            try
            {
                StreamReader sr = p.StandardOutput;

                string line = "";
                int num = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        num++;
                        line = line + "\r\n";
                        Console.Write(line);
                        Stdoutput?.Invoke(line);
                    }
                }

                //char[] bs = new char[256];
                //int len;
                //while ((len = sr.Read(bs, 0, bs.Length)) != 0)
                //{
                //    char[] dest = new char[len];
                //    Array.Copy(bs, dest, len);
                //    string value = new string(dest);
                //    Console.Write(value);
                //    Stdoutput?.Invoke(value);
                //}

                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n;" + ex.StackTrace);
            }

        }


        public void StdErrOutput(object obj)
        {
            try
            { 
                StreamReader sr = p.StandardError;

                string line = "";
                int num = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        num++;
                        line = line + "\r\n";
                        Console.Write(line);
                        Stdoutput?.Invoke(line);
                    }
                }

                //char[] bs = new char[256];
                //int len;
                //while ((len = sr.Read(bs, 0, bs.Length)) != 0)
                //{
                //    char[] dest = new char[len];
                //    Array.Copy(bs, dest, len);
                //    string value = new string(dest);
                //    Console.Write(value);
                //    Stdoutput?.Invoke(value);
                //}

                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\r\n;" + ex.StackTrace);
            }

        }
    }
}
