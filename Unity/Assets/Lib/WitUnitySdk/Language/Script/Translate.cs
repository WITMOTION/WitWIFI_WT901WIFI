using Assets.Library.WitUnitySdk.Language.Constant;
using Assets.Library.WitUnitySdk.Language.Context;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Library.WitUnitySdk.Language.Script
{
    internal class Translate : MonoBehaviour
    {
        public string ZHCN = "";

        public string EN = "";

        /// <summary>
        /// 翻译
        /// </summary>
        public void Start()
        {
            Text text = transform.GetComponent<Text>();
            if (text == null)
            {
                return;
            }
            text.text = GetResult();
        }

        /// <summary>
        /// 获得语言
        /// </summary>
        public string GetResult()
        {

            // 如果是英文环境，并且包含这样的key
            if (LanguageContext.Lang == LanguageConstant.EN)
            {
                return EN;
            }
            else
            {
                return ZHCN;
            }
        }
    }
}
