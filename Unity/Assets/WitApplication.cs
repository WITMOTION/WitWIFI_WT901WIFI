using Assets.Library.WitUnitySdk.IOC.Context;
using Assets.Library.WitUnitySdk.Language.Constant;
using Assets.Library.WitUnitySdk.Language.Context;
using Assets.Library.WitUnitySdk.WitConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{

    /// <summary>
    /// 应用程序启动先执行此类
    /// </summary>
    internal class WitApplication : MonoBehaviour
    {
        /// <summary>
        /// 应用程序上下文
        /// </summary>
        public static AssemblyApplicationContext Context { get; private set; }

        /// <summary>
        /// 程序版本
        /// </summary>
        public static string SoftwareVersion = "1.0.0";

        /// <summary>
        /// 启动时先执行此类
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Startup()
        {;
            // 加载语言
            LoadLanguage();
            // 初始化上下文
            Context = new AssemblyApplicationContext(typeof(WitApplication).Assembly);
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        private static void LoadLanguage()
        {
            string commandLine = Environment.CommandLine;

            print("commandLine: " + commandLine);

            if (string.IsNullOrEmpty(commandLine) == false && commandLine.Contains("lang=0"))
            {
                LanguageContext.Lang = LanguageConstant.ZHCN;
            }
            else if (string.IsNullOrEmpty(commandLine) == false && commandLine.Contains("lang=1"))
            {
                LanguageContext.Lang = LanguageConstant.EN;
            }
            else
            {
                LanguageContext.Lang = LanguageConstant.ZHCN;
            }
        }
    }
}
