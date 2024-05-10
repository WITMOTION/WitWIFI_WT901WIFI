using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library.WitUnitySdk.MsgCenter
{

    /// <summary>
    /// 消息中心帮助类
    /// </summary>
    public class MessageCenterHelper
    {
        /// <summary>
        /// 消息活动
        /// </summary>
        public delegate void MessageAction();

        /// <summary>
        /// 待消费的消息活动
        /// </summary>
        private static Dictionary<string, List<MessageAction>> MessageDict = new Dictionary<string, List<MessageAction>>();


        /// <summary>
        /// 推送数据，但是没有动作
        /// </summary>
        /// <param name="key"></param>
        public static void PutMessage(string key)
        {
            PutMessage(key, () => { });
        }

        /// <summary>
        /// 推送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public static void PutMessage(string key, MessageAction action)
        {

            List<MessageAction> messageActions = null;
            if (MessageDict.ContainsKey(key))
            {
                messageActions = MessageDict[key];
            }
            else
            {
                messageActions = new List<MessageAction>();
                MessageDict[key] = messageActions;
            }
            messageActions.Add(action);
        }

        /// <summary>
        /// 拉取消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public static MessageAction PullMessage(string key)
        {
            List<MessageAction> messageActions = null;
            if (MessageDict.ContainsKey(key))
            {
                messageActions = MessageDict[key];
            }
            else
            {
                messageActions = new List<MessageAction>();
                MessageDict[key] = messageActions;
            }

            if (messageActions.Count > 0)
            {
                var action = messageActions[0];
                messageActions.Remove(action);
                return action;
            }

            return null;
        }

        /// <summary>
        /// 清除消息
        /// </summary>
        /// <param name="key"></param>
        public static void ClearMessage(string key)
        {
            List<MessageAction> messageActions = null;
            if (MessageDict.ContainsKey(key))
            {
                messageActions = MessageDict[key];
                messageActions.Clear();
            }
        }
    }
}
