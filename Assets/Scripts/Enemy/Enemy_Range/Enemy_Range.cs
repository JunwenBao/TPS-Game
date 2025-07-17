using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum CoverPerk { Unavalible, CanTakeCover, CanTakeAndChangeCover }
public enum UnstoppablePerk { Unavalible, Unstoppable }
public enum GrenadePerk { Unavalible, CanThrowGrenade }

public class Enemy_Range : Enemy
{
    [Header("Enemy perks")]
    public CoverPerk coverPerk;
    public UnstoppablePerk unstoppablePerk;
    public GrenadePerk grenadePerk;

    [Header("Grenade perk")]
    public GameObject grenadePrefab;
    public float impactPower;
    public float explosionTimer = 0.75f;
    public float timeToTarget = 1.2f;
    public float grenadeCooldown;
    public float lastTimeGrenadeThrown;
    [SerializeField] private Transform grenadeStartPoint;
 
    [Header("Advance perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;
    public float advanceDuration = 2.5f;

    [Header("Cover system")]
    public float minCoverTime;
    public float safeDistance;
    //TODO: {ps}
    public CoverPoint currentCover;
    public CoverPoint lastCover;

    [Header("Weapon Details")]
    public float attackDelay;
    public Enemy_RangeWeaponType weaponType;
    public Enemy_Range_WeaponData weaponData;

    public Transform gunPoint;
    public Transform weaponHolder;
    public GameObject bulletPrefab;

    [Header("Aim Details")]
    public float slowAim = 4f;
    public float fastAim = 20f;
    public Transform aim;
    public Transform playerBody;
    public LayerMask whatToIgnore;

    [SerializeField] private List<Enemy_Range_WeaponData> availableWeaponData;

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
    public AdvancePlayerState_Range advancePlayerState { get; private set; }
    public ThrowGrenadeState_Range throwGrenadeState { get; private set; }
    public DeadState_Range deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        advancePlayerState = new AdvancePlayerState_Range(this, stateMachine, "Advance");
        throwGrenadeState = new ThrowGrenadeState_Range(this, stateMachine, "ThrowGrenade");
        deadState = new DeadState_Range(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        playerBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;

        InitializePerk();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
        SetupWeapon();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    protected override void InitializePerk()
    {
        if(IsUnstoppable())
        {
            animator.SetFloat("AdvanceAnimIndex", 1);
        }
    }

    // 敌人死亡
    public override void Die()
    {
        base.Die();

        if(stateMachine.currentState != deadState) stateMachine.ChangeState(deadState);
    }

    #region Grenade

    // 判断是否可以投掷手榴弹
    public bool CanThrowGrenade()
    {
        if (grenadePerk == GrenadePerk.Unavalible) return false;

        if(Vector3.Distance(player.transform.position, transform.position) < safeDistance) return false;

        if(Time.time > grenadeCooldown + lastTimeGrenadeThrown) return true;

        return false;
    }

    // 投掷手雷
    public void ThrowGrenade()
    {
        lastTimeGrenadeThrown = Time.time;
        visuals.EnableGrenadeModel(false);

        GameObject newGrenade = ObjectPool.Instance.GetObject(grenadePrefab, grenadeStartPoint);

        Enemy_Grenade newGrenadeScript = newGrenade.GetComponent<Enemy_Grenade>();
        
        /* 如果角色死亡，且正在投掷手雷，则让手雷滚到自己旁边 */
        if(stateMachine.currentState == deadState)
        {
            newGrenadeScript.SetupGrenade(whatIsAlly, transform.position, 1, explosionTimer, impactPower);
            return;
        }

        newGrenadeScript.SetupGrenade(whatIsAlly, player.transform.position, timeToTarget, explosionTimer, impactPower);
    }

    #endregion

    // 设置武器参数
    private void SetupWeapon()
    {
        List<Enemy_Range_WeaponData> filteredData = new List<Enemy_Range_WeaponData>();

        foreach (var weaponData in availableWeaponData)
        {
            if (weaponData.weaponType == weaponType)
            {
                filteredData.Add(weaponData);
            }
        }

        if (filteredData.Count > 0)
        {
            int random = Random.Range(0, filteredData.Count);
            weaponData = filteredData[random];
        }
        else
        {
            Debug.LogWarning("No avalible weapon data was found!");
        }

        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_Range_WeaponModel>().gunPoint;
    }

    #region Enemy Aim
    public void UpdateAimPosition()
    {
        float aimSpeed = IsAimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playerBody.position + Vector3.up * 2.5f, aimSpeed * Time.deltaTime);
    }

    public bool IsAimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, player.position);

        return distanceAimToPlayer < 5f;
    }

    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up * 2.5f;
        Vector3 directionToPlayer = playerBody.position - myPosition;

        if (Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform == playerBody)
            {
                UpdateAimPosition();
                return true;
            }
        }

        return false;
    }

    #endregion

    public void FireSingleBullet()
    {
        animator.SetTrigger("Shoot");

        Vector3 bulletDirection = (aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, gunPoint);
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Bullet>().BulletSetup(whatIsAlly);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirectionWithSpread * weaponData.bulletSpeed;
    }

    // 进入战斗状态
    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        if (CanGetCover())
        {
            stateMachine.ChangeState(runToCoverState);
        }
        else
        {
            stateMachine.ChangeState(battleState);
        }
    }

    #region Cover System

    // 判断：敌人是否有可用的掩体
    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavalible) return false;

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (lastCover != currentCover && currentCover != null) return true;

        Debug.LogWarning("No cover found!");

        return false;
    }

    // 尝试寻找掩体
    private Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();

        foreach (Cover cover in CollectNearByCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);
            if (currentDistance < shortestDistance)
            {
                closestCoverPoint = coverPoint;
                shortestDistance = currentDistance;
            }
        }

        if (closestCoverPoint != null)
        {
            lastCover?.SetOccupied(false);
            lastCover = currentCover;

            currentCover = closestCoverPoint;
            currentCover.SetOccupied(true);

            return currentCover.transform;
        }

        return null;
    }

    // 获取敌人一定范围内的所有Cover
    private List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 100;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverRadiusCheck);
        List<Cover> collectedCovers = new List<Cover>();

        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.GetComponent<Cover>();

            if (cover != null && collectedCovers.Contains(cover) == false)
            {
                collectedCovers.Add(cover);
            }
        }
        Debug.Log("获取到的掩体数量：" + collectedCovers.Count);
        return collectedCovers;
    }

    #endregion

    public bool IsUnstoppable() => unstoppablePerk == UnstoppablePerk.Unstoppable;
}