//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace GameMain
{
    public static class AssetUtility
    {
        public static string GetConfig(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Configs/{0}.{1}", assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDataTable(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/Game/DataTables/{0}.{1}", assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDictionary(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language.ToString(), assetName, loadType == LoadType.Text ? "xml" : "bytes");
        }

        public static string GetFont(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Fonts/{0}.ttf", assetName);
        }

        public static string GetScene(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Scenes/{0}.unity", assetName);
        }

        public static string GetMusic(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Music/{0}.mp3", assetName);
        }

        public static string GetSound(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Sounds/{0}.wav", assetName);
        }

        public static string GetEntity(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/Entities/{0}.prefab", assetName);
        }

        public static string GetUIForm(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/UI/UIForms/{0}.prefab", assetName);
        }

        public static string GetUISound(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Game/UI/UISounds/{0}.wav", assetName);
        }
    }
}
