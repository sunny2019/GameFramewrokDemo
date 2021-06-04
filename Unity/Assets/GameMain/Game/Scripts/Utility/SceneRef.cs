using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMain
{
    public class SceneRef : SerializedMonoBehaviour
    {
        public static SceneRef Ins;

        // Start is called before the first frame update
        void Awake()
        {
            Ins = this;
        }

        [ShowInInspector] 
        [SerializeField]private Dictionary<string, Transform> dic_Ref = new Dictionary<string, Transform>();

        public Transform Get(string keyword)
        {
            if (dic_Ref.ContainsKey(keyword))
                return dic_Ref[keyword];
            return null;
        }
    }
}