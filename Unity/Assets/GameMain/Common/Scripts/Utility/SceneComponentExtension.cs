using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public static class SceneComponentExtension 
{
    /// <summary>
    /// 卸载所有场景
    /// </summary>
    /// <param name="scene"></param>
    public static List<string> UnloadAllScene(this SceneComponent scene,object userData)
    {
        string[] loadedSceneAssetNames = scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            scene.UnloadScene(loadedSceneAssetNames[i],userData);
        }

        return new List<string>(loadedSceneAssetNames);
    }
}
