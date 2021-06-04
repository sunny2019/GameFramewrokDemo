using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Entity_Eugen : EntityLogic
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            camer = GetComponentInChildren<ICinemachineCamera>();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void InternalSetVisible(bool visible)
        {
            base.InternalSetVisible(visible);
        }

        private ICinemachineCamera camer;
        public void SetCamera()
        {
            CinemachineBrain.SoloCamera = camer;
        }
    }
}