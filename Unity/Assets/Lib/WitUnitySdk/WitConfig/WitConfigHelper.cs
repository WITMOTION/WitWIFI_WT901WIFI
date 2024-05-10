using Assets.Library.WitUnitySdk.WitConfig.Entity;
using Assets.Library.WitUnitySdk.WitConfig.Option;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.Library.WitUnitySdk.WitConfig
{
    /// <summary>
    /// 配置框架
    /// </summary>
    public class WitConfigHelper
    {

        /// <summary>
        /// 区缓存
        /// </summary>
        private static Dictionary<string, StorageArea> AreaCache = new Dictionary<string, StorageArea>();

        /// <summary>
        /// 分区锁
        /// </summary>
        private static Dictionary<string, object> AreaLocks = new Dictionary<string, object>();

        /// <summary>
        /// 配置文件目录
        /// </summary>
        private static string ConfigDirPath;

        /// <summary>
        /// 扩展名
        /// </summary>
        private static string Extension = ".wconf";

        /// <summary>
        /// 文件编码
        /// </summary>
        public static Encoding FileEncoding = Encoding.Default;

        #region 加载配置
        public static void LoadData(string configDirPath)
        {
            // 检查文件夹是否存在
            if (Directory.Exists(configDirPath) == false)
            {
                Directory.CreateDirectory(configDirPath);
            }
            ConfigDirPath = configDirPath;
        }

        #endregion

        #region 读取数据

        /// <summary>
        /// 读取为一个int值
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ReadToInt(string area, string group, string key, int defaultValue)
        {
            string stringValue = ReadToString(area, group, key, defaultValue.ToString());
            int value;
            if (int.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 读取为一个浮点值
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ReadToDouble(string area, string group, string key, double defaultValue)
        {
            string stringValue = ReadToString(area, group, key, defaultValue.ToString());
            double value;
            if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 读取为一个字符串
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ReadToString(string area, string group, string key, string defaultValue)
        {
            StorageArea configArea = GetArea(area);

            // 不存在组，或者不存在key都返回默认值
            if (configArea.Groups.ContainsKey(group) == false || configArea.Groups[group].KeyValuePairs.ContainsKey(key) == false)
            {
                return defaultValue;
            }

            string value = configArea.Groups[group].KeyValuePairs[key];
            return value;
        }

        /// <summary>
        /// 读取为一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ReadToObj<T>(string area, string group, string key, T defaultValue) where T : class
        {
            string stringValue = ReadToString(area, group, key, SerializerToString(defaultValue));

            try
            {
                T t = DeserializeString<T>(stringValue);
                return t;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            return defaultValue;
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteString(string area, string group, string key, string value)
        {
            StorageArea configArea = GetArea(area);

            if (configArea.Groups.ContainsKey(group) == false)
            {
                configArea.Groups[group] = new StorageGroup();
            }
            configArea.Groups[group].KeyValuePairs[key] = value;

            // 保存一个配置组
            SaveArea(area, configArea);
        }

        /// <summary>
        /// 写入浮点型值
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteDouble(string area, string group, string key, double value)
        {
            WriteString(area, group, key, value.ToString());
        }

        /// <summary>
        /// 写入整形数据
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteInt(string area, string group, string key, int value)
        {
            WriteString(area, group, key, value.ToString());
        }

        /// <summary>
        /// 写入一个对象
        /// </summary>
        /// <param name="area"></param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteObj<T>(string area, string group, string key, T value) where T : class
        {
            WriteString(area, group, key, SerializerToString(value));
        }



        #endregion

        #region 根据配置项读取配置

        /// <summary>
        /// 根据配置项，读取为一个int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static int ReadToInt(ConfigOption<int> option)
        {
            int value = ReadToInt(option.Area, option.Group, option.Key, option.DefaultValue);
            return value;
        }

        /// <summary>
        /// 根据配置项，读取为一个int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static double ReadToDouble(ConfigOption<double> option)
        {
            double value = ReadToDouble(option.Area, option.Group, option.Key, option.DefaultValue);
            return value;
        }

        /// <summary>
        /// 根据配置项，读取为一个string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string ReadToString(ConfigOption<string> option)
        {
            string value = ReadToString(option.Area, option.Group, option.Key, option.DefaultValue);
            return value;
        }

        /// <summary>
        /// 根据配置项，读取为一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T ReadToObj<T>(ConfigOption<T> option) where T : class
        {
            T value = ReadToObj(option.Area, option.Group, option.Key, option.DefaultValue);
            return value;
        }

        /// <summary>
        /// 根据配置项，写入一个对象
        /// </summary>
        /// <param name="option"></param>
        public static void WriteInt(ConfigOption<int> option, int value)
        {
            WriteInt(option.Area, option.Group, option.Key, value);
        }

        /// <summary>
        /// 根据配置项，写入一个对象
        /// </summary>
        /// <param name="option"></param>
        public static void WriteDouble(ConfigOption<double> option, double value)
        {
            WriteDouble(option.Area, option.Group, option.Key, value);
        }

        /// <summary>
        /// 根据配置项，写入一个对象
        /// </summary>
        /// <param name="option"></param>
        public static void WriteString(ConfigOption<string> option, string value)
        {
            WriteString(option.Area, option.Group, option.Key, value);
        }

        /// <summary>
        /// 根据配置项，写入一个对象
        /// </summary>
        /// <param name="option"></param>
        public static void WriteObj<T>(ConfigOption<T> option, T value) where T : class
        {
            WriteObj(option.Area, option.Group, option.Key, value);
        }

        #endregion

        #region 分区操作

        /// <summary>
        /// 获得一个区
        /// </summary>
        /// <param name="area"></param>
        public static StorageArea GetArea(string area)
        {
            // 锁住分区，防止多个线程同时操作同一个分区
            lock (GetAreaLock(area))
            {
                StorageArea configArea = null;

                if (AreaCache.ContainsKey(area))
                {
                    return AreaCache[area];
                }
                else
                {
                    configArea = LoadArea(area);
                }
                return configArea;
            }
        }

        /// <summary>
        /// 加载一个区
        /// </summary>
        /// <param name="area"></param>
        public static StorageArea LoadArea(string area)
        {
            // 锁住分区，防止多个线程同时操作同一个分区
            lock (GetAreaLock(area))
            {
                string filepath = ConfigDirPath + area + Extension;
                StorageArea configArea = null;
                try
                {
                    configArea = DeserializeFile<StorageArea>(filepath);
                }
                catch (Exception e)
                {
                    // 读取失败就创建一个新对象
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    configArea = new StorageArea();
                    SaveArea(area, configArea);
                }
                return configArea;
            }
        }

        /// <summary>
        /// 保存一个区
        /// </summary>
        /// <param name="area"></param>
        /// <param name="configArea"></param>
        public static void SaveArea(string area, StorageArea configArea)
        {
            // 锁住分区，防止多个线程同时操作同一个分区
            lock (GetAreaLock(area))
            {
                string filepath = ConfigDirPath + area + Extension;
                SerializerFile(filepath, configArea);
            }
        }

        /// <summary>
        /// 删除分区
        /// </summary>
        /// <param name="area"></param>
        public static void DeleteArea(string area)
        {
            // 锁住分区，防止多个线程同时操作文件
            lock (GetAreaLock(area))
            {
                string filepath = ConfigDirPath + area + Extension;
                // 刷新分区缓存
                RefreshCache(area);
                // 删除文件
                File.Delete(filepath);
            }
        }


        /// <summary>
        /// 说明：删除这个分区的缓存
        /// </summary>
        /// <param name="area"></param>
        public static void RefreshCache(string area)
        {
            // 锁住分区，防止多个线程同时操作文件
            lock (GetAreaLock(area))
            {
                AreaCache.Remove(area);
            }
        }

        /// <summary>
        /// 获得分区锁
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private static object GetAreaLock(string area)
        {

            // 不存在分区锁则创建
            if (AreaLocks.ContainsKey(area) == false)
            {
                AreaLocks[area] = new object();
            }

            return AreaLocks[area];
        }
        #endregion


        #region xml序列化和反序列化

        /// <summary>
        /// 反序列化一个文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="filepath">XML文件</param>
        /// <returns></returns>
        private static T DeserializeFile<T>(string filepath)
        {
            var xml = File.ReadAllText(filepath, FileEncoding);
            using (StringReader reader = new StringReader(xml))
            {
                var model = new XmlSerializer(typeof(T)).Deserialize(reader);
                return (T)model;
            }
        }

        /// <summary>
        /// 序列化到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="t"></param>
        private static void SerializerFile<T>(string file, T t)
        {
            Stream fs = new FileStream(file, FileMode.Create);
            using (XmlWriter writer = new XmlTextWriter(fs, FileEncoding))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, t);
            }
        }

        /// <summary>
        /// 序列化对象为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static string SerializerToString<T>(T t)
        {
            MemoryStream stream = new MemoryStream();
            using (XmlWriter writer = new XmlTextWriter(stream, FileEncoding))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, t);
                return FileEncoding.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 反序列化一个字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="content">XML文件内容</param>
        /// <returns></returns>
        public static T DeserializeString<T>(string content)
        {
            using (MemoryStream reader = new MemoryStream(FileEncoding.GetBytes(content)))
            {
                var model = new XmlSerializer(typeof(T)).Deserialize(reader);
                return (T)model;
            }
        }

        #endregion

    }
}
