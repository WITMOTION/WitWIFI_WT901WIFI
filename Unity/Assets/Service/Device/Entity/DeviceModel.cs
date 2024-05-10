using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �豸ģ��
/// </summary>
public class DeviceModel
{
    /// <summary>
    /// �豸id
    /// </summary>
    public string DeivceId { get; private set; }

    /// <summary>
    /// ���ٶ�X
    /// </summary>
    public double AccX;

    /// <summary>
    /// ���ٶ�Y
    /// </summary>
    public double AccY;

    /// <summary>
    /// ���ٶ�Z
    /// </summary>
    public double AccZ;

    /// <summary>
    /// ���ٶ�X
    /// </summary>
    public double AsX;

    /// <summary>
    /// ���ٶ�Y
    /// </summary>
    public double AsY;

    /// <summary>
    /// ���ٶ�Z
    /// </summary>
    public double AsZ;

    /// <summary>
    /// �Ƕ�X
    /// </summary>
    public double AngleX;

    /// <summary>
    /// �Ƕ�Y
    /// </summary>
    public double AngleY;

    /// <summary>
    /// �Ƕ�Z
    /// </summary>
    public double AngleZ;

    /// <summary>
    /// �ų�X
    /// </summary>
    public double HX;

    /// <summary>
    /// �ų�Y
    /// </summary>
    public double HY;

    /// <summary>
    /// �ų�Z
    /// </summary>
    public double HZ;

    /// <summary>
    /// ����
    /// </summary>
    public int Electricity;


    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool IsOnline
    {
        get
        {
            // ���3���������ݾ������ߣ������������
            var ts = DateTime.Now - LastUpdateTime;
            return ts.TotalMilliseconds < 3000;
        }
    }

    /// <summary>
    /// �ϴ��յ�����ʱ��
    /// </summary>
    public DateTime LastUpdateTime = DateTime.MinValue;

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="deivceId"></param>
    public DeviceModel(string deivceId) {
        DeivceId = deivceId;
    }

}
