using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnergyShield : MonoBehaviour
{
    public float forceAmount;
    public float damage;
    private ParticleSystem spark;
    public GameObject sparkParticle;
    public AudioSource audioSource;
    public AudioClip electricity;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * forceAmount, ForceMode.Impulse);
            other.gameObject.GetComponent<Health>().TakeDamageWithoutDefense(damage);
            GameObject Electric = Instantiate(sparkParticle, other.gameObject.transform.position, Quaternion.identity);
            ParticleSystem electricSpark = Electric.GetComponent<ParticleSystem>();
            this.spark = electricSpark;
            spark.Play();
            audioSource.PlayOneShot(electricity);
            Debug.Log("shield" + damage);
        }
    }
}
