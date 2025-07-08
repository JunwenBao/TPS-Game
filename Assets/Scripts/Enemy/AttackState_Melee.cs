using System.Collections.Generic;
using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private Vector3 attackDirection;
    private float attackMoveSpeed;

    private const float MAX_ATTACK_DISTANCE = 50f;

    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.PullWeapon();

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.animator.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.animator.SetFloat("AttackIndex", enemy.attackData.attackIndex);
        //enemy.anim.SetFloat("SlashAttackIndex", Random.Range(0, 5));

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();

        SetupNextAttack();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation =  enemy.FaceTarget(enemy.player.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        }

        if (triggerCalled)
        {
            if (enemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.recoveryState);
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }

    // 设置下一次攻击
    private void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;

        enemy.animator.SetFloat("RecoveryIndex", recoveryIndex);
        if(enemy.PlayerInAttackRange()) enemy.animator.SetFloat("RecoveryIndex", 0);
        enemy.attackData = UpdatedAttackData();
    }

    // 更新攻击数据：从敌人的AttackData List中随机选择
    private AttackData UpdatedAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(enemy.attackList);

        if (PlayerClose())
        {
            validAttacks.RemoveAll(parameter => parameter.attackType == AttackType_Melee.Charge);
        }

        int random = Random.Range(0, validAttacks.Count);

        return validAttacks[random];
    }

    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.position) <= 1;
}