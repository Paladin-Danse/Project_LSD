using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossAnimControll : MonoBehaviour
{
    public Animator anim;
    void Update()
    {
        BossAnim();
    }

    void BossAnim()
    {
        if(gameObject.GetComponent<Health>().curHealth > gameObject.GetComponent<Health>().maxHealth/2f)
        {
            anim.SetLayerWeight(1, 0);
        }
        else
        {
            anim.SetLayerWeight(1, 1);
        }
    }
}
