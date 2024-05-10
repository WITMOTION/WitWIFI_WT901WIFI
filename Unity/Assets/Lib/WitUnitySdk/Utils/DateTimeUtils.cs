using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.Utils
{
    public class DateTimeUtils
    {

        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {

            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000 * 1000;
        }

    }
}
