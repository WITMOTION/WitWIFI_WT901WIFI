using Assets.Library.WitUnitySdk.IOC.Attribute;
using Assets.Service.Device.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


namespace Assets.Device.Service
{
    [Compoment]
    public class UdpServer
    {
        /// <summary>
        /// 设备服务 Device Service
        /// </summary>
        [Resource]
        public DeviceService DeviceService { get; set; }

        /// <summary>
        /// 日志事件 Log events
        /// </summary>
        public MsgEvent msgEvent = new MsgEvent();

        ~UdpServer()
        {
            StopReceive();
        }

        [PostConstruct]
        public void Init()
        {
            //// 开始接数据
            //StartReceive();
            //MonoBehaviour.print("UDP服务初始化完成");
        }

        /// <summary>
        /// 用于UDP发送的网络服务类 Network service class for UDP sending
        /// </summary>
        private UdpClient udpcRecv = null;

        /// <summary>
        /// 本地IP Local IP
        /// </summary>
        private IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1399);
        /// <summary>
        /// 远程IP Remote IP
        /// </summary>
        private IPEndPoint remoteIpep;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        private bool IsUdpcRecvStart = false;


        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        private Thread thrRecv;


        /// <summary>
        /// 开始接收数据 Start receiving data
        /// </summary>
        public void StartReceive()
        {
            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {
                try
                {
                    udpcRecv = new UdpClient(localIpep);
                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.IsBackground = true;
                    IsUdpcRecvStart = true;
                    thrRecv.Start();
                    Print($"UDP监听启动成功 address:{localIpep.Address}  port:{localIpep.Port}");
                    MonoBehaviour.print("UDP服务初始化完成");
                }
                catch (Exception e)
                {
                    Print("UDP监听启动失败" + e.Message);
                }
            }
        }


        /// <summary>
        /// 停止服务 Stop receiving data
        /// </summary>
        public void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                udpcRecv.Close();
                udpcRecv = null;  
                IsUdpcRecvStart = false;
                Print("UDP监听器已成功关闭");
            }
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="s"></param>
        private void Print(string s)
        {
            //MonoBehaviour.print(s);
            msgEvent.Invoke(s); 
        }

        /// <summary>
        /// 接收数据 Receive data 
        /// </summary>
        private void ReceiveMessage()
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    // string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);

                    if (bytRecv.Length < 1)
                    {
                        return;
                    }
                    DeviceService.OnReceive(bytRecv);
                }
                catch (ThreadAbortException ex)
                {
                    // Print(ex.Message);
                    return;
                }
                catch(Exception ex) {
                    Print(ex.Message);
                }
            }
        }

        /// <summary>
        /// 发送udp数据 Send UDP data
        /// </summary>
        private void SendMessage(byte[] data, string hostname, int port) {
            // MonoBehaviour.print(hostname);
            udpcRecv.Send(data, data.Length, hostname, port);
        }

        /// <summary>
        /// 发送本地地址 Send local address
        /// </summary>
        public void SendLoc() {
            if (udpcRecv == null) {
                Print("Udp未初始化！");
                return;
            }
            string ip = GetLocIp();
            if (ip == null) {
                return;            
            }
            try
            {
                List<string> lis = ip.Split(".").ToList();
                for (int i = 0; i < 255; i++)
                {
                    lis.RemoveAt(lis.Count - 1);
                    lis.Add(i.ToString());
                    string remote = string.Join(".", lis);
                    string msg = $"WIT{ip}\r\n";
                    SendMessage(Encoding.UTF8.GetBytes(msg), remote, 9250);
                }
                Print("发送本地IP完成");
            }
            catch (Exception ex)
            {
                Print("发送本地IP失败");
            }

        }

        /// <summary>
        /// 获取本机IP Obtain local IP address
        /// </summary>
        /// <returns></returns>
        private string GetLocIp() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }
    }
}