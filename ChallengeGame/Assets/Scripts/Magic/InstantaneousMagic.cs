using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantaneousMagic : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject particleFX;

    public void SetData(Transform enemy)
    {
        if (enemy != null)
        {
            enemy.transform.GetComponent<Enemy>().TakeDamage(damage, this.gameObject);
            particleFX.SetActive(true);
        }

        Destroy(this.gameObject, 4);
    }

}
