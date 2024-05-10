using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设备模型
/// </summary>
public class DeviceModel
{
    /// <summary>
    /// 设备id
    /// </summary>
    public string DeivceId { get; private set; }

    /// <summary>
    /// 加速度X
    /// </summary>
    public double AccX;

    /// <summary>
    /// 加速度Y
    /// </summary>
    public double AccY;

    /// <summary>
    /// 加速度Z
    /// </summary>
    public double AccZ;

    /// <summary>
    /// 角速度X
    /// </summary>
    public double AsX;

    /// <summary>
    /// 角速度Y
    /// </summary>
    public double AsY;

    /// <summary>
    /// 角速度Z
    /// </summary>
    public double AsZ;

    /// <summary>
    /// 角度X
    /// </summary>
    public double AngleX;

    /// <summary>
    /// 角度Y
    /// </summary>
    public double AngleY;

    /// <summary>
    /// 角度Z
    /// </summary>
    public double AngleZ;

    /// <summary>
    /// 磁场X
    /// </summary>
    public double HX;

    /// <summary>
    /// 磁场Y
    /// </summary>
    public double HY;

    /// <summary>
    /// 磁场Z
    /// </summary>
    public double HZ;

    /// <summary>
    /// 电量
    /// </summary>
    public int Electricity;


    /// <summary>
    /// 是否在线
    /// </summary>
    public bool IsOnline
    {
        get
        {
            // 如果3秒内有数据就是在线，否则就是离线
            var ts = DateTime.Now - LastUpdateTime;
            return ts.TotalMilliseconds < 3000;
        }
    }

    /// <summary>
    /// 上次收到数据时间
    /// </summary>
    public DateTime LastUpdateTime = DateTime.MinValue;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="deivceId"></param>
    public DeviceModel(string deivceId) {
        DeivceId = deivceId;
    }

}
