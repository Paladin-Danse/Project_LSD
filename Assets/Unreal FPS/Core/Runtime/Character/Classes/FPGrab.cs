/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;

namespace UnrealFPS.Runtime
{
    [System.Serializable]
    public class FPGrab
    {
        [SerializeField] private float grabRange = 3.0f;
        [SerializeField] private float grabDistance = 1.0f;
        [SerializeField] private float heightOffset = 0.5f;
        [SerializeField] private float spring = 50.0f;
        [SerializeField] private float damper = 5.0f;
        [SerializeField] private float drag = 10.0f;
        [SerializeField] private float angularDrag = 5.0f;
        [SerializeField] private float distance = 0.2f;

        private Transform camera;
        private GameObject grabObject;
        private IHealth health;
        private IInventory inventoryCallbacks;
        private MonoBehaviour monoBehaviour;
        private bool isGrabbing;
        private SpringJoint springJoint;

        #region [Functions]
        /// <summary>
        /// Initialize is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        public virtual void Initialize(Transform camera, MonoBehaviour monoBehaviour, IHealth health, IInventory inventoryCallbacks)
        {
            this.camera = camera;
            this.health = health;
            this.inventoryCallbacks = inventoryCallbacks;
            this.monoBehaviour = monoBehaviour;

            //Add and configuring Spring Joint
            if (!springJoint)
            {
                GameObject go = new GameObject("[Rigidbody Dragger]");
                Rigidbody body = go.AddComponent<Rigidbody>();
                springJoint = go.AddComponent<SpringJoint>();
                body.isKinematic = true;
            }

        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        /// <summary>
        public virtual void Update()
        {
            if (UInput.GetButtonDown("Grab") && !isGrabbing)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(camera.position, camera.forward, out raycastHit, grabRange))
                {
                    Debug.Log(raycastHit.transform.name);
                    if (raycastHit.rigidbody != null && !raycastHit.rigidbody.isKinematic && grabObject == null)
                    {
                        Debug.Log("Grab");
                        springJoint.transform.position = raycastHit.point;
                        springJoint.anchor = Vector3.zero;
                        springJoint.spring = spring;
                        springJoint.damper = damper;
                        springJoint.maxDistance = distance;
                        springJoint.connectedBody = raycastHit.rigidbody;
                        isGrabbing = true;
                        inventoryCallbacks.HideWeapon(true);
                        monoBehaviour.StartCoroutine(DragObject(grabDistance));
                    }
                }
            }
            else if ((UInput.GetButtonDown("Grab") && isGrabbing) || !health.IsAlive())
            {
                grabObject = null;
                isGrabbing = false;
            }
        }

        protected virtual IEnumerator DragObject(float distance)
        {
            var oldDrag = springJoint.connectedBody.drag;
            var oldAngularDrag = springJoint.connectedBody.angularDrag;
            springJoint.connectedBody.drag = drag;
            springJoint.connectedBody.angularDrag = angularDrag;
            while (isGrabbing)
            {
                springJoint.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
                springJoint.transform.position = camera.transform.position + (camera.transform.forward * distance) + (Vector3.up * heightOffset);
                yield return null;
            }
            if (springJoint.connectedBody)
            {
                springJoint.connectedBody.drag = oldDrag;
                springJoint.connectedBody.angularDrag = oldAngularDrag;
                springJoint.connectedBody = null;
            }
        }

        #endregion
        #region [Getter / Setter]

        public Transform GetCamera()
        {
            return camera;
        }

        public void SetCamera(Transform value)
        {
            camera = value;
        }

        public float GetGrabRange()
        {
            return grabRange;
        }

        public void SetGrabRange(float value)
        {
            grabRange = value;
        }

        public float GetGrabDistance()
        {
            return grabDistance;
        }

        public void SetGrabDistance(float value)
        {
            grabDistance = value;
        }

        public float GetHeightOffset()
        {
            return heightOffset;
        }

        public void SetHeightOffset(float value)
        {
            heightOffset = value;
        }

        public float GetSpring()
        {
            return spring;
        }

        public void SetSpring(float value)
        {
            spring = value;
        }

        public float GetDamper()
        {
            return damper;
        }

        public void SetDamper(float value)
        {
            damper = value;
        }

        public float GetDrag()
        {
            return drag;
        }

        public void SetDrag(float value)
        {
            drag = value;
        }

        public float GetAngularDrag()
        {
            return angularDrag;
        }

        public void SetAngularDrag(float value)
        {
            angularDrag = value;
        }

        public float GetDistance()
        {
            return distance;
        }

        public void SetDistance(float value)
        {
            distance = value;
        }
        #endregion
    }
}