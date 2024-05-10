using Assets.Library.WitUnitySdk.IOC.Attribute;
using Assets.Service.Device.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Device.Service
{
    /// <summary>
    /// 设备服务
    /// </summary>
    [Compoment]
    public class DeviceService
    {
        /// <summary>
        /// 设备字典 Device Dictionary
        /// </summary>
        private Dictionary<string, DeviceModel> DeviceDict = new Dictionary<string, DeviceModel>();

        /// <summary>
        /// 添加设备事件 Add device event
        /// </summary>
        public PutDeviceEvent putDeviceEvent = new PutDeviceEvent();

        /// <summary>
        /// 收到数据时 When receiving data
        /// </summary>
        /// <param name="data"></param>
        public void OnReceive(byte[] data)
        {
            byte[] buffer;
            while (data.Length >= 54 && data[0] == 0x57 && data[1] == 0x54)
            {
                buffer = data.Skip(0).Take(54).ToArray();
                DoHandle(buffer);
                data = data.Skip(54).ToArray();
            }
        }

        /// <summary>
        /// 处理数据 Processing data
        /// </summary>
        /// <param name="v"></param>
        private void DoHandle(byte[] package)
        {
            // MonoBehaviour.print(package);
            byte[] deviceNameLis = package.Skip(0).Take(12).ToArray();
            // 拿到设备ID Obtain device ID
            string deviceId = Encoding.UTF8.GetString(deviceNameLis, 0, deviceNameLis.Length);
            byte[] data = package.Skip(12).ToArray();
            DeviceModel deviceModel = GetOrPutDevice(deviceId);
            SetDeviceData(deviceModel, data);
        }

        /// <summary>
        /// 设置设备数据 Set device data
        /// </summary>
        private void SetDeviceData(DeviceModel deviceModel, byte[] data) {
            deviceModel.AccX = Math.Round((double)((short)(data[9] << 8 | data[8])) / 32768 * 16, 3);
            deviceModel.AccY = Math.Round((double)((short)(data[11] << 8 | data[10])) / 32768 * 16, 3);
            deviceModel.AccZ = Math.Round((double)((short)(data[13] << 8 | data[12])) / 32768 * 16, 3);

            deviceModel.AsX = Math.Round((double)((short)(data[15] << 8 | data[14])) / 32768 * 2000, 3);
            deviceModel.AsY = Math.Round((double)((short)(data[17] << 8 | data[16])) / 32768 * 2000, 3);
            deviceModel.AsZ = Math.Round((double)((short)(data[19] << 8 | data[18])) / 32768 * 2000, 3);

            deviceModel.HX = Math.Round((double)((short)(data[21] << 8 | data[20])) * 100 / 1024, 3);
            deviceModel.HY = Math.Round((double)((short)(data[23] << 8 | data[22])) * 100 / 1024, 3);
            deviceModel.HZ = Math.Round((double)((short)(data[25] << 8 | data[24])) * 100 / 1024, 3);

            deviceModel.AngleX = Math.Round((double)((short)(data[27] << 8 | data[26])) / 32768 * 180, 2);
            deviceModel.AngleY = Math.Round((double)((short)(data[29] << 8 | data[28])) / 32768 * 180, 2);
            deviceModel.AngleZ = Math.Round((double)((short)(data[31] << 8 | data[30])) / 32768 * 180, 2);

            short v = (short)(data[35] << 8 | data[34]);
            deviceModel.Electricity = GetElectricity(v);

            // 更新时间
            deviceModel.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 获得电量百分比 Obtaining battery percentage
        /// </summary>
        private int GetElectricity(short quantity)
        {
            int Electricity = 0;
            if (quantity > 396)
            {
                Electricity = 100;
            }
            else if (quantity > 393 && quantity <= 396)
            {
                Electricity = 90;
            }
            else if (quantity > 387 && quantity <= 393)
            {
                Electricity = 75;
            }
            else if (quantity > 382 && quantity <= 387)
            {
                Electricity = 60;
            }
            else if (quantity > 379 && quantity <= 382)
            {
                Electricity = 50;
            }
            else if (quantity > 377 && quantity <= 379)
            {
                Electricity = 40;
            }
            else if (quantity > 373 && quantity <= 377)
            {
                Electricity = 30;
            }
            else if (quantity > 370 && quantity <= 373)
            {
                Electricity = 20;
            }
            else if (quantity > 368 && quantity <= 370)
            {
                Electricity = 15;
            }
            else if (quantity > 350 && quantity <= 368)
            {
                Electricity = 10;
            }
            else if (quantity > 340 && quantity <= 350)
            {
                Electricity = 5;
            }
            else if (quantity <= 340)
            {
                Electricity = 0;
            }
            return Electricity;
        }

        /// <summary>
        /// 创建或者添加设备 Create or add devices
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private DeviceModel GetOrPutDevice(string deviceId)
        {
            DeviceModel deviceModel = GetDevice(deviceId);
            if (deviceModel == null)
            {
                PutDevice(new DeviceModel(deviceId));
                deviceModel = GetDevice(deviceId);
            }
            return deviceModel;
        }

        /// <summary>
        /// 增加一个设备 Add a device
        /// </summary>
        /// <param name="deviceModel"></param>
        private void PutDevice(DeviceModel deviceModel)
        {
            DeviceDict[deviceModel.DeivceId] = deviceModel;
            putDeviceEvent.Invoke(deviceModel);
        }

        /// <summary>
        /// 获得设备  Get Device
        /// </summary>
        /// <param name="deviceId"></param>
        public DeviceModel GetDevice(string deviceId) {
            if (deviceId == null) {
                return null;
            }

            if (DeviceDict.ContainsKey(deviceId)) {
                return DeviceDict[deviceId];
            }
            return null;
        }

        /// <summary>
        /// 获得所有设备
        /// </summary>
        /// <returns></returns>
        public List<DeviceModel> GetDeviceList() {
            return new List<DeviceModel>(DeviceDict.Values);
        }

    }
}