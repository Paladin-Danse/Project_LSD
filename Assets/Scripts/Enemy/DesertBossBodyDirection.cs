using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossBodyDirection : MonoBehaviour
{
    //Animator animator;
    //public bool ikActive = false;
    public Transform target;

    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    void Update()
    {
        LookTarget();
    }

    public void LookTarget()
    {

        //Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        //Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        //transform.rotation = Quaternion.LookRotation(to - from);
        //
        Vector3 vec = target.position - transform.position;
        vec.Normalize();
        Quaternion q = Quaternion.LookRotation(vec);
        transform.localEulerAngles = q.eulerAngles;

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
