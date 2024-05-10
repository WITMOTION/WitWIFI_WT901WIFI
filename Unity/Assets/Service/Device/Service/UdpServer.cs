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
        /// �豸���� Device Service
        /// </summary>
        [Resource]
        public DeviceService DeviceService { get; set; }

        /// <summary>
        /// ��־�¼� Log events
        /// </summary>
        public MsgEvent msgEvent = new MsgEvent();

        ~UdpServer()
        {
            StopReceive();
        }

        [PostConstruct]
        public void Init()
        {
            //// ��ʼ������
            //StartReceive();
            //MonoBehaviour.print("UDP�����ʼ�����");
        }

        /// <summary>
        /// ����UDP���͵���������� Network service class for UDP sending
        /// </summary>
        private UdpClient udpcRecv = null;

        /// <summary>
        /// ����IP Local IP
        /// </summary>
        private IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1399);
        /// <summary>
        /// Զ��IP Remote IP
        /// </summary>
        private IPEndPoint remoteIpep;

        /// <summary>
        /// ���أ��ڼ���UDP���Ľ׶�Ϊtrue������Ϊfalse
        /// </summary>
        private bool IsUdpcRecvStart = false;


        /// <summary>
        /// �̣߳����ϼ���UDP����
        /// </summary>
        private Thread thrRecv;


        /// <summary>
        /// ��ʼ�������� Start receiving data
        /// </summary>
        public void StartReceive()
        {
            if (!IsUdpcRecvStart) // δ�������������ʼ����
            {
                try
                {
                    udpcRecv = new UdpClient(localIpep);
                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.IsBackground = true;
                    IsUdpcRecvStart = true;
                    thrRecv.Start();
                    Print($"UDP���������ɹ� address:{localIpep.Address}  port:{localIpep.Port}");
                    MonoBehaviour.print("UDP�����ʼ�����");
                }
                catch (Exception e)
                {
                    Print("UDP��������ʧ��" + e.Message);
                }
            }
        }


        /// <summary>
        /// ֹͣ���� Stop receiving data
        /// </summary>
        public void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                thrRecv.Abort(); // �����ȹر�����̣߳�������쳣
                udpcRecv.Close();
                udpcRecv = null;  
                IsUdpcRecvStart = false;
                Print("UDP�������ѳɹ��ر�");
            }
        }

        /// <summary>
        /// ��ӡ��Ϣ
        /// </summary>
        /// <param name="s"></param>
        private void Print(string s)
        {
            //MonoBehaviour.print(s);
            msgEvent.Invoke(s); 
        }

        /// <summary>
        /// �������� Receive data 
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
        /// ����udp���� Send UDP data
        /// </summary>
        private void SendMessage(byte[] data, string hostname, int port) {
            // MonoBehaviour.print(hostname);
            udpcRecv.Send(data, data.Length, hostname, port);
        }

        /// <summary>
        /// ���ͱ��ص�ַ Send local address
        /// </summary>
        public void SendLoc() {
            if (udpcRecv == null) {
                Print("Udpδ��ʼ����");
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
                Print("���ͱ���IP���");
            }
            catch (Exception ex)
            {
                Print("���ͱ���IPʧ��");
            }

        }

        /// <summary>
        /// ��ȡ����IP Obtain local IP address
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