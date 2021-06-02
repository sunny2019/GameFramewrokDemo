using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    /// <summary>
    /// WebGL平台单机模式进行资源预请求
    /// </summary>
    public class Procedure_WebglPreRequest : ProcedureBase
    {
        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();
        private int m_UpdateCount = 0;
        private long m_UpdateTotalZipLength = 0L;
        private int m_UpdateSuccessCount = 0;
        private List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>();
        private UIForm_UpdateResource m_UpdateResourceForm = null;


        /// <summary>
        /// 进入状态时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_UpdateResourceForm = null;
            m_UpdateCount = 0;
            m_UpdateTotalZipLength = 0L;
            m_UpdateSuccessCount = 0;
            m_UpdateLengthData.Clear();
            m_UpdateResourceForm = null;
            
            GameEntry.Event.Subscribe(WebRequestStartEventArgs.EventId, OnWebRequestStart);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            WebRequestResource();
        }

        private void WebRequestResource()
        {
            if (m_UpdateResourceForm == null)
            {
                m_UpdateResourceForm = Object.Instantiate(GameEntry.BuiltinData.UpdateResourceFormTemplate);
            }
            
            Log.Info("Start request resource ...");

            IResourceGroup resourceGroup = GameEntry.Resource.GetResourceGroup();
            m_UpdateCount = resourceGroup.TotalCount;
            m_UpdateTotalZipLength = resourceGroup.TotalZipLength;
            string[] resNames = resourceGroup.GetResourceNames();
            for (int i = 0; i < resNames.Length; i++)
            {
                m_LoadedFlag.Add(Utility.Path.GetRemotePath(System.IO.Path.Combine(Application.streamingAssetsPath, resNames[i])), false);
                GameEntry.WebRequest.AddWebRequest(Utility.Path.GetRemotePath(System.IO.Path.Combine(Application.streamingAssetsPath, resNames[i])) ,this);
            }
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }
            
            Log.Error("Resquest '{0}' Failure,Error: '{1}'.", ne.WebRequestUri,ne.ErrorMessage );

            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.WebRequestUri)
                {
                    m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
                    RefreshProgress();
                    return;
                }
            }
            
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }

            Log.Info("Resquest '{0}' OK.", ne.WebRequestUri);
            
            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.WebRequestUri)
                {
                    m_UpdateLengthData[i].Length = ne.GetWebResponseBytes().Length;
                    m_UpdateSuccessCount++;
                    RefreshProgress();
                    m_LoadedFlag[ne.WebRequestUri] = true;
                    return;
                }
            }
            
        }

        private void OnWebRequestStart(object sender, GameEventArgs e)
        {
            WebRequestStartEventArgs ne = (WebRequestStartEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }
            
            Log.Info("Start Resquest '{0}'.", ne.WebRequestUri);
            
            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name == ne.WebRequestUri)
                {
                    Log.Warning("Update resource '{0}' is invalid.", ne.WebRequestUri);
                    m_UpdateLengthData[i].Length = 0;
                    RefreshProgress();
                    return;
                }
            }
            
            m_UpdateLengthData.Add(new UpdateLengthData(ne.WebRequestUri));
        }


        /// <summary>
        /// 状态轮询时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
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

        /// <summary>
        /// 离开状态时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否是关闭状态机时触发。</param>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_UpdateResourceForm != null)
            {
                Object.Destroy(m_UpdateResourceForm.gameObject);
                m_UpdateResourceForm = null;
            }
            
            GameEntry.Event.Unsubscribe(WebRequestStartEventArgs.EventId, OnWebRequestStart);
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        private void RefreshProgress()
        {
            long currentTotalUpdateLength = 0L;
            for (int i = 0; i < m_UpdateLengthData.Count; i++)
            {
                currentTotalUpdateLength += m_UpdateLengthData[i].Length;
            }

            float progressTotal = (float) currentTotalUpdateLength / m_UpdateTotalZipLength;
            string descriptionText = string.Format("{0}/{1}, {2}/{3}, {4:P0}, {5}/s", m_UpdateSuccessCount.ToString(), m_UpdateCount.ToString(),
                GetLengthString(currentTotalUpdateLength), GetLengthString(m_UpdateTotalZipLength), progressTotal, GetLengthString((int) GameEntry.Download.CurrentSpeed));
            m_UpdateResourceForm.SetProgress(progressTotal, descriptionText);
        }
        
        private string GetLengthString(long length)
        {
            if (length < 1024)
            {
                return string.Format("{0} Bytes", length.ToString());
            }

            if (length < 1024 * 1024)
            {
                return string.Format("{0} KB", (length / 1024f).ToString("F2"));
            }

            if (length < 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", (length / 1024f / 1024f).ToString("F2"));
            }

            return string.Format("{0} GB", (length / 1024f / 1024f / 1024f).ToString("F2"));
        }

        /// <summary>
        /// 状态销毁时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}