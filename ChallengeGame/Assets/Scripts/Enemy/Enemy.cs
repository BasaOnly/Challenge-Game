using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animEnemy;
    [SerializeField] Collider colEnemy;
    [SerializeField] Transform target;
    [SerializeField] Transform instantaneousPosMagic;

    [Header("Status")]
    [SerializeField] float life;

    [Header("Variables")]
    bool die;
    

    void Update()
    {
        if(!die)
            transform.LookAt(target);
    }

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
}
