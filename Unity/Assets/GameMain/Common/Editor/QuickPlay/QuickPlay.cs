using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMain
{
    public class QuickPlay
    {
        private static string startScenePath = "Assets/GameMain/Common/Scenes/GameMain.unity";

        [RuntimeInitializeOnLoadMethod]
        public static void EditorQuickPlay()
        {
            if (SceneManager.GetActiveScene().path != startScenePath &&
                EditorPrefs.GetBool("QuickPlayGFFramework", true))
            {
                GameObject[] dontDesObjs = GameObjectHelper.GetDontDestroyOnLoadGameObjects();
                for (int i = 0; i < dontDesObjs.Length; i++)
                {
                    GameObject.Destroy(dontDesObjs[i]);
                }

                SceneManager.LoadScene(startScenePath);
            }
        }


        //Set CheckMark
        [MenuItem("Game Framework/QuickPlay/是", true)]
        public static bool CheckQuickPlay()
        {
            bool quickPlay = EditorPrefs.GetBool("QuickPlayGFFramework", true);
            Menu.SetChecked("Game Framework/QuickPlay/是", quickPlay);
            Menu.SetChecked("Game Framework/QuickPlay/否", !quickPlay);
            return true;
        }

        [MenuItem("Game Framework/QuickPlay/是")]
        public static void SwitchToQuick()
        {
            SwitchToQuickPrefs(true);
        }

        [MenuItem("Game Framework/QuickPlay/否")]
        public static void SwitchToNoQuick()
        {
            SwitchToQuickPrefs(false);
        }

        private static void SwitchToQuickPrefs(bool targetKeyword)
        {
            EditorPrefs.SetBool("QuickPlayGFFramework", targetKeyword);
        }
    }
}