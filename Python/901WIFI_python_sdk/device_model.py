# coding:UTF-8
# 设备实例 Device instance
class DeviceModel:
    # region 属性 attribute
    # 设备名称 deviceName
    deviceName = "我的设备"

    # 设备数据字典 Device Data Dictionary
    deviceData = {}

    # 设备是否开启
    isOpen = False

    # 临时数组 Temporary array
    TempBytes = []

    # endregion

    def __init__(self, deviceName, callback_method):
        print("Initialize device model")
        self.deviceName = deviceName
        self.isOpen = False
        self.callback_method = callback_method
        self.deviceData = {}

    # region 获取设备数据 Obtain device data
    # 设置设备数据 Set device data
    def set(self, key, value):
        # 将设备数据存到键值 Saving device data to key values
        self.deviceData[key] = value

    # 获得设备数据 Obtain device data
    def get(self, key):
        # 从键值中获取数据，没有则返回None Obtaining data from key values
        if key in self.deviceData:
            return self.deviceData[key]
        else:
            return None

    # 删除设备数据 Delete device data
    def remove(self, key):
        # 删除设备键值
        del self.deviceData[key]

    # endregion

    # region 数据解析 data analysis
    # 数据处理  Data processing
    def onDataReceived(self, data):
        if self.deviceName == bytes(data[:12]).decode('ascii'):
            # 时间
            self.set("Time", "20{}-{}-{} {}:{}:{}.{}".format(data[12], data[13], data[14], data[15], data[16], data[17], (data[19] << 8 | data[18])))
            # 加速度
            AccX = self.getSignInt16(data[21] << 8 | data[20]) / 32768 * 16
            AccY = self.getSignInt16(data[23] << 8 | data[22]) / 32768 * 16
            AccZ = self.getSignInt16(data[25] << 8 | data[24]) / 32768 * 16
            self.set("AccX", round(AccX, 3))
            self.set("AccY", round(AccY, 3))
            self.set("AccZ", round(AccZ, 3))
            # 角速度
            AsX = self.getSignInt16(data[27] << 8 | data[26]) / 32768 * 2000
            AsY = self.getSignInt16(data[29] << 8 | data[28]) / 32768 * 2000
            AsZ = self.getSignInt16(data[31] << 8 | data[30]) / 32768 * 2000
            self.set("AsX", round(AsX, 3))
            self.set("AsY", round(AsY, 3))
            self.set("AsZ", round(AsZ, 3))
            # 磁场
            GX = self.getSignInt16(data[33] << 8 | data[32]) * 100 / 1024
            GY = self.getSignInt16(data[35] << 8 | data[34]) * 100 / 1024
            GZ = self.getSignInt16(data[37] << 8 | data[36]) * 100 / 1024
            self.set("GX", round(GX, 3))
            self.set("GY", round(GY, 3))
            self.set("GZ", round(GZ, 3))
            # 角度
            AngX = self.getSignInt16(data[39] << 8 | data[38]) / 32768 * 180
            AngY = self.getSignInt16(data[41] << 8 | data[40]) / 32768 * 180
            AngZ = self.getSignInt16(data[43] << 8 | data[42]) / 32768 * 180
            self.set("AngleX", round(AngX, 2))
            self.set("AngleY", round(AngY, 2))
            self.set("AngleZ", round(AngZ, 2))
            # 温度
            Temp = self.getSignInt16(data[45] << 8 | data[44]) / 100
            self.set("Temp", round(Temp, 2))
            # 电量
            quantity = data[47] << 8 | data[46]
            if quantity > 396:
                self.set("ElectricPercentage", "100")
            elif 393 < quantity <= 396:
                self.set("ElectricPercentage", "90")
            elif 387 < quantity <= 393:
                self.set("ElectricPercentage", "75")
            elif 382 < quantity <= 387:
                self.set("ElectricPercentage", "60")
            elif 379 < quantity <= 382:
                self.set("ElectricPercentage", "50")
            elif 377 < quantity <= 379:
                self.set("ElectricPercentage", "40")
            elif 373 < quantity <= 377:
                self.set("ElectricPercentage", "30")
            elif 370 < quantity <= 373:
                self.set("ElectricPercentage", "20")
            elif 368 < quantity <= 370:
                self.set("ElectricPercentage", "15")
            elif 350 < quantity <= 368:
                self.set("ElectricPercentage", "10")
            elif 340 < quantity <= 350:
                self.set("ElectricPercentage", "5")
            elif quantity <= 340:
                self.set("ElectricPercentage", "0")
            # 信号
            rssi = self.getSignInt16(data[49] << 8 | data[48])
            self.set("Rssi", rssi)
            # 版本
            version = self.getSignInt16(data[51] << 8 | data[50])
            self.set("Version", version)
            self.callback_method(self)

    # 获得int16有符号数 Obtain int16 signed number
    @staticmethod
    def getSignInt16(num):
        if num >= pow(2, 15):
            num -= pow(2, 16)
        return num

    # endregion

