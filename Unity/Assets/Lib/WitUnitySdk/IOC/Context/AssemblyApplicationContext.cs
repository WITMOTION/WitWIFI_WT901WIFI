using Assets.Library.WitUnitySdk.IOC.Attribute;
using Assets.Library.WitUnitySdk.IOC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Library.WitUnitySdk.IOC.Context
{
    /// <summary>
    /// 程序集应用程序上下文
    /// </summary>
    public class AssemblyApplicationContext : IApplicationContext
    {
        /// <summary>
        /// bean集合
        /// </summary>
        public Dictionary<string, object> BeanMap = new Dictionary<string, object>();

        #region  给外部操作bean的方法

        /// <summary>
        /// 获得实例，如果不存在示例会通过类型自行添加实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBean<T>()
        {
            Type type = typeof(T);

            if (BeanMap.ContainsKey(type.FullName))
            {
                return (T)BeanMap[type.FullName];
            }
            else
            {
                throw new Exception("不存在此 bean");
            }
        }

        /// <summary>
        /// 获得实例，如果不存在示例会通过类型自行添加实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public object GetBean(Type type)
        {
            if (BeanMap.ContainsKey(type.FullName))
            {
                return BeanMap[type.FullName];
            }
            else
            {
                throw new Exception("不存在此 bean");
            }
        }

        /// <summary>
        /// 添加实例
        /// </summary>
        /// <param name="bean"></param>
        public void AddBean(object bean)
        {
            Type type = bean.GetType();
            if (BeanMap.ContainsKey(type.FullName))
            {
                throw new Exception("已经存在相同名称的 bean");
            }
            else
            {
                BeanMap[type.FullName] = bean;
            }
        }

        /// <summary>
        /// 删除实例
        /// </summary>
        public void DeleteBean(object bean)
        {
            Type type = bean.GetType();
            if (BeanMap.ContainsKey(type.FullName))
            {
                BeanMap.Remove(type.FullName);
            }
        }

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public AssemblyApplicationContext(Assembly assembly)
        {
            ScanAssembly(assembly);
        }


        #region 扫描程序集

        /// <summary>
        /// 扫描指定程序集
        /// </summary>
        public void ScanAssembly(Assembly assembly)
        {

            // 扫描所有的类
            Type[] typeArr = assembly.GetTypes();
            foreach (Type type in typeArr)
            {
                // 不是抽象类
                if (!type.Attributes.HasFlag(TypeAttributes.Abstract | TypeAttributes.Abstract))
                {
                    MemberInfo info = type;
                    object[] attributes = info.GetCustomAttributes(true);
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        // 如果有Compoment注解
                        if (attributes[i].GetType().FullName.Equals(typeof(Compoment).FullName))
                        {
                            // 加入map
                            object bean = type.Assembly.CreateInstance(type.FullName);
                            if (!BeanMap.ContainsKey(type.FullName))
                            {
                                BeanMap[type.FullName] = bean;
                            }
                        }
                    }
                }
            }

            // 给bean属性赋值
            for (int i = 0; i < BeanMap.Count; i++)
            {
                KeyValuePair<string, object> keyValue = BeanMap.ElementAt(i);
                object obj = keyValue.Value;
                Type objType = obj.GetType();
                // 获得所有属性
                var properties = objType.GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    var prop = properties[j];
                    MemberInfo info = prop;
                    // 获得所有注解
                    object[] attributes = info.GetCustomAttributes(true);
                    for (int k = 0; k < attributes.Length; k++)
                    {
                        // 如果有Compoment注解
                        if (attributes[k].GetType().FullName.Equals(typeof(Resource).FullName))
                        {
                            var bean = GetBean(prop.PropertyType);
                            prop.SetValue(obj, bean);
                        }
                    }
                }
            }

            // 调用初始化方法
            for (int i = 0; i < BeanMap.Count; i++)
            {
                KeyValuePair<string, object> keyValue = BeanMap.ElementAt(i);
                object obj = keyValue.Value;
                Type objType = obj.GetType();
                // 获得所有属性
                var methodInfos = objType.GetMethods();
                for (int j = 0; j < methodInfos.Length; j++)
                {
                    var method = methodInfos[j];
                    MemberInfo info = method;
                    // 获得所有注解
                    object[] attributes = info.GetCustomAttributes(true);
                    for (int k = 0; k < attributes.Length; k++)
                    {
                        // 如果有PostConstruct注解
                        if (attributes[k].GetType().FullName.Equals(typeof(PostConstruct).FullName))
                        {
                            method.Invoke(obj, new object[] { });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获得所有实例
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetBeanMap()
        {

            return BeanMap;
        }

        #endregion

    }
}