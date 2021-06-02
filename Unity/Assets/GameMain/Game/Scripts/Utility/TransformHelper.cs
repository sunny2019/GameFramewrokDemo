using System.Reflection;
using UnityEngine;


namespace GameMain
{
    public static class TransformHelper
    {
        public static Vector3 GetInspectorRotationValueMethod(this Transform transform)
        {
            // 获取原生值
            System.Type transformType = transform.GetType();
            PropertyInfo m_propertyInfo_rotationOrder =
                transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
            object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
            MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles",
                BindingFlags.Instance | BindingFlags.NonPublic);
            object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] {m_OldRotationOrder});
            return (Vector3) value;
            // string temp = value.ToString();
            // //将字符串第一个和最后一个去掉
            // temp = temp.Remove(0, 1);
            // temp = temp.Remove(temp.Length - 1, 1);
            // //用‘，’号分割
            // string[] tempVector3;
            // tempVector3 = temp.Split(',');
            // //将分割好的数据传给Vector3
            // Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]),
            //     float.Parse(tempVector3[2]));
            // return vector3;
        }

        // public static Vector3 GetInspectorRotation(this Transform trans)
        // {
        //     var localEulerAngles = trans.localEulerAngles;
        //     var x = FitAngleForInspector(localEulerAngles.x);
        //     var y = FitAngleForInspector(localEulerAngles.y);
        //     var z = FitAngleForInspector(localEulerAngles.z);
        //     return new Vector3(x,y,z);
        // }
        //
        // private static float FitAngleForInspector(float temp)
        // {
        //     temp = temp < 180 ? temp : temp - 360;
        //     return temp;
        // }


        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <returns></returns>
        public static float Angle2Radian(float value)
        {
            return value * Mathf.PI / 180.0f;
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <returns></returns>
        public static double Angle2Radian(double value)
        {
            return value * Mathf.PI / 180.0f;
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <returns></returns>
        public static Vector2 Angle2Radian(Vector2 value)
        {
            value.x = Angle2Radian(value.x);
            value.y = Angle2Radian(value.y);
            return value;
        }
        
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <returns></returns>
        public static Vector2 Angle2Radian(Vector3 value)
        {
            value.x = Angle2Radian(value.x);
            value.y = Angle2Radian(value.y);
            value.z = Angle2Radian(value.z);
            return value;
        }


        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <returns></returns>
        public static float Radian2Angle(float value)
        {
            return value * 180.0f / Mathf.PI;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <returns></returns>
        public static double Radian2Angle(double value)
        {
            return value * 180.0f / Mathf.PI;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <returns></returns>
        public static Vector2 Radian2Angle(Vector2 value)
        {
            value.x = Radian2Angle(value.x);
            value.y = Radian2Angle(value.y);
            return value;
        }
        
        public static Vector2 Radian2Angle(Vector3 value)
        {
            value.x = Radian2Angle(value.x);
            value.y = Radian2Angle(value.y);
            value.z = Radian2Angle(value.z);
            return value;
        }
    }
}