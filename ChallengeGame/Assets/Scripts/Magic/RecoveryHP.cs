using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHP : MonoBehaviour
{
    [SerializeField] ParticleSystem FX_Heal;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FX_Heal.Play();
            other.GetComponent<HealthPlayer>().RecoveryHP();
        }
    }
}
