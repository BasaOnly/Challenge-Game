using UnityEngine;

public class HealthPlayer : Health
{
    public override void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        if (!animator.GetBool("defend"))
        {
            life -= damage;
            GetHit();
        }

        if(life <= 0)
        {
            GameManager.instance.stopActionsPlayer = true;
        }

        base.TakeDamage(damage, instantaneousMagic);
    
    }
}
