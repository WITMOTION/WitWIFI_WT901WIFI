using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.Utils
{


    /// <summary>
    /// 定制器
    /// </summary>
    class TimeoutHelper
    {
        /// <summary>
        /// 执行定时器
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time"></param>
        public static void SetTimeout(ThreadStart action, int time)
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(time);
                action.Invoke();
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
