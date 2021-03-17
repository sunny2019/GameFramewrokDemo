using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class Procedure_Main : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("开启Main流程...");
            GameEntry.Scene.UnloadAllScene();
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId,OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId,OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId,OnLoadSceneUpdate);

            GameEntry.Scene.LoadScene("Assets/GameMain/Game/Scenes/Main.unity",this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId,OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId,OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId,OnLoadSceneUpdate);
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs re = (LoadSceneUpdateEventArgs) e;
            if (re.UserData!=this)
            {
                return;
            }
            Log.Info("加载进度："+re.Progress);
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs re = (LoadSceneFailureEventArgs) e;
            if (re.UserData!=this)
            {
                return;
            }
            Log.Info("加载成功："+re.SceneAssetName+"\t"+re.ErrorMessage);
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs re = (LoadSceneSuccessEventArgs) e;
            if (re.UserData!=this)
            {
                return;
            }
            Log.Info("加载成功:"+re.SceneAssetName+"\t"+re.Duration);
        }
    }
}