using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameMain
{
    public static partial class Constant
    {
        public static class UIGroups
        {
            /// <summary>
            /// 大厅以下 预留 目前没有使用到
            /// </summary>
            public const string Default = "Default";

            /// <summary>
            /// 大厅层
            /// </summary>
            public const string Lobby = "Lobby";

            /// <summary>
            /// 大厅上运营活动
            /// </summary>
            public const string LobbyFace = "LobbyFace";

            /// <summary>
            /// 大厅上各种外围系统层级
            /// </summary>
            public const string LobbyForSystem = "LobbyForSystem";

            /// <summary>
            /// 大厅上的各种邀请或者浮动界面 
            /// </summary>
            public const string LobbyForMatchingSystem = "LobbyForMatchingSystem";

            /// <summary>
            /// 游戏中各种通用弹框（低于loading页面）
            /// </summary>
            public const string LowLoadingCommonMessageBoxTips = "LowLoadingCommonMessageBoxTips";

            /// <summary>
            /// 各种loading页面层级
            /// </summary>
            public const string LobbyForLoading = "LobbyForLoading";

            /// <summary>
            /// 游戏中各种通用弹框层级（高于loading页面）
            /// </summary>
            public const string UpLoadingCommonMessageBoxTips = "UpLoadingCommonMessageBoxTips";

            /// <summary>
            /// 游戏中新手指引层级
            /// </summary>
            public const string LobbyForNewPlayerGuide = "LobbyForNewPlayerGuide";

            /// <summary>
            /// 获取所有UIGroup的名称
            /// </summary>
            /// <returns></returns>
            public static string[] GetAllUIGroupNames()
            {
                return new string[]
                {
                    Default,
                    Lobby,
                    LobbyFace,
                    LobbyForSystem,
                    LobbyForMatchingSystem,
                    LowLoadingCommonMessageBoxTips,
                    LobbyForLoading,
                    UpLoadingCommonMessageBoxTips,
                    LobbyForNewPlayerGuide,
                };
            }
        }
    }
}