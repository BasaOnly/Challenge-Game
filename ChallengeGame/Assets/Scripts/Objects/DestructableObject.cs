using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] GameObject FX_Crash;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Magic"))
        {
            FX_Crash.SetActive(true);
            FX_Crash.transform.parent = null;
            Destroy(FX_Crash, 7);
            Destroy(this.gameObject);
        }
    }
}
