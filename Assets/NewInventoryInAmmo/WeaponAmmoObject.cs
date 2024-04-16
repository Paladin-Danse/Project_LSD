using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary> ź�� ���� </summary>
public enum AmmoType
{
    None = 0,
    Rifle, // ������
    Pistol, // �ǽ���
}

[System.Serializable]
public class WeaponAmmoObject : MonoBehaviour
{
    public AmmoType ammoType; // ���޵Ǵ� ź�� ����
    public int ammoCount; // ���޵Ǵ� ����
    // �ݶ��̴��� ��ü ���˽�
    // �ٴڿ� �浹�Ҷ��� ����� -> �÷��̾�� �浹�Ҷ��� ����ǵ��� ���� �ʿ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IObjectCrash objCrash))
        {
            objCrash.TakeAmmoItemColliderCrash(ammoType, ammoCount);
            Destroy(gameObject);
        }
    }
}