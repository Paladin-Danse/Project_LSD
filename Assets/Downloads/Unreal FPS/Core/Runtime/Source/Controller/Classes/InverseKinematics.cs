/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Runtime
{
    [RequireComponent(typeof(Animator))]
    public class InverseKinematics : MonoBehaviour
    {
        [SerializeField] private Transform leftFootPivot;
        [SerializeField] private Transform rightFootPivot;
        [SerializeField] private float footOffsetY;
        [SerializeField] private float upperBodyIKWeight;
        [SerializeField] private float bodyIKWeight;
        [SerializeField] private float headIKWeight;
        [SerializeField] private float eyesIKWeight;
        [SerializeField] private float clampWeight;
        [SerializeField] private bool active;
        [SerializeField] private bool footIKActive;
        [SerializeField] private bool headIKActive;

        private Vector3 lookAtDirection;
        private float leftFootWeight;
        private float rightFootWeight;
        private Transform leftFoot;
        private Transform rightFoot;
        private LayerMask ignoreLayer;

        private Animator animator;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Called only once during the lifetime of the script instance and after all objects are initialized
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            ignoreLayer = ~1 << LayerMask.NameToLayer(LNC.PLAYER);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void OnAnimatorIK(int layerIndex)
        {
            FootIK();
            UpperBodyIK();
        }

        /// <summary>
        /// Foot IK system
        /// </summary>
        public virtual void FootIK()
        {
            //If the IK is not active, set the position and rotation of the hand and head back to the original position.
            if (!active || !footIKActive)
                return;

            leftFootWeight = animator.GetFloat("IK Left Foot");
            rightFootWeight = animator.GetFloat("IK Right Foot");

            //Activate IK, set the rotation directly to the goal.
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

            RaycastHit leftHit;
            if (Physics.Raycast(leftFootPivot.position, -Vector3.up, out leftHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(leftFoot.up, leftHit.normal) * leftFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, leftFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(leftFoot.position.x, leftHit.point.y, leftFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikPosition + (Vector3.up * footOffsetY));
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, ikRotation);
            }

            RaycastHit rightHit;
            if (Physics.Raycast(rightFootPivot.position, -Vector3.up, out rightHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(rightFoot.up, rightHit.normal) * rightFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, rightFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(rightFoot.position.x, rightHit.point.y, rightFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, ikPosition + (Vector3.up * footOffsetY));
                animator.SetIKRotation(AvatarIKGoal.RightFoot, ikRotation);
            }
        }

        /// <summary>
        /// Upper body IK system
        /// </summary>
        public virtual void UpperBodyIK()
        {
            if (!active || !headIKActive)
                return;

            animator.SetLookAtWeight(upperBodyIKWeight, bodyIKWeight, headIKWeight, eyesIKWeight, clampWeight);
            animator.SetLookAtPosition(lookAtDirection);
        }

        public Transform GetLeftFootPivot()
        {
            return leftFootPivot;
        }

        public void SetLeftFootPivot(Transform value)
        {
            leftFootPivot = value;
        }

        public Transform GetRightFootPivot()
        {
            return rightFootPivot;
        }

        public void SetRightFootPivot(Transform value)
        {
            rightFootPivot = value;
        }

        public float GetFootOffsetY()
        {
            return footOffsetY;
        }

        public void SetFootOffsetY(float value)
        {
            footOffsetY = value;
        }

        public float GetUpperBodyIKWeight()
        {
            return upperBodyIKWeight;
        }

        public void SetUpperBodyIKWeight(float value)
        {
            upperBodyIKWeight = value;
        }

        public float GetBodyIKWeight()
        {
            return bodyIKWeight;
        }

        public void SetBodyIKWeight(float value)
        {
            bodyIKWeight = value;
        }

        public float GetHeadIKWeight()
        {
            return headIKWeight;
        }

        public void SetHeadIKWeight(float value)
        {
            headIKWeight = value;
        }

        public float GetEyesIKWeight()
        {
            return eyesIKWeight;
        }

        public void SetEyesIKWeight(float value)
        {
            eyesIKWeight = value;
        }

        public float GetClampWeight()
        {
            return clampWeight;
        }

        public void SetClampWeight(float value)
        {
            clampWeight = value;
        }

        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool value)
        {
            active = value;
        }

        public bool GetFootIKActive()
        {
            return footIKActive;
        }

        public void SetFootIKActive(bool value)
        {
            footIKActive = value;
        }

        public bool GetHeadIKActive()
        {
            return headIKActive;
        }

        public void SetHeadIKActive(bool value)
        {
            headIKActive = value;
        }

        public Vector3 GetLookAtDirection()
        {
            return lookAtDirection;
        }

        public void SetLookAtDirection(Vector3 value)
        {
            lookAtDirection = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public void SetAnimator(Animator value)
        {
            animator = value;
        }

        public float GetLeftFootWeight()
        {
            return leftFootWeight;
        }

        public void SetLeftFootWeight(float value)
        {
            leftFootWeight = value;
        }

        public float GetRightFootWeight()
        {
            return rightFootWeight;
        }

        public void SetRightFootWeight(float value)
        {
            rightFootWeight = value;
        }

        public Transform GetLeftFoot()
        {
            return leftFoot;
        }

        public void SetLeftFoot(Transform value)
        {
            leftFoot = value;
        }

        public Transform GetRightFoot()
        {
            return rightFoot;
        }

        public void SetRightFoot(Transform value)
        {
            rightFoot = value;
        }
    }
}