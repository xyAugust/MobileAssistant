using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.ViewModel;

namespace WpfApp1.tcp
{
    class Client
    {
        Socket clientSocket;
        SendRcvHandler Handler;
        private void start()
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.BeginConnect(new IPEndPoint(ip, 12306), new AsyncCallback(Connect), clientSocket);
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
            }
        }
         
        private void Connect(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                socket.EndConnect(ar);
                Handler = new SendRcvHandler
                {
                    SSocket = clientSocket
                };
                Handler.Receive();
                CConnected();
                Console.WriteLine("连接服务器成功");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Close()
        {
            clientSocket.Close();                   //关闭连接并释放资源
        }

        private Action CConnected;

        public void Request(object obj)
        {
            start();
            RequestEntity requestEntity = (RequestEntity)obj;
            CConnected = delegate
            {
                Handler.ResponseAction = requestEntity.ResponseAction;
                Handler.Send(requestEntity.RequestUri);
            };
        }

        public void Request(string reqeust, Action<object> ResponseAction)
        { 
            ThreadPool.QueueUserWorkItem(new WaitCallback(Request), BuildRequest(reqeust, ResponseAction));
        }
        
        public static RequestEntity BuildRequest(string reqeust, Action<object> ResponseAction)
        {
            return new RequestEntity
            {
                RequestUri = reqeust,
                ResponseAction = ResponseAction
            };
        }
    }
}
