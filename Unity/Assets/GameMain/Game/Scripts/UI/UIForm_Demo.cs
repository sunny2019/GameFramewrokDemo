using System;
using System.Text;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UIForm_Demo : UGuiForm
    {
        public Button btn_BOOM;
        public Toggle tg_BOOM;
        public Button btn_Player;
        public Button btn_PlayerLeftRotate;
        public Button btn_PlayerRightRotate;
        public Button btn_Quit;

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            tg_BOOM.onValueChanged.AddListener((v) =>
            {
                ((Entity_Boom)(GameEntry.Entity.GetEntity(2).Logic)).Boom();

            });
            btn_Player.onClick.AddListener(() =>
            {
                ((Entity_Eugen)(GameEntry.Entity.GetEntity(1).Logic)).SetCamera();
            });
            btn_BOOM.onClick.AddListener(() =>
            {
                ((Entity_Boom)(GameEntry.Entity.GetEntity(2).Logic)).SetCamera();
            });
            
            btn_PlayerLeftRotate.onClick.AddListener(() =>
            {
                GameEntry.Entity.GetEntity(1).Logic.transform
                    .DOLocalRotate(new Vector3(0, -10f, 0), 0.5f, RotateMode.LocalAxisAdd);
            });
            
            
            btn_PlayerRightRotate.onClick.AddListener(() =>
            {
                GameEntry.Entity.GetEntity(1).Logic.transform
                    .DOLocalRotate(new Vector3(0, 10f, 0), 0.5f, RotateMode.LocalAxisAdd);
            });
            btn_Quit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}