using System.Collections.Generic;
using System.Threading;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public struct AttackData_EnemyMelee
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
    public Enemy_MeleeWeaponType weaponType;

    public Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodge = -10f;

    [Header("Axe Throw Ability")]
    public GameObject axePrefab;
    public float axeFlySpeed;
    public float axeAimTimer;
    public float axeThrowCooldown;
    public float lastTimerAxeTrown;
    public Transform axeStartPoint;

    [Header("Attack Data")]
    public AttackData_EnemyMelee attackData;
    public List<AttackData_EnemyMelee> attackList;

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

        InitializePerk();

        visuals.SetupLook();
        UpdateAttackData();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();
        stateMachine.ChangeState(recoveryState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        walkSpeed = walkSpeed * 0.6f;
        visuals.EnableWeaponModel(false);
    }

    public void UpdateAttackData()
    {
        Enemy_WeaponModel currentWeapon = visuals.currentWeaponModel.GetComponent<Enemy_WeaponModel>();

        if(currentWeapon.weaponData != null)
        {
            attackList = new List<AttackData_EnemyMelee>(currentWeapon.weaponData.attackData);
            turnSpeed = currentWeapon.weaponData.turnSpeed;
        }
    }

    // 初始化敌人的类型 + 外观
    protected override void InitializePerk()
    {
        if(meleeType == EnemyMelee_Type.AxeThrow)
        {
            weaponType = Enemy_MeleeWeaponType.Throw;
        }

        if(meleeType == EnemyMelee_Type.Shield)
        {
            animator.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            weaponType = Enemy_MeleeWeaponType.OneHand;
        }

        if(meleeType == EnemyMelee_Type.Dodge)
        {
            weaponType = Enemy_MeleeWeaponType.Unarmed;
        }
    }

    public override void Die()
    {
        base.Die();

        if(stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;

    // 触发翻滚闪避
    public void ActivateDogeRoll()
    {
        if (meleeType != EnemyMelee_Type.Dodge) return;

        if (stateMachine.currentState != chaseState) return;

        if (Vector3.Distance(transform.position, player.position) < 1.5f) return;

        float dodgeAnimationDuration = GetAnimationClipDuration("Dodge Roll");

        if(Time.time > dodgeCooldown + dodgeAnimationDuration + lastTimeDodge)
        {
            lastTimeDodge = Time.time;
            animator.SetTrigger("Dodge");
        }
    }

    public void ThrowAxe()
    {
        GameObject newAxe = ObjectPool.Instance.GetObject(axePrefab, axeStartPoint);

        newAxe.GetComponent<EnemyAxe>().AxeSetup(axeFlySpeed, player, axeAimTimer);
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

    private void ResetCoolDown()
    {
        lastTimeDodge -= dodgeCooldown;
        lastTimerAxeTrown -= axeThrowCooldown;
    }

    private float GetAnimationClipDuration(string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach(AnimationClip clip in clips)
        {
            if(clip.name == clipName) return clip.length;
        }

        Debug.Log(clipName + "Not found!");

        return 0;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, attackData.attackRange);
    }
}