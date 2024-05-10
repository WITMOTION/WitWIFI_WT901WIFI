using Assets.Library.WitUnitySdk.WitConfig;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Library.WitUnitySdk.Utils
{
    /// <summary>
    /// 对象复制工具类
    /// </summary>
    public class ObjectCopyUtils
    {
        /// <summary>
        /// 使用UNITY的json克隆对象
        /// </summary>
        public static T JsonUtilityCloneObject<T>(T obj)
        {
            string json = JsonUtility.ToJson(obj);
            T clone = JsonUtility.FromJson<T>(json);
            return clone;
        }

        /// <summary>
        /// 使用witconfighelper克隆对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T WitConfigHelperCloneObject<T>(T obj)
        {
            string str = WitConfigHelper.SerializerToString(obj);
            T clone = WitConfigHelper.DeserializeString<T>(str);
            return clone;
        }

        /// <summary>
        /// 使用内存克隆
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bFormatter.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)bFormatter.Deserialize(stream);
        }
    }
}