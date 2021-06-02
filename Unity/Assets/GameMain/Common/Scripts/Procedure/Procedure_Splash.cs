//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class Procedure_Splash : ProcedureBase
    {
        public bool SplashOver;
        public bool ChangeOver;


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // TODO: 增加一个 Splash 动画，这里先跳过
            SplashOver = true;
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (SplashOver && !ChangeOver)
            {
                if (GameEntry.Base.EditorResourceMode)
                {
                    //编辑器模式直接开始预加载
                    ChangeState(procedureOwner, typeof(Procedure_Preload));
                }
                else
                {
                    if (GameEntry.Resource.ResourceMode == ResourceMode.Package)
                    {
                        GameEntry.Resource.InitResources(() =>
                        {
#if UNITY_WEBGL
                            //WebGL平台单机模式进行资源预请求
                            ChangeState(procedureOwner, typeof(Procedure_WebglPreRequest));
#else
                            //非WebGL平台单机模式
                            ChangeState(procedureOwner,typeof(Procedure_Preload));
#endif
                        });
                    }
                    else
                    {
#if UNITY_WEBGL
                        //WebGL平台可更新模式提示不支持,并关闭
                        Log.Fatal("WebGL platform does not support resource hot update mode.");
#else
                        //非WebGL平台可更新模式
                    ChangeState(procedureOwner, typeof(Procedure_CheckVersion));
#endif
                    }
                }

                ChangeOver = true;
            }
        }
    }
}