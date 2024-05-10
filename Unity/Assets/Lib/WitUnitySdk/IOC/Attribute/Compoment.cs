using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.IOC.Attribute
{
    // 一个自定义特性 BugFix 被赋给类及其成员
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Compoment : System.Attribute
    {
        public Compoment()
        {
        }
    }
}

