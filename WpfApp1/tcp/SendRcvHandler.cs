using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;

namespace WpfApp1.tcp
{
    class SendRcvHandler
    {
        public Action<object> ResponseAction { set; get; } 
        public Socket SSocket { get; set; }

        public void Send(String data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            Send(byteData);
        }

        public void Send(byte[] byteData)
        {
            if (SSocket.Connected)
            {
                SSocket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), SSocket);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

        }
        
        internal void Receive()
        {
            if (SSocket.Connected)
            {
                SSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), SSocket);
            }
        }


        byte[] buffer = new byte[2048];
        const string DATA_HEAD = "_!@#_HEAD";
        const string DATA_TAIL = "_!@#_TAIL";
        const int TYPE_STRING = 0x01;
        const int TYPE_BYTES = 0x02;
        readonly int HeadInfoLen = DATA_HEAD.Length + 8;

        ResponseEntity RcvEntity = new ResponseEntity();

        public void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;

                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.endreceive.aspx
                var length = socket.EndReceive(ar);

                byte[] ValidData = null;
                if (RcvEntity.DataHead == null)
                {
                    if (length >= HeadInfoLen)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, DATA_HEAD.Length);
                        if (DATA_HEAD.Equals(message))
                        {
                            RcvEntity.DataHead = message;
                            if (buffer[DATA_HEAD.Length] == TYPE_STRING)
                            {
                                RcvEntity.ContentType = TYPE_STRING;
                            }
                            else if (buffer[DATA_HEAD.Length] == TYPE_BYTES)
                            {
                                RcvEntity.ContentType = TYPE_BYTES;
                            }
                            byte[] dataLen = new byte[4];
                            Array.Copy(buffer, HeadInfoLen - 4, dataLen, 0, dataLen.Length);
                            RcvEntity.TotalLength = BytesToInt2(dataLen, 0);

                            RcvEntity.CurrentLength += (length - HeadInfoLen);

                            if (length > HeadInfoLen)
                            {
                                ValidData = new byte[length - HeadInfoLen];
                                Array.Copy(buffer, HeadInfoLen, ValidData, 0, ValidData.Length);
                                AddData(ValidData);
                            }
                        }
                    }
                }
                else
                {
                    ValidData = new byte[length];
                    Array.Copy(buffer, 0, ValidData, 0, ValidData.Length);
                    AddData(ValidData);

                    RcvEntity.CurrentLength += length;
                    if (RcvEntity.CurrentLength >= RcvEntity.TotalLength)
                    {
                        ParseData();
                        ClearData();
                    }
                }

                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static int BytesToInt2(byte[] src, int offset)
        {
            int value;
            value = (int)(((src[offset] & 0xFF) << 24)
                    | ((src[offset + 1] & 0xFF) << 16)
                    | ((src[offset + 2] & 0xFF) << 8)
                    | (src[offset + 3] & 0xFF));
            return value;
        }


        public void AddData(byte[] bytes)
        {
            if (bytes == null)
            {
                return;
            }
            if (RcvEntity.BytesData == null)
            {
                RcvEntity.BytesData = new List<byte>();
            }
            // Console.WriteLine("dest len " + bytes.Length);
            RcvEntity.BytesData.AddRange(bytes);
        }

        public void ClearData()
        {
            RcvEntity.TotalLength = 0;
            RcvEntity.CurrentLength = 0;
            RcvEntity.DataHead = null;
            if (RcvEntity.BytesData == null)
            {
                return;
            }
            RcvEntity.BytesData.Clear();
        }

        private void ParseData()
        {
            byte[] bytes = RcvEntity.BytesData.ToArray();
            if (RcvEntity.ContentType == TYPE_STRING)
            {
                // TODO 解析数据
                var str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                Console.WriteLine(str);
                ResponseAction?.Invoke(str);
            }
            else
            if (RcvEntity.ContentType == TYPE_BYTES)
            {
                ResponseAction?.Invoke(bytes);
            }
        }

    }

}
