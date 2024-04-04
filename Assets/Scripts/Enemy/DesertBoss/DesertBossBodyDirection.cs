using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossBodyDirection : MonoBehaviour
{
    //Animator animator;
    //public bool ikActive = false;
    public Transform target;
    public float maxR;
    public float minR;
    public float maxD;
    public float minD;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    void LateUpdate()
    {
        LookTarget();
    }

    public void LookTarget()
    {
        //transform.LookAt(target.position);

        //Vector3 vec = target.position - transform.position;
        //transform. = Vector3.Lerp(transform.right, vec, 0.01f);

        float distance = Vector3.Distance(transform.position, target.position);
        float nor = Mathf.InverseLerp(minD, maxD, distance);
        float targetR = Mathf.Lerp(minR, maxR, nor);
        //targetR = Mathf.Clamp(targetR, minD, maxD);
        transform.Rotate(0f, targetR, 0f);

        //Vector3 vec = (target.position - transform.position).normalized;
        //Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        //Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        //transform.rotation = Quaternion.LookRotation(to - from);
        //
        //Vector3 vec = target.position - transform.position;
        //vec.Normalize();
        //Quaternion q = Quaternion.LookRotation(vec);
        //transform.eulerAngles = q.eulerAngles;

        //Vector3 vector = target.transform.position - transform.position;
        //gameObject.transform.rotation = Quaternion.LookRotation(vector).normalized;        
    }

    //private void OnAnimatorIK()
    //{
    //    if (animator)
    //    {
    //        if(!ikActive)
    //        {
    //            if(target != null)
    //            {
    //                animator.SetLookAtWeight(1);
    //                animator.SetLookAtPosition(target.position);
    //            }
    //            else
    //            {
    //                animator.SetLookAtWeight(0);
    //            }
    //        }
    //    }
    //}
}
