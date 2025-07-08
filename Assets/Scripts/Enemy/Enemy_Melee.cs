using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;
    [UnityEngine.Range(1, 2)]
    public float animationSpeed;
    public AttackType_Melee attackType;
}

public enum AttackType_Melee { Close, Charge }
public enum EnemyMelee_Type  { Regular, Shield, Dodge, AxeThrow }

public class Enemy_Melee : Enemy
{
    public IdleState_Melee     idleState {  get; private set; }
    public MoveState_Melee     moveState {  get; private set; }
    public RecoveryState_Melee recoveryState {  get; private set; }
    public ChaseState_Melee    chaseState {  get; private set; }
    public AttackState_Melee   attackState {  get; private set; }
    public DeadState_Melee     deadState {  get; private set; }
    public AbilityState_Melee  abilityState {  get; private set; }

    [Header("Enemy Settings")]
    public EnemyMelee_Type meleeType;
    public Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodge;

    [Header("Axe Throw Ability")]
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float axeAimTimer;
    public float axeThrowCooldown;
    public float lastTimerAxeTrown;
    public Transform axeStartPoint;

    [Header("Attack Data")]
    public AttackData attackData;
    public List<AttackData> attackList;

    // 武器
    [SerializeField] private Transform hiddernWeapon;
    [SerializeField] private Transform pulledWeapon;

    protected override void Awake()
    {
        base.Awake();

        idleState     = new IdleState_Melee(this, stateMachine, "Idle");
        moveState     = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState    = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState   = new AttackState_Melee(this, stateMachine, "Attack");
        deadState     = new DeadState_Melee(this, stateMachine, "Idle");
        abilityState  = new AbilityState_Melee(this, stateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        InitializeSpeciality();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        moveSpeed = moveSpeed * 0.6f;
        pulledWeapon.gameObject.SetActive(false);
    }

    // 初始化敌人的类型：正常 / 持盾
    private void InitializeSpeciality()
    {
        if(meleeType == EnemyMelee_Type.Shield)
        {
            animator.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
        }
    }

    public override void GetHit()
    {
        base.GetHit();

        if(healthPoints <= 0) stateMachine.ChangeState(deadState);
    }

    public void PullWeapon()
    {
        hiddernWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;

    public void ActivateDogeRoll()
    {
        if (meleeType != EnemyMelee_Type.Dodge) return;

        if (stateMachine.currentState != chaseState) return;

        if (Vector3.Distance(transform.position, player.position) < 1.5f) return;

        if(Time.time > dodgeCooldown + lastTimeDodge)
        {
            lastTimeDodge = Time.time;
            animator.SetTrigger("Dodge");
        }
    }

    public bool CanThrowAxe()
    {
        if (meleeType != EnemyMelee_Type.AxeThrow) return false;

        if(Time.time > lastTimeDodge + axeThrowCooldown)
        {
            lastTimerAxeTrown = Time.time;
            return true;
        }

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, attackData.attackRange);
    }
}