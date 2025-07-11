using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;
    private int bulletShot = 0;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.position);

        if (WeaponOutOfBullets())
        {
            if (WeaponOnCoolDown()) AttempToResetWeapon();
            return;
        }

        if (CanShoot()) Shoot();
    }
    
    public override void Exit()
    {
        base.Exit();
    }

    private void AttempToResetWeapon() => bulletShot = 0;
    private bool WeaponOnCoolDown() => Time.time > lastTimeShot + enemy.weaponCooldown;
    private bool WeaponOutOfBullets() => bulletShot >= enemy.bulletsToShoot;
    private bool CanShoot() => Time.time > lastTimeShot + 1 / enemy.fireRate;

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletShot++;
    }
}