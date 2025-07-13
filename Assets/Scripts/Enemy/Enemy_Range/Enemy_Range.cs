using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("Cover system")]
    public float minCoverTime;
    public float safeDistance;
    public CoverPoint currentCover { get; private set; }
    public CoverPoint lastCover { get; private set; }

    [Header("Weapon Details")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_Range_WeaponData weaponData;

    public Transform gunPoint;
    public Transform weaponHolder;
    public GameObject bulletPrefab;

    [SerializeField] private List<Enemy_Range_WeaponData> availableWeaponData;

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
        SetupWeapon();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void FireSingleBullet()
    {
        animator.SetTrigger("Shoot");

        Vector3 bulletDirection = ((player.position + Vector3.up) - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();

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

    #region Cover System

    public bool CanGetCover()
    {
        //if (coverPerk == CoverPerk.Unavalible) return false;

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (lastCover != currentCover && currentCover != null)
            return true;

        Debug.LogWarning("No cover found!");
        return false;
    }

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

    // 收集敌人范围内的所有Cover Point
    private List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 30;
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

        return collectedCovers;
    }

    #endregion
}