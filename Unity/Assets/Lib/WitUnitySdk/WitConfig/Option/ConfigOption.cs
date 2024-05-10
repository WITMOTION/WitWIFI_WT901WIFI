using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.WitConfig.Option
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class ConfigOption<T>
    {
        /// <summary>
        /// 分区
        /// </summary>
        public virtual string Area { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public T DefaultValue { get; set; }
    }
}
