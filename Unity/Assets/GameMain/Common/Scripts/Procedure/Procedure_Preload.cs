//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameFramework;
using GameFramework.Resource;
using IFix.Core;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class Procedure_Preload : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            
            m_LoadedFlag.Clear();

            PreloadResources();
        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            IEnumerator<bool> iter = m_LoadedFlag.Values.GetEnumerator();
            while (iter.MoveNext())
            {
                if (!iter.Current)
                {
                    return;
                }
            }

            procedureOwner.SetChangeSceneData<Procedure_Main>("Main","music_main");
            ChangeState<Procedure_ChangeScene>(procedureOwner);
        }


        private void PreloadResources()
        {
            LoadInject("Assets/GameMain/Common/InjectFix/ByteFiles/Assembly-CSharp.patch.bytes");

            LoadInject("Assets/GameMain/Common/InjectFix/ByteFiles/Assembly-CSharp-filepass.patch.bytes");
            // Preload fonts
            LoadFont("Assets/GameMain/Common/Fonts/MainFont.ttf");
        }


        /// <summary>
        /// 更新代码
        /// </summary>
        private void LoadInject(string bytesAssetName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("ByteFiles.{0}", bytesAssetName), false);
            GameEntry.Resource.LoadAsset(bytesAssetName, Constant.AssetPriority.InjectFixAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    byte[] bytes = ((TextAsset) asset).bytes;
                    if (bytes.Length != 0)
                    {
                        Log.Info($"patching {bytesAssetName} ...");
                        var sw = Stopwatch.StartNew();
                        PatchManager.Load(new MemoryStream(bytes));
                        Log.Info($"patch {bytesAssetName}, using " + sw.ElapsedMilliseconds + " ms");
                    }
                    else
                    {
                        Log.Info("Load bytes '{0}' Failure.", assetName);
                    }

                    m_LoadedFlag[Utility.Text.Format("ByteFiles.{0}", bytesAssetName)] = true;
                },
                (assetName, status, errorMessage, userData) =>
                {
                    m_LoadedFlag[Utility.Text.Format("ByteFiles.{0}", bytesAssetName)] = true;
                    Log.Info("Can not load bytes {0} from '{1}' with error message '{2}'.", bytesAssetName, assetName, errorMessage);
                }));
        }


        private void LoadFont(string fontAssetName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("Font.{0}", fontAssetName), false);
            GameEntry.Resource.LoadAsset(fontAssetName, Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    m_LoadedFlag[Utility.Text.Format("Font.{0}", fontAssetName)] = true;
                    UGuiForm.SetMainFont((Font) asset);
                    Log.Info("Load font '{0}' OK.", fontAssetName);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontAssetName, assetName, errorMessage);
                }));
        }
    }
}