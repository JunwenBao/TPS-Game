using UnityEngine;

public class AdvancePlayerState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 playerPos;

    public float lastTimeAdvanced { get; private set; }

    public AdvancePlayerState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        Debug.Log("½øÈë×´Ì¬£ºAdvance");
        base.Enter();

        enemy.visuals.EnableIK(true, true);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceSpeed;

        if (enemy.IsUnstoppable())
        {
            enemy.visuals.EnableIK(true, false);
            stateTimer = enemy.advanceDuration;
        }
    }

    public override void Exit()
    {
        Debug.Log("ÍË³ö×´Ì¬£ºAdvance");
        base.Exit();
        lastTimeAdvanced = Time.time;
    }

    public override void Update()
    {
        base.Update();
        
        playerPos = enemy.player.transform.position;
        enemy.UpdateAimPosition();

        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPathPoint());

        if (CanEnterBattleState() || enemy.IsSeeingPlayer())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
    
    private bool CanEnterBattleState()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, playerPos);
        bool closeEnoughToPlayer = distanceToPlayer < enemy.advanceStoppingDistance;
        //Debug.Log($"¾àÀëPlayer:{distanceToPlayer} ÊÇ·ñ×·¸Ï£º{!closeEnoughToPlayer}");
        if (enemy.IsUnstoppable()) return closeEnoughToPlayer || stateTimer < 0;
        else return closeEnoughToPlayer;
    }
}