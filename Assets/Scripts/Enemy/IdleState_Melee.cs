using UnityEngine;

public class IdleState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public IdleState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // 如果检测到玩家进入攻击范围，则切换到Recovery State
        if (enemy.PlayerInAggresionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
