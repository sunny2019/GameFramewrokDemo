using System;
using System.Text;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UIForm_ShowProgress : UGuiForm
    {
        public Image image_Mask;
        public Text txt_Progress;
        public Sprite[] sprites_Backgrounds;


        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (sprites_Backgrounds.Length > 0)
            {
                image_Mask.sprite = sprites_Backgrounds[Random.Range(0, sprites_Backgrounds.Length)];
            }

        }
        

        public void SetProgressConttent(string content)
        {
            txt_Progress.text = content;
        }

        
    }
}