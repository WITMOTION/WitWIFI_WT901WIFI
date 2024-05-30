package entity;

/**
 * 设备数据实体
 *
 * @author: maoqiang
 * @date: 2024/3/15 15:02
 */
public class DeviceData {

    // 设备编号
    private String deviceNo;

    // 片上时间
    private String chipTime;

    // 加速度X
    private String accX;

    // 加速度Y
    private String accY;

    // 加速度Z
    private String accZ;

    // 角速度X
    private String gyroX;

    // 角速度Y
    private String gyroY;

    // 角速度Z
    private String gyroZ;

    // 角度X
    private String angleX;

    // 角度Y
    private String angleY;

    // 角度Z
    private String angleZ;

    // 磁场X
    private String magX;

    // 磁场Y
    private String magY;

    // 磁场Z
    private String magZ;

    // 温度
    private String temperature;

    // 电量
    private String electricity;

    // 信号
    private String signal;

    // 版本号
    private String firmwareVersion;

    public String getDeviceNo() {
        return deviceNo;
    }

    public void setDeviceNo(String deviceNo) {
        this.deviceNo = deviceNo;
    }

    public String getChipTime() {
        return chipTime;
    }

    public void setChipTime(String chipTime) {
        this.chipTime = chipTime;
    }

    public String getAccX() {
        return accX;
    }

    public void setAccX(String accX) {
        this.accX = accX;
    }

    public String getAccY() {
        return accY;
    }

    public void setAccY(String accY) {
        this.accY = accY;
    }

    public String getAccZ() {
        return accZ;
    }

    public void setAccZ(String accZ) {
        this.accZ = accZ;
    }

    public String getGyroX() {
        return gyroX;
    }

    public void setGyroX(String gyroX) {
        this.gyroX = gyroX;
    }

    public String getGyroY() {
        return gyroY;
    }

    public void setGyroY(String gyroY) {
        this.gyroY = gyroY;
    }

    public String getGyroZ() {
        return gyroZ;
    }

    public void setGyroZ(String gyroZ) {
        this.gyroZ = gyroZ;
    }

    public String getAngleX() {
        return angleX;
    }

    public void setAngleX(String angleX) {
        this.angleX = angleX;
    }

    public String getAngleY() {
        return angleY;
    }

    public void setAngleY(String angleY) {
        this.angleY = angleY;
    }

    public String getAngleZ() {
        return angleZ;
    }

    public void setAngleZ(String angleZ) {
        this.angleZ = angleZ;
    }

    public String getMagX() {
        return magX;
    }

    public void setMagX(String magX) {
        this.magX = magX;
    }

    public String getMagY() {
        return magY;
    }

    public void setMagY(String magY) {
        this.magY = magY;
    }

    public String getMagZ() {
        return magZ;
    }

    public void setMagZ(String magZ) {
        this.magZ = magZ;
    }

    public String getTemperature() {
        return temperature;
    }

    public void setTemperature(String temperature) {
        this.temperature = temperature;
    }

    public String getElectricity() {
        return electricity;
    }

    public void setElectricity(String electricity) {
        this.electricity = electricity;
    }

    public String getSignal() {
        return signal;
    }

    public void setSignal(String signal) {
        this.signal = signal;
    }

    public String getFirmwareVersion() {
        return firmwareVersion;
    }

    public void setFirmwareVersion(String firmwareVersion) {
        this.firmwareVersion = firmwareVersion;
    }
}
