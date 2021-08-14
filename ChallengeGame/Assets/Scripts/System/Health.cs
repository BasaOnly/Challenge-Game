using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Collider collider;
    public Transform instantaneousPosMagic;

    [Header("Status")]
    public float life;
    public float maxLife;
    public bool die;

    #region combat
    public virtual void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        if (instantaneousMagic)
            instantaneousMagic.transform.position = instantaneousPosMagic.position;
       
        if (life <= 0)
        {
            die = true;
            collider.enabled = false;
            animator.Play("Death");
        }
    }
    public void GetHit()
    {
        animator.ResetTrigger("getHit");
        animator.SetTrigger("getHit");
    }

    
    #endregion
}
