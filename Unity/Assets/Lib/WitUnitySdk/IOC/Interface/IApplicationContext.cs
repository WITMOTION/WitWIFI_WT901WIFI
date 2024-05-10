using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Library.WitUnitySdk.IOC.Interface
{

    /// <summary>
    /// 应用程序上下文接口
    /// </summary>
    public interface IApplicationContext
    {
        /// <summary>
        /// 获得实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetBean(Type type);

        /// <summary>
        /// 获得实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetBean<T>();

        /// <summary>
        /// 添加实例
        /// </summary>
        /// <param name="bean"></param>
        void AddBean(object bean);

        /// <summary>
        /// 获得所有实例
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetBeanMap();

    }

}