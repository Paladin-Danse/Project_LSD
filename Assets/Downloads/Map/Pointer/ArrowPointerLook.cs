using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointerLook : MonoBehaviour
{
    Transform target;
    void Start()
    {
        Invoke("FindChar", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, 100f).eulerAngles;
        transform.rotation = Quaternion.Euler(0, euler.y, 0);
    }

    void FindChar()
    {
        target = FindObjectOfType<PlayerCharacter>().transform;
    }
}
