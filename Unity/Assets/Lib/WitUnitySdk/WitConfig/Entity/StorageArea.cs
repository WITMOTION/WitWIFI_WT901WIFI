using Assets.Library.WitUnitySdk.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.WitConfig.Entity
{
    /// <summary>
    /// 配置区
    /// </summary>
    public class StorageArea
    {
        /// <summary>
        /// 配置组
        /// </summary>
        public XmlSerializableDictionary<string, StorageGroup> Groups = new XmlSerializableDictionary<string, StorageGroup>();
    }
}
