using Assets.Library.WitUnitySdk.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Assets.Library.WitUnitySdk.WitConfig.Entity
{
    /// <summary>
    /// 配置组
    /// </summary>
    public class StorageGroup
    {
        /// <summary>
        /// 键值对
        /// </summary>
        public XmlSerializableDictionary<string, string> KeyValuePairs = new XmlSerializableDictionary<string, string>();
    }
}
