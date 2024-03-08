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

namespace UnrealFPS.AI
{
    /// <summary>
    /// AI field of view cast info.
    /// </summary>
    public struct ViewCast : IEquatable<ViewCast>
    {
        private bool hit;
        private Vector3 point;
        private float dst;
        private float angle;

        public ViewCast(bool hit, Vector3 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }

        public bool GetHit()
        {
            return hit;
        }

        public void SetHit(bool value)
        {
            hit = value;
        }

        public Vector3 GetPoint()
        {
            return point;
        }

        public void SetPoint(Vector3 value)
        {
            point = value;
        }

        public float GetDst()
        {
            return dst;
        }

        public void SetDst(float value)
        {
            dst = value;
        }

        public float GetAngle()
        {
            return angle;
        }

        public void SetAngle(float value)
        {
            angle = value;
        }

        /// <summary>
        /// Empty field of view cast info.
        /// </summary>
        /// <value></value>
        public readonly static ViewCast Empty = new ViewCast(false, Vector3.zero, 0, 0);

        public static bool operator ==(ViewCast left, ViewCast right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ViewCast left, ViewCast right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is ViewCast metrics) && Equals(metrics);
        }

        public bool Equals(ViewCast other)
        {
            return (hit, point, dst, angle) == (other.hit, other.point, other.dst, other.angle);
        }

        public override int GetHashCode()
        {
            return (hit, point, dst, angle).GetHashCode();
        }
    }
}