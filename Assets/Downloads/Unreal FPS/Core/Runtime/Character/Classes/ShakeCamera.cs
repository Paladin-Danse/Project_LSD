/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    public partial class ShakeCamera : Singleton<ShakeCamera>
    {
        protected List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
        private Vector3 originalPosition;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            originalPosition = transform.localPosition;
        }

        /// <summary>
        /// LateUpdate is called every frame after Update function, if the Behaviour is enabled.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (shakeEvents.Count == 0)
                return;

            Vector3 positionOffset = originalPosition;
            Vector3 rotationOffset = transform.localRotation.eulerAngles;

            for (int i = shakeEvents.Count - 1; i != -1; i--)
            {
                ShakeEvent se = shakeEvents[i];
                se.Update();

                if (se.GetTarget() == ShakeEvent.Target.Position)
                    positionOffset += se.GetNoise();
                else
                    rotationOffset += se.GetNoise();

                if (!se.IsAlive())
                    shakeEvents.RemoveAt(i);
                transform.localPosition = positionOffset;
                transform.localRotation = Quaternion.Euler(rotationOffset);
            }
        }

        public void AddShakeEvent(ShakeProperties properties)
        {
            shakeEvents.Add(new ShakeEvent(properties));
        }

        public void AddShakeEvent(ShakeEvent shakeEvent)
        {
            shakeEvents.Add(shakeEvent);
        }

        public static ShakeProperties HitShakeProperties()
        {
            return new ShakeProperties(0.1f, 5, 0.5f, new AnimationCurve(
                new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
                new Keyframe(0.2f, 1.0f),
                new Keyframe(1.0f, 0.0f)), ShakeEvent.Target.Position);

        }

        public static ShakeProperties ExplosionShakeProperties()
        {
            return new ShakeProperties(0.5f, 5, 0.5f, new AnimationCurve(
                new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
                new Keyframe(0.2f, 1.0f),
                new Keyframe(1.0f, 0.0f)), ShakeEvent.Target.Position);
        }

        public static ShakeProperties ExplosionShakeProperties(float radius, float distance, float minAmplitude = 0, float maxAmplitude = 0.55f)
        {
            float amplitude = Mathf.InverseLerp(0, radius, radius - distance);
            amplitude = Mathf.Clamp(amplitude, minAmplitude, maxAmplitude);
            return new ShakeProperties(amplitude, 5, 0.5f, new AnimationCurve(
                new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
                new Keyframe(0.2f, 1.0f),
                new Keyframe(1.0f, 0.0f)), ShakeEvent.Target.Position);
        }
    }
}