using GameFramework.Event;
using GameFramework.Fsm;
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
            Log.Info("Welcome Procedure_Main");


            #region Test

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowEntitySuccessEvent);
            
            GameEntry.Entity.AddEntityGroup("EugenGroup", 5.0f, 15, 5.0f, 1);
            GameEntry.Entity.ShowEntity<Entity_Eugen>(1, AssetUtility.GetEntity("Eugen"), "EugenGroup",this);
            GameEntry.Entity.AddEntityGroup("BoomGroup", 5.0f, 15, 5.0f, 1);
            GameEntry.Entity.ShowEntity<Entity_Boom>(2, AssetUtility.GetEntity("Boom"), "BoomGroup",this);

            #endregion
        }

        #region Test

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowEntitySuccessEvent);
            base.OnLeave(procedureOwner, isShutdown);
        }


        private bool playerLoaded = false;
        private bool boomLoaded = false;

        private void ShowEntitySuccessEvent(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = e as ShowEntitySuccessEventArgs;
            if (ne.UserData != this)
            {
                return;
            }

            if (ne.Entity.EntityAssetName == AssetUtility.GetEntity("Eugen"))
            {
                playerLoaded = true;
                ne.Entity.Logic.CachedTransform.position = SceneRef.Ins.Get("trans_Player").position;
                ne.Entity.Logic.CachedTransform.rotation = SceneRef.Ins.Get("trans_Player").rotation;
            }

            if (ne.Entity.EntityAssetName == AssetUtility.GetEntity("Boom"))
            {
                boomLoaded = true;
                ne.Entity.Logic.CachedTransform.position = SceneRef.Ins.Get("trans_Boom").position;
                ne.Entity.Logic.CachedTransform.rotation = SceneRef.Ins.Get("trans_Boom").rotation;

            }

            if (playerLoaded && boomLoaded)
            {
                GameEntry.UI.OpenUIForm(AssetUtility.GetUIForm("UIForm_Demo"), Constant.UIGroups.LobbyForSystem,
                    Constant.AssetPriority.UIFormAsset);
            }
        }

        #endregion
    }
}