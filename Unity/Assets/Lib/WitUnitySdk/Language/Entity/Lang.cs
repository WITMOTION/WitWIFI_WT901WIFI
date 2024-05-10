using Assets.Library.WitUnitySdk.Language.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.Language.Entity
{
    /// <summary>
    /// 语言包
    /// </summary>
    public class Lang
    {
        public string ZHCN = "";

        public string EN = "";

        public override string ToString()
        {
            return LanguageHelper.GetResult(this);
        }

    }
}
