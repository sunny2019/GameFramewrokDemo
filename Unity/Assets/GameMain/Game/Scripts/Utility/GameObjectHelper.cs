using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMain
{
    public static class GameObjectHelper
    {
        public static GameObject[] GetDontDestroyOnLoadGameObjects()
        {
            List<GameObject> allGameObjects = GameObject.FindObjectsOfType<GameObject>().ToList();
            //移除当前加载场景所包含的物体
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                GameObject[] objsInScene = scene.GetRootGameObjects();
                for (var j = 0; j < objsInScene.Length; j++)
                {
                    allGameObjects.Remove(objsInScene[j]);
                }
            }

            //移除父级不为null的对象
            int k = allGameObjects.Count;
            while (--k >= 0)
            {
                if (allGameObjects[k].transform.parent != null)
                {
                    allGameObjects.RemoveAt(k);
                }
            }

            return allGameObjects.ToArray();
        }
    }
}