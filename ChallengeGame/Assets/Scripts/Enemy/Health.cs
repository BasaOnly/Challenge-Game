using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animEnemy;
    [SerializeField] Transform target;
    [SerializeField] Collider colEnemy;
    [SerializeField] Transform instantaneousPosMagic;

    [Header("Status")]
    [SerializeField] float life;
    public bool die;

    #region combat
    public void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        life -= damage;

        if (instantaneousMagic)
            instantaneousMagic.transform.position = instantaneousPosMagic.position;

        if (life <= 0)
        {
            die = true;
            colEnemy.enabled = false;
            animEnemy.Play("die");
        }

    }
    
    #endregion
}
