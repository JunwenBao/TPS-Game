using UnityEngine;

public class IdleState_Range : EnemyState
{
    private Enemy_Range enemy;

    public IdleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        /*
        enemy.anim.SetFloat("IdleAnimIndex", Random.Range(0, 2));

        enemy.visuals.EnableIK(true, false);

        if (enemy.weaponType == Enemy_RangeWeaponType.Pistol)
            enemy.visuals.EnableIK(false, false);
        */
        stateTimer = enemy.idleTime; //��ʱ���������л���Move State
    }

    public override void Update()
    {
        base.Update();

        /* ���ݼ�ʱ�����ж��Ƿ�Ҫ����Move State */
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}