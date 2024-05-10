using Assets.Library.WitUnitySdk.Language.Constant;
using Assets.Library.WitUnitySdk.Language.Context;
using Assets.Library.WitUnitySdk.Language.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.Language.Utils
{
    /// <summary>
    /// 语言帮助类
    /// </summary>
    internal class LanguageHelper
    {
        /// <summary>
        /// 获得语言
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string GetResult(Lang words)
        {

            // 如果是英文环境，并且包含这样的key
            if (LanguageContext.Lang == LanguageConstant.EN)
            {
                return words.EN;
            }
            else
            {
                return words.ZHCN;
            }
        }
    }
}
