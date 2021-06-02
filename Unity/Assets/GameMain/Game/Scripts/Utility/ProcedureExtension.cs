using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public static class ProcedureExtension
    {
        public static void SetChangeSceneData<T>(this ProcedureOwner procedureOwner, string nextSceneName,
            string nextSceneMusic) where T : ProcedureBase
        {
            procedureOwner.SetData<VarString>(Constant.ProcedureData.NextSceneName, nextSceneName);
            procedureOwner.SetData<VarString>(Constant.ProcedureData.NextSceneMusic, nextSceneMusic);
            procedureOwner.SetData<VarString>(Constant.ProcedureData.NextProcedureTypeFullName,
                typeof(T).FullName);
        }
    }
}