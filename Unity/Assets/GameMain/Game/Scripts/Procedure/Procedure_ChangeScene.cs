//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class Procedure_ChangeScene : ProcedureBase
    {
        //单场景模式,决定加载新场景时是否需要卸载所有场景
        //为true时加载新场景会自动卸载所有旧场景
        //为false时需要手动进行场景卸载（加载重复场景时不需要手动卸载）
        public  bool m_SingleSceneModel = true;
        
        private bool m_IsCloseUIFormComplete = false;
        private string m_SceneName = "";
        private string m_BackgroundMusicName = "";
        private string m_ProcedureFullName = "";
        private int m_ShowProgressUIFormID = -1;
        private List<string> m_UnloadingSceneName;
        private bool m_NeedWaitOnload = false;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_IsCloseUIFormComplete = false;
            m_SceneName = "";
            m_BackgroundMusicName = "";
            m_ProcedureFullName = "";
            m_ShowProgressUIFormID = -1;
            m_UnloadingSceneName = null;
            m_NeedWaitOnload = false;

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(CloseUIFormCompleteEventArgs.EventId, OnCloseUIForm);

            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);

            m_SceneName = procedureOwner.GetData<VarString>(Constant.ProcedureData.NextSceneName).Value;
            m_BackgroundMusicName = procedureOwner.GetData<VarString>(Constant.ProcedureData.NextSceneMusic).Value;
            m_ProcedureFullName = procedureOwner.GetData<VarString>(Constant.ProcedureData.NextProcedureTypeFullName)
                .Value;

            m_ShowProgressUIFormID = GameEntry.UI.OpenUIForm(AssetUtility.GetUIForm("UIForm_ShowProgress"),
                Constant.UIGroups.LobbyForLoading,
                Constant.AssetPriority.UIFormAsset, this);
        }

        private void OnCloseUIForm(object sender, GameEventArgs e)
        {
            CloseUIFormCompleteEventArgs ne = (CloseUIFormCompleteEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            m_IsCloseUIFormComplete = true;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(CloseUIFormCompleteEventArgs.EventId, OnCloseUIForm);

            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            GameEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);

            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnUnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            UnloadSceneSuccessEventArgs ne = (UnloadSceneSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            if (m_NeedWaitOnload)
            {
                //要加载的场景已卸载，则可重新加载
                if (ne.SceneAssetName == AssetUtility.GetScene(m_SceneName))
                {
                    LoadScene(m_SceneName);
                }
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsCloseUIFormComplete)
            {
                return;
            }

            ChangeState(procedureOwner, Type.GetType(m_ProcedureFullName));
        }

        private void UnLodeSceneAndResetSetting()
        {
            // 停止所有声音
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            // 卸载所有场景
            if (m_SingleSceneModel)
            {
                m_UnloadingSceneName = GameEntry.Scene.UnloadAllScene(this);
            }
            else
            {
                m_UnloadingSceneName = new List<string>();
                GameEntry.Scene.GetLoadedSceneAssetNames(m_UnloadingSceneName);
            }

            //如果要加载的场景是当前已加载的场景，则需要等待卸载完成，否则直接加载即可
            if (!m_UnloadingSceneName.Contains(AssetUtility.GetScene(m_SceneName)))
            {
                LoadScene(m_SceneName);
            }
            else
            {
                if (!m_SingleSceneModel)
                {
                    GameEntry.Scene.UnloadScene(AssetUtility.GetScene(m_SceneName),this);
                }
                m_NeedWaitOnload = true;
            }
        }

        private void LoadScene(string loadSceneName)
        {
            GameEntry.Scene.LoadScene(AssetUtility.GetScene(loadSceneName), Constant.AssetPriority.SceneAsset, this);
        }

        private void SetProgressUIForm(string content)
        {
            if (GameEntry.UI.HasUIForm(m_ShowProgressUIFormID))
            {
                ((UIForm_ShowProgress) GameEntry.UI.GetUIForm(m_ShowProgressUIFormID).Logic)
                    .SetProgressConttent(content);
            }
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));

            SetProgressUIForm(String.Format($"场景加载中(<color=yellow>{ne.Progress.ToString("P0")}</color>)..."));
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            //播放目标场景的音效
            if (!string.IsNullOrEmpty(m_BackgroundMusicName))
            {
                GameEntry.Sound.PlayMusic(m_BackgroundMusicName);
            }


            SetProgressUIForm(String.Format($"场景加载完成(<color=yellow>{1.ToString("P0")}</color>)..."));
            GameEntry.UI.CloseUIForm(m_ShowProgressUIFormID, this);
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName,
                ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());

            SetProgressUIForm(String.Format(
                $"资源加载中(<color=yellow>{((float) ne.LoadedCount / (float) ne.TotalCount).ToString("P0")}</color>)..."));
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Open uiform '{0}' OK.", ne.UIForm.UIFormAssetName);

            UnLodeSceneAndResetSetting();
        }
    }
}