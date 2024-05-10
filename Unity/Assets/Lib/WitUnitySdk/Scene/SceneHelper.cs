using Assets.Library.WitUnitySdk.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Library.WitUnitySdk.Scene
{
    /// <summary>
    /// 场景帮助类
    /// </summary>
    public class SceneHelper
    {
        /// <summary>
        /// 待传输的数据
        /// </summary>
        private static SceneParam SceneParams { get; set; } = new SceneParam();

        static SceneHelper()
        {
            // var ses = SceneManager.GetAllScenes();
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="param"></param>
        private static void WriteSceneParam(SceneParam param)
        {
            if (param == null)
            {
                param = new SceneParam();
            }
            SceneParams = param;
        }

        /// <summary>
        /// 取出数据
        /// </summary>
        /// <returns></returns>
        public static SceneParam ReadSceneParam()
        {
            return SceneParams;
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        public static void LoadScene(string sceneName)
        {
            LoadScene(sceneName, new SceneParam());
        }

        /// <summary>
        /// 加载场景并且传递参数
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="param"></param>
        public static void LoadScene(string sceneName, SceneParam param)
        {
            WriteSceneParam(param);
            SceneManager.LoadScene(sceneName);
        }
    }
}
