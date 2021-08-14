using UnityEngine;

public class HealthEnemy : Health
{
    public bool wasAttacked;

    [Header("EnemyBar")]
    [SerializeField] int barIndex;
    [SerializeField] Transform targetBar;
    [SerializeField] Transform myBar;

    private void Start()
    {
        barIndex = UIManager.instance.InstantiateLifeBar();
        myBar = UIManager.instance.GetBar(barIndex).transform.parent;
    }

    private void Update()
    {
        myBar.position = targetBar.position;
        myBar.rotation = targetBar.rotation;
    }

    public override void TakeDamage(float damage, GameObject instantaneousMagic = null)
    {
        wasAttacked = true;
        life -= damage;
        UIManager.instance.SetHPEnemy(life / maxLife, barIndex);
        GetHit();
        base.TakeDamage(damage, instantaneousMagic);
    }
    private void OnDestroy()
    {
        if(myBar != null)
            Destroy(myBar.gameObject);
    }
}
