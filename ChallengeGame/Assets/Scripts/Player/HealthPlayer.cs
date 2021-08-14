using UnityEngine;

public class HealthPlayer : Health
{
    public override void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        if (!animator.GetBool("defend"))
        {
            life -= damage;
            UIManager.instance.SetValueHP(life / maxLife);
            GetHit();
        }

        if(life <= 0)
        {
            GameManager.instance.stopActionsPlayer = true;
        }

        base.TakeDamage(damage, instantaneousMagic);
    }

    public void RecoveryHP()
    {
        if (life >= 100) return;

        life = maxLife;
        UIManager.instance.SetValueHP(life / maxLife);
    }
}
