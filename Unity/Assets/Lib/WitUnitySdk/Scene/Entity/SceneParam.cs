using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.Scene.Entity
{
    /// <summary>
    /// 场景参数
    /// </summary>
    public class SceneParam
    {
        private Dictionary<string, string> Dict { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value)
        {
            Dict[key] = value;
        }

        /// <summary>
        /// 获得参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public string Get(string key, string defaultValue)
        {
            if (Dict.ContainsKey(key))
            {
                return Dict[key];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 重新toString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Dict.Count; i++)
            {
                var kv = Dict.ElementAt(i);
                s += kv.Key + ": " + kv.Value + "\r\n";
            }
            return s;
        }
    }
}
