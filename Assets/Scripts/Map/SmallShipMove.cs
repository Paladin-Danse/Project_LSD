using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShipMove : MonoBehaviour
{
    [SerializeField] private Transform LandingPos;
    [SerializeField] private float Speed;
    
    void Start()
    {
        transform.position = Vector3.Lerp(transform.position, LandingPos.position, Speed);
    }    
}
