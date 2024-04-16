# coding:UTF-8
from tcp_service import TcpService
from udp_service import UdpService


# 数据更新时会调用此方法 This method will be called when data is updated
def updateData(DeviceModel):
    # 直接打印出设备数据字典 Directly print out the device data dictionary
    # print(DeviceModel.deviceData)

    # 获得X轴加速度 Obtain X-axis acceleration
    print(DeviceModel.get("AngleX"))


# 示例程序主入口 Example program main entrance
if __name__ == '__main__':
    # TCP服务 TCP service
    server = TcpService(1399, updateData)
    # UDP服务 UDP service
    # server = UdpService(1399, updateData)
    server.start()

