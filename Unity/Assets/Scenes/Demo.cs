using Assets;
using Assets.Device.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    // �����б� Search List
    public GameObject deviceScanResultProto;
    Transform scanResultRoot;
    // �����������б� Sensor Data List
    public GameObject deviceDataResultProto;
    Transform dataResultRoot;
    // ��Ϣ�ı� Message text
    public Text TextMsg;

    // ���������� Device Service
    DeviceService deviceService;
    // udp���� Udp Service
    UdpServer udpServer;
    // �豸����б� List of device components
    List<GameObject> deviceModels = new List<GameObject>();
    // �·��ֵ��豸�б� List of newly discovered devices
    List<DeviceModel> findList = new List<DeviceModel>();

    // Start is called before the first frame update
    void Start()
    {
        // ��udp��־�¼� Bind UDP log events
        udpServer = WitApplication.Context.GetBean<UdpServer>();
        udpServer.msgEvent.AddListener(OnMsg);
        // ���豸�����¼� Bind device search events
        deviceService = WitApplication.Context.GetBean<DeviceService>();
        deviceService.putDeviceEvent.AddListener(OnFindDevice);
        // ��ʼ���б� Initialize List
        scanResultRoot = deviceScanResultProto.transform.parent;
        deviceScanResultProto.transform.SetParent(null);
        dataResultRoot = deviceDataResultProto.transform.parent;
        deviceDataResultProto.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        // ����������豸����� If new devices are found, add them
        if (findList.Count > 0) {
            DeviceModel deviceModel = findList[0];
            GameObject g = Instantiate(deviceScanResultProto, scanResultRoot);
            g.name = deviceModel.DeivceId;
            g.transform.GetChild(0).GetComponent<Text>().text = deviceModel.DeivceId;
            findList.RemoveAt(0);

            GameObject d = Instantiate(deviceDataResultProto, dataResultRoot);
            d.name = deviceModel.DeivceId;
            d.transform.GetChild(0).GetComponent<Text>().text = deviceModel.DeivceId;
            d.transform.GetChild(1).GetComponent<Text>().text = GetDeviceData(deviceModel);
            deviceModels.Add(d);
        }

        // ����������豸���� Update added device data
        for (int i = 0; i < deviceModels.Count; i++)
        {
            GameObject d = deviceModels[i];
            d.transform.GetChild(1).GetComponent<Text>().text = GetDeviceData(deviceService.GetDevice(d.name));
        }
    }

    /// <summary>
    /// ��ȡ�豸���� Get device data
    /// </summary>
    private string GetDeviceData(DeviceModel deviceModel)
    {
        string Acc = $"AccX:{deviceModel.AccX}g\t\tAccY:{deviceModel.AccY}g\t\tAccZ:{deviceModel.AccZ}g\r\n";
        string As = $"AsX:{deviceModel.AsX}��/s\t\tAsY:{deviceModel.AsY}��/s\t\tAsZ:{deviceModel.AsZ}��/s\r\n";
        string Angle = $"AngleX:{deviceModel.AngleX}��\t\tAngleY:{deviceModel.AngleY}��\t\tAngleZ:{deviceModel.AngleZ}��\r\n";
        string Mag = $"HX:{deviceModel.HX}ut\t\tHY:{deviceModel.HY}ut\t\tHZ:{deviceModel.HZ}ut\r\n";
        string Electricity = $"Electricity:{deviceModel.Electricity}%";
        string data = Acc + As + Angle + Mag + Electricity;
        return data;
    }

    /// <summary>
    /// ��UDP���� Open UDP service
    /// </summary>
    public void StartUDP() {
        try
        {
            udpServer.StartReceive();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// �㲥���ͱ���IP��ַ Broadcast sending local IP address
    /// </summary>
    public void SendLoc() {
        try
        {
            udpServer.SendLoc();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// �ر�UDP���� Turn off UDP service
    /// </summary>
    public void StopUDP() {
        try
        {
            udpServer.StopReceive();
        }
        catch (Exception ex)
        {
            MonoBehaviour.print(ex.Message);
        }
    }

    /// <summary>
    /// �ҵ����豸ʱ��ִ��������� This method will be executed when a new device is found
    /// </summary>
    /// <param name="deviceModel"></param>
    private void OnFindDevice(DeviceModel deviceModel) 
    {
        findList.Add(deviceModel);  
    }

    /// <summary>
    /// ����Messageʱ��ִ��������� This method will be executed when updating the Message
    /// </summary>
    /// <param name="msg"></param>
    private void OnMsg(string msg) {
        try
        {
            TextMsg.text = msg;
        }
        catch (Exception)
        {
            return;
        }
    }
}
