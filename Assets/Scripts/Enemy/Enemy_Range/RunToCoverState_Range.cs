using UnityEngine;

public class RunToCoverState_Range : EnemyState
{
    private Enemy_Range enemy;

    private Vector3 destination;

    public float lastTimeTookCover { get; private set; }

    public RunToCoverState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        Debug.Log("进入Cover State");
        base.Enter();
        destination = enemy.currentCover.transform.position;

        enemy.visuals.EnableIK(true, false);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.runSpeed;
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeTookCover = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPathPoint());

        //TODO:这个距离控制不合理，可能会有潜在的问题
        if (Vector3.Distance(enemy.transform.position, destination) < 3f)
        {
            Debug.Log("切换状态：Cover -> Battle");
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}