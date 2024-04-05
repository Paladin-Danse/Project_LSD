using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePrefab : MonoBehaviour
{    
    WaitForSeconds WFS1;
    WaitForSeconds WFS2;
    WaitForSeconds WFS3;
    private ParticleSystem Fire;
    private GameObject FireInstance;

    private void Awake()
    {
        WFS1 = new WaitForSeconds(5f);
        WFS2 = new WaitForSeconds(3f);
        WFS3 = new WaitForSeconds(2f);
        FireInstance = this.gameObject;
    }
    
    void Start()
    {
        CFire();
    }
    
    void CFire()
    {
        StartCoroutine("CreateFire");
    }
    IEnumerator CreateFire()
    {
        FireInstance.transform.GetChild(0).gameObject.SetActive(true);
        FireInstance.transform.GetChild(0).gameObject.transform.position += new Vector3(0, 0.3f, 0);
        ParticleSystem Effect = FireInstance.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        this.Fire = Effect;
        Fire.Play();
        yield return WFS1;
        FireInstance.transform.GetChild(0).gameObject.SetActive(false);        
        FireInstance.transform.GetChild(1).gameObject.SetActive(true);
        FireInstance.transform.GetChild(1).gameObject.transform.position += new Vector3(0, 0.3f, 0);
        Effect = FireInstance.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        this.Fire = Effect;
        Fire.Play();
        yield return WFS2;
        FireInstance.transform.GetChild(1).gameObject.SetActive(false);        
        FireInstance.transform.GetChild(2).gameObject.SetActive(true);
        FireInstance.transform.GetChild(2).gameObject.transform.position += new Vector3(0, 0.2f, 0);
        Effect = FireInstance.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        this.Fire = Effect;
        Fire.Play();
        yield return WFS3;
        Destroy(FireInstance);
    }
}
