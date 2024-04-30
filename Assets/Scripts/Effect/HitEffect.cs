using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private void OnDisable()
    {
        ObjectPoolManager.Instance.TryPush(gameObject);
    }
}
