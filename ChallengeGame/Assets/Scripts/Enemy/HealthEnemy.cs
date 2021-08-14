using UnityEngine;

public class HealthEnemy : Health
{
    public bool wasAttacked;
    public override void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        wasAttacked = true;
        life -= damage;
        GetHit();
        base.TakeDamage(damage, instantaneousMagic);
    }
}
