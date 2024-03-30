using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ź�� ���� </summary>
public enum AmmoType
{
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
            objCrash.TakePhysicalCrashAmmo(ammoType, ammoCount);
            Destroy(gameObject);
        }
    }
}