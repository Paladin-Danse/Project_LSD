using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffTest : MonoBehaviour
{
    [SerializeField]
    CharacterBuffSO buffSO;

    [SerializeField]
    CharacterBuff buff;

    private void Awake()
    {
        buff = (Instantiate(buffSO) as CharacterBuffSO).buff;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<CharacterBuffHandler>(out CharacterBuffHandler handler)) 
        {
            handler.AddBuff(buff);
        }
    }
}
