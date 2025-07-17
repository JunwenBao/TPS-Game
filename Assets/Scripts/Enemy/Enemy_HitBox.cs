using UnityEngine;

public class Enemy_HitBox : HitBox
{
    [SerializeField] private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        enemy.GetHit();
    }
}