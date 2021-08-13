using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantaneousMagic : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject particleFX;

    public void SetData(Health target)
    {
        if (target)
        {
            target.TakeDamage(damage, this.gameObject);
            particleFX.SetActive(true);
        }

        Destroy(this.gameObject, 4);
    }

}
