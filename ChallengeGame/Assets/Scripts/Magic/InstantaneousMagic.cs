using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantaneousMagic : MonoBehaviour
{
    [SerializeField] GameObject particleFX;
    [SerializeField] float groundDistance;
    [SerializeField] float distanceMagic;
    [SerializeField] LayerMask enemyLayer;
    void Start()
    {
        if(Physics.SphereCast(this.transform.position, 1f, this.transform.forward, out RaycastHit hit, distanceMagic, enemyLayer))
        {
            float yHalfExtents = hit.collider.bounds.extents.y;
            float yLower = (hit.transform.position.y + groundDistance) - yHalfExtents;
            this.transform.position = new Vector3(hit.transform.position.x, yLower, hit.transform.transform.position.z);
            particleFX.SetActive(true);
            Destroy(this.gameObject, 4);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
