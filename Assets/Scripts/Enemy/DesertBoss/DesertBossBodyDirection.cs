using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossBodyDirection : MonoBehaviour
{    
    private Transform target;                         
    float attackRange;
    public LayerMask layerMask;
    DesertBoss desertBoss;
    public float spinSpeed;

    private void Awake()
    {
        desertBoss = GetComponentInParent<DesertBoss>();
        attackRange = desertBoss.RData.AttackRange;
    }

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.25f);
    }
    void LateUpdate()
    {
        if(target != null)
        {
            BodyDir();
        }        
    }       

    void BodyDir()
    {

        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;

        float distance = Vector3.Distance(transform.position, target.position);
        float maxRotation = -170f;
        float mxR = 0f;

        if (distance < 10f)
        {
            mxR = maxRotation;
        }
        else if (distance >= 10f && distance < 15f)
        {            
            mxR = maxRotation - 20f;
        }
        else if (distance >= 15f && distance < 18.7f)
        {            
            mxR = maxRotation - 17f;
        }
        else if (distance >= 18.7f && distance < 23f)
        {
            mxR = maxRotation - 14f;
        }
        else if (distance >= 23f && distance < 26f)
        {
            mxR = maxRotation - 6f;
        }
        else if (distance >= 26f && distance < 28.5f)
        {
            mxR = maxRotation - 2.5f;
        }
        else if (distance >= 28.5f && distance < 31.1f)
        {
            mxR = maxRotation + 2f;
        }
        else if (distance >= 31.1f)
        {
            mxR = maxRotation + 5f;
        }
        
        float yRotation = Mathf.LerpAngle(-115f, mxR, Mathf.Clamp01(distance / desertBoss.RData.AttackRange));

        transform.localRotation = Quaternion.Euler(euler.x, yRotation - 10f, 0);        
    }

    void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, attackRange, layerMask);

        if(cols.Length > 0)
        {
            for(int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            target = null;
        }
    }
}
