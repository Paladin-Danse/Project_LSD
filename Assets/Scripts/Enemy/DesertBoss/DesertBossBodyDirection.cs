using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossBodyDirection : MonoBehaviour
{    
    private Transform target;
    public float maxR;
    public float minR;
    public float maxD;
    public float minD;
    float attackRange;
    public LayerMask layerMask;
    DesertBoss desertBoss;

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
            LookTarget();
        }        
    }

    public void LookTarget()
    {        
        float distance = Vector3.Distance(transform.position, target.position);
        float nor = Mathf.InverseLerp(minD, maxD, distance);
        float targetR = Mathf.Lerp(minR, maxR, nor);
        //targetR = Mathf.Clamp(targetR, minD, maxD);
        transform.Rotate(0f, targetR, 0f);              
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
