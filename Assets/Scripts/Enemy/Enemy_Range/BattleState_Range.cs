using System.Threading;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;
    private int bulletShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;
    private float coverCheckTimer;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        enemy.visuals.EnableIK(true, true);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsSeeingPlayer()) enemy.FaceTarget(enemy.aim.position);

        if (MustAdvancePlayer())
        {
            stateMachine.ChangeState(enemy.advancePlayerState);
        }

        /* �жϣ��Ƿ�Ҫ�л�Cover */
        ChangeCoverIfShould();

        enemy.FaceTarget(enemy.player.position);

        if (WeaponOutOfBullets())
        {
            if (WeaponOnCoolDown()) AttempToResetWeapon();
            return;
        }

        if (CanShoot() && enemy.IsAimOnPlayer()) Shoot();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.visuals.EnableIK(false, false);
    }

    #region Weapon

    private void AttempToResetWeapon()
    {
        bulletShot = 0;
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
    }

    private bool WeaponOnCoolDown() => Time.time > lastTimeShot + weaponCooldown;
    private bool WeaponOutOfBullets() => bulletShot >= bulletsPerAttack;
    private bool CanShoot() => Time.time > lastTimeShot + 1 / enemy.weaponData.fireRate;

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletShot++;
    }

    #endregion

    #region Cover System

    // �ж�����Ƿ������˹���
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance;
    }

    // ��������Ƿ��������Ұ��Χ���ж��Ƿ�Ҫ�л�Cover
    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = enemy.player.transform.position - enemy.transform.position;

        if(Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>();
        }

        return false;
    }

    // �жϵ����Ƿ�Ҫ�л�Cover
    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = 0.5f;

            if (IsPlayerInClearSight() || IsPlayerClose())
            {
                if (enemy.CanGetCover()) stateMachine.ChangeState(enemy.runToCoverState);
            }
        }
    }

    // ����Aggrresion Range���ж��Ƿ�Ӧ��׷�����
    private bool MustAdvancePlayer()
    {
        //if (enemy.IsUnstopppable()) return false;

        return enemy.IsPlayerInAgrresionRange() == false;
    }

    #endregion
}