using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class Procedure_Luncher : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            //构建信息：发布版本时，把一些数据以 Json 的格式写入 Assets/GameMain/Configs/BuildInfo.txt，供游戏逻辑读取。
            GameEntry.BuiltinData.InitBuildInfo();
            ChangeState<Procedure_Splash>(procedureOwner);
        }

    }
}