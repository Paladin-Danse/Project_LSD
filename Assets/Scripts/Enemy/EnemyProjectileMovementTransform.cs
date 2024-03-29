using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMovementTransform : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        //Debug.Log(transform.position);
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
