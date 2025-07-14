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
    private bool firstTimeAttack = true;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        SetupValuesForFirstAttack();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        enemy.visuals.EnableIK(true, true);

        stateTimer = enemy.attackDelay;
    }

    public override void Update()
    {
        base.Update();

        /* 看到玩家时：转向玩家 */
        if (enemy.IsSeeingPlayer())
        {
            enemy.FaceTarget(enemy.aim.position);
        }

        /* 判断：是否要投掷手雷 */
        if (enemy.CanThrowGrenade())
        {
            stateMachine.ChangeState(enemy.throwGrenadeState);
        }

        /* 判断：是否要追击玩家*/
        if (MustAdvancePlayer())
        {
            stateMachine.ChangeState(enemy.advancePlayerState);
        }

        /* 判断：是否要切换Cover */
        ChangeCoverIfShould();

        /* 控制延迟射击：防止敌人见面就开火 */
        if (stateTimer > 0) return;

        if (WeaponOutOfBullets())
        {
            if(enemy.IsUnstoppable() && UnstoppableWalkReady())
            {
                enemy.advanceDuration = weaponCooldown;
                stateMachine.ChangeState(enemy.advancePlayerState);
            }

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

    private bool UnstoppableWalkReady()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        bool outOfStoppingDistance = distanceToPlayer > enemy.advanceStoppingDistance;
        bool unstoppableWalkOnCooldown = Time.time < enemy.weaponData.minWeaponCooldown + enemy.advancePlayerState.lastTimeAdvanced;

        return outOfStoppingDistance && unstoppableWalkOnCooldown == false;
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

    private void SetupValuesForFirstAttack()
    {
        if (firstTimeAttack)
        {
            firstTimeAttack = false;
            bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
            weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        }
    }

    #endregion

    #region Cover System

    // 离开Cover
    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.minCoverTime + enemy.runToCoverState.lastTimeTookCover;
    }

    // 判断玩家是否距离敌人过近
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance;
    }

    // 根据玩家是否出现在视野范围，判断是否要切换Cover
    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = enemy.player.transform.position - enemy.transform.position;

        if(Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>();
        }

        return false;
    }

    // 判断敌人是否要切换Cover
    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = 0.5f;

            if (ReadyToChangeCover())
            {
                if (enemy.CanGetCover()) stateMachine.ChangeState(enemy.runToCoverState);
            }
        }
    }

    // 根据Aggrresion Range，判断是否应该追击玩家
    private bool MustAdvancePlayer()
    {
        if (enemy.IsUnstoppable()) return false;

        return enemy.IsPlayerInAgrresionRange() == false && ReadyToLeaveCover();
    }

    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        bool advanceTimeIsOver = Time.time > enemy.advancePlayerState.lastTimeAdvanced + enemy.advanceDuration;

        return inDanger && advanceTimeIsOver;
    }

    #endregion
}