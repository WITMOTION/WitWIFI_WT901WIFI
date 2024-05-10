using Assets.Library.WitUnitySdk.Language.Constant;
using Assets.Library.WitUnitySdk.Language.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Library.WitUnitySdk.Language.Script
{
    /// <summary>
    /// 翻译语言
    /// </summary>
    public class TranslationLanguage : MonoBehaviour
    {

        /// <summary>
        /// 需要翻译的控件
        /// </summary>
        public List<GameObject> GameObjectList = new List<GameObject>();

        /// <summary>
        /// 语言定义
        /// </summary>
        public List<string> LanguageKeyValueList = new List<string>();

        /// <summary>
        /// 语言字典
        /// </summary>
        public Dictionary<string, string> LanguageDic = new Dictionary<string, string>();

        /// <summary>
        /// 分割符
        /// </summary>
        public string Delimiter = "=";

        public void Start()
        {

            // 初始化语言字典
            for (int i = 0; i < LanguageKeyValueList.Count; i++)
            {
                string keyvalue = LanguageKeyValueList[i];

                string[] keyValueSplit = keyvalue.Split(Delimiter);

                if (keyValueSplit.Length == 2)
                {
                    string key = keyValueSplit[0];
                    string value = keyValueSplit[1];

                    if (LanguageDic.ContainsKey(key))
                    {
                        Debug.LogError(key + "已经存在");
                    }

                    LanguageDic.Add(key, value);
                }
                else
                {
                    Debug.LogError(keyvalue + "无法拆分为语言");
                }
            }

            // 翻译所有控件
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObject gameObject = GameObjectList[i];

                TransObject(gameObject);
            }
        }

        /// <summary>
        /// 翻译控件
        /// </summary>
        /// <param name="gameObject"></param>
        private void TransObject(GameObject gameObject)
        {
            Text text = gameObject.GetComponent<Text>();
            if (text == null)
            {
                return;
            }
            text.text = GetResult(text.text);
        }

        /// <summary>
        /// 获得语言
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetResult(string key)
        {

            // 如果是英文环境，并且包含这样的key
            if (LanguageContext.Lang == LanguageConstant.EN && LanguageDic.ContainsKey(key))
            {
                return LanguageDic[key];
            }
            else
            {
                return key;
            }
        }
    }
}
