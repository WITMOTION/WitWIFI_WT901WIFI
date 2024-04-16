# coding:UTF-8
import socket
import threading

from device_model import DeviceModel


# UDP服务
class UdpService:
    port = 1399
    socket = None
    isOpen = False
    # 存储设备的字典 Dictionary of storage devices
    deviceList = {}
    # 存储临时数据 Store temporary data
    tempBuffer = []

    def __init__(self, port, callback_method):
        self.port = port
        self.isOpen = False
        self.callback_method = callback_method

    def start(self):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.socket.bind(("0.0.0.0", self.port))
        self.isOpen = True
        print('UDP服务器已启动...')
        # 开启一个线程读取数据 Start a thread to read data
        t = threading.Thread(target=self.onReceive)
        t.start()

    def onReceive(self):
        device_Id = ""
        # 开始监听
        while self.isOpen:
            try:
                # 接收数据，这将阻塞 Receive data, which will block
                data, address = self.socket.recvfrom(1024)
                for var in data:
                    self.tempBuffer.append(var)
                    # 必须是WT开头 Must start with WT
                    if len(self.tempBuffer) == 2 and (self.tempBuffer[0] != 0x57 or self.tempBuffer[1] != 0x54):
                        del self.tempBuffer[0]
                        continue
                    # 检测设备ID Detection device ID
                    if len(self.tempBuffer) == 12:
                        device_Id = bytes(self.tempBuffer).decode('ascii')
                        if device_Id not in self.deviceList:
                            self.deviceList[device_Id] = DeviceModel(device_Id, self.callback_method)
                    # 数据分包 Data subcontracting
                    if len(self.tempBuffer) == 54:
                        device = self.deviceList[device_Id]
                        device.onDataReceived(self.tempBuffer)
                        self.tempBuffer.clear()
            except:
                print("Error onReceive")

    # 停止服务 stop service
    def stop(self):
        self.isOpen = False
        try:
            self.socket.close()
        except:
            print("Error socket.close()")
