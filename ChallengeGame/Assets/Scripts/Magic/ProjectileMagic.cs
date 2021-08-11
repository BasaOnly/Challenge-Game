using System.Collections;
using UnityEngine;

public class ProjectileMagic : MonoBehaviour
{
    [SerializeField] float forceVelocity;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] Rigidbody rig;
    private void Start()
    {
        StartCoroutine(ExplodeAfterTime());
        rig.AddForce(transform.forward * forceVelocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(5);
        Explode();
    }

    void Explode()
    {
        Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
