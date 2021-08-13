using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] Collider collider;
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
            collider.enabled = false;
            animator.Play("Death");
        }

    }
    
    #endregion
}
