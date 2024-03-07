/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace UnrealFPS.Runtime
{
    [Serializable]
    public struct DropProperties : IEquatable<DropProperties>
    {
        [SerializeField] private GameObject dropObject;
        [SerializeField] private float force;
        [SerializeField] private AudioClip soundEffect;
        [SerializeField] private float distance;
        [SerializeField] private Vector3 rotation;

        /// <summary>
        /// Constructor
        /// </summary>
        public DropProperties(GameObject dropObject, float force = 0.5f)
        {
            this.dropObject = dropObject;
            this.force = force;
            this.soundEffect = null;
            this.distance = 0;
            this.rotation = Vector3.zero;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DropProperties(GameObject dropObject, float force, AudioClip soundEffect, float distance, Vector3 rotation)
        {
            this.dropObject = dropObject;
            this.force = force;
            this.soundEffect = soundEffect;
            this.distance = distance;
            this.rotation = rotation;
        }

        public GameObject GetDropObject()
        {
            return dropObject;
        }

        public void SetDropObject(GameObject value)
        {
            dropObject = value;
        }

        public float GetForce()
        {
            return force;
        }

        public void SetForce(float value)
        {
            force = value;
        }

        public AudioClip GetSoundEffect()
        {
            return soundEffect;
        }

        public void SetSoundEffect(AudioClip value)
        {
            soundEffect = value;
        }

        public float GetDistance()
        {
            return distance;
        }

        public void SetDistance(float value)
        {
            distance = value;
        }

        public Vector3 GetRotation()
        {
            return rotation;
        }

        public void SetRotation(Vector3 value)
        {
            rotation = value;
        }

        /// <summary>
        /// Empty DropProperties instance.
        /// </summary>
        public readonly static DropProperties Empty = new DropProperties(null, 0, null, 0, Vector3.zero);


        public static bool operator ==(DropProperties left, DropProperties right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DropProperties left, DropProperties right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is DropProperties metrics) && Equals(metrics);
        }

        public bool Equals(DropProperties other)
        {
            return (dropObject, force, soundEffect, distance, rotation) == (other.dropObject, other.force, other.soundEffect, other.distance, other.rotation);
        }

        public override int GetHashCode()
        {
            return (dropObject, force, soundEffect, distance, rotation).GetHashCode();
        }
    }
}