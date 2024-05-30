import entity.DeviceData;

import java.io.IOException;
import java.io.InputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

public class TcpServer {

    private static ServerSocket serverSocket;
    private static boolean running = false;

    /**
     * 开启TCP连接
     *
     * @author: maoqiang
     * @date: 2024/3/15 16:41
     */
    public static void main(String[] args) {
        start(1890);
    }

    /**
     * 开启TCP服务器
     *
     * @author: maoqiang
     * @date: 2024/3/15 16:42
     */
    public static void start(int port) {
        try {
            serverSocket = new ServerSocket(port);
            running = true;
            System.out.println("开启TCP服务器成功 ");
            while (running) {
                Socket clientSocket = serverSocket.accept();

                new Thread(() -> {
                    handleClient(clientSocket);
                }).start();
            }
        } catch (IOException e) {
            e.printStackTrace();
            System.out.println("开启TCP服务器失败：" + e.getMessage());
        } finally {
            if (serverSocket != null) {
                try {
                    serverSocket.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    /**
     * 接收数据
     *
     * @author: maoqiang
     * @date: 2024/3/15 16:42
     */
    private static void handleClient(Socket clientSocket) {
        try (InputStream inputStream = clientSocket.getInputStream()) {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = inputStream.read(buffer)) != -1) {
                byte[] receivedData = new byte[bytesRead];
                System.arraycopy(buffer, 0, receivedData, 0, bytesRead);
                // 处理接收的数据
                processData(receivedData);
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                clientSocket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    /**
     * 解算数据
     *
     * @author: maoqiang
     * @date: 2024/3/15 16:42
     */
    private static void processData(byte[] data) {
        // 未解算的数据组
        List<List<Byte>> dataByteList = new ArrayList<>();
        // 未解算的单条数据
        List<Byte> dataBytes = new ArrayList<>();
        for (int i = 0; i < data.length; i++) {
            dataBytes.add(data[i]);

            if(i != 0){
                // 单条数据以'\r\n'结尾
                if(data[i - 1] == '\r' && data[i] == '\n'){
                    List<Byte> oneData = dataBytes;
                    dataByteList.add(oneData);
                    dataBytes = new ArrayList<>();
                }
            }
        }

        // 已经解算的设备数据
        List<DeviceData> dataEntityList = new ArrayList<>();
        for (int i = 0; i < dataByteList.size(); i++) {
            List<Byte> item = dataByteList.get(i);
            DeviceData entity = new DeviceData();
            // 单条数据以'WT'开始
            if(item.size() == 54 && item.get(0) == 'W' && item.get(1) == 'T'){
                // 设备编号解析
                byte[] noBytes = new byte[]{item.get(0),item.get(1),item.get(2),item.get(3),item.get(4),item.get(5),item.get(6),item.get(7),item.get(8),item.get(9),item.get(10),item.get(11)};
                String deviceNo = new String(noBytes);
                entity.setDeviceNo(deviceNo);

                // 片上时间解析
                String dateTime = (item.get(12) & 0xFF) + "-" +  (item.get(13) & 0xFF) + "-" + (item.get(14) & 0xFF) + " " + (item.get(15) & 0xFF) + ":" + (item.get(16) & 0xFF) + ":" + (item.get(17) & 0xFF) + "." + ((item.get(19) << 8) | (item.get(18) & 0xFF));
                entity.setChipTime(dateTime);

                // 加速度解析
                double accX = (short) ((item.get(21) << 8) | (item.get(20) & 0xFF)) / 32768.0 * 16.0;
                double accY = (short) ((item.get(23) << 8) | (item.get(22) & 0xFF)) / 32768.0 * 16.0;
                double accZ = (short) ((item.get(25) << 8) | (item.get(24) & 0xFF)) / 32768.0 * 16.0;
                entity.setAccX(String.format("%.4f", accX));
                entity.setAccY(String.format("%.4f", accY));
                entity.setAccZ(String.format("%.4f", accZ));

                // 角速度解析
                double gyroX = (short) ((item.get(27) << 8) | (item.get(26) & 0xFF)) / 32768.0 * 2000.0;
                double gyroY = (short) ((item.get(29) << 8) | (item.get(28) & 0xFF)) / 32768.0 * 2000.0;
                double gyroZ = (short) ((item.get(31) << 8) | (item.get(30) & 0xFF)) / 32768.0 * 2000.0;
                entity.setGyroX(String.format("%.4f", gyroX));
                entity.setGyroY(String.format("%.4f", gyroY));
                entity.setGyroZ(String.format("%.4f", gyroZ));

                // 磁场解析
                double magX = (short) ((item.get(33) << 8) | (item.get(32) & 0xFF));
                double magY = (short) ((item.get(35) << 8) | (item.get(34) & 0xFF));
                double magZ = (short) ((item.get(37) << 8) | (item.get(36) & 0xFF));
                entity.setMagX(String.format("%.4f", magX));
                entity.setMagY(String.format("%.4f", magY));
                entity.setMagZ(String.format("%.4f", magZ));

                // 角度解析
                double angleX = (short) ((item.get(39) << 8) | (item.get(38) & 0xFF)) / 32768.0 * 180.0;
                double angleY = (short) ((item.get(41) << 8) | (item.get(40) & 0xFF)) / 32768.0 * 180.0;
                double angleZ = (short) ((item.get(43) << 8) | (item.get(42) & 0xFF)) / 32768.0 * 180.0;
                entity.setAngleX(String.format("%.4f", angleX));
                entity.setAngleY(String.format("%.4f", angleY));
                entity.setAngleZ(String.format("%.4f", angleZ));

                // 温度解析
                double temp = (short) ((item.get(45) << 8) | (item.get(44) & 0xFF)) / 100.0;
                entity.setTemperature(String.format("%.2f", temp));

                // 电量解析
                double electricity = (short) ((item.get(47) << 8) | (item.get(46) & 0xFF)) / 100.0;
                entity.setElectricity(String.format("%.2f", electricity));

                // 信号解析
                double signal = (short) ((item.get(49) << 8) | (item.get(48) & 0xFF));
                entity.setSignal(String.format("%.2f", signal));

                // 版本号解析
                double firmwareVersion = (short) ((item.get(51) << 8) | (item.get(50) & 0xFF));
                entity.setFirmwareVersion(String.format("%.0f", firmwareVersion));

                // 打印数据
                System.out.println(entity.getDeviceNo() + "," + entity.getChipTime()
                        + "," + entity.getAccX() + "," + entity.getAccY() + "," + entity.getAccZ()
                        + "," + entity.getGyroX() + "," + entity.getGyroY() + "," + entity.getGyroZ()
                        + "," + entity.getMagX() + "," + entity.getMagY() + "," + entity.getMagZ()
                        + "," + entity.getAngleX() + "," + entity.getAngleY() + "," + entity.getAngleZ()
                        + "," + entity.getTemperature() + "," + entity.getElectricity() + "," + entity.getSignal() + "," + entity.getFirmwareVersion());
                dataEntityList.add(entity);
            }
        }
    }

    /**
     * 关闭TCP服务器
     *
     * @author: maoqiang
     * @date: 2024/3/15 16:42
     */
    public static void stop() {
        running = false;
        try {
            if (serverSocket != null) {
                serverSocket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}