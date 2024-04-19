using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary> 탄약 종류 </summary>
public enum AmmoType
{
    None = 0,
    Rifle, // 라이플
    Pistol, // 피스톨
}

[System.Serializable]
public class WeaponAmmoObject : MonoBehaviour
{
    public AmmoType ammoType; // 지급되는 탄약 종류
    public int ammoCount; // 지급되는 개수
    // 콜라이더에 객체 접촉시
    // 바닥에 충돌할때도 실행됨 -> 플레이어와 충돌할때만 실행되도록 개선 필요
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IObjectCrash objCrash))
        {
            objCrash.TakeAmmoItemColliderCrash(ammoType, ammoCount);
            Destroy(gameObject);
        }
    }
}