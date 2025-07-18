using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsAlly;
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;

    [SerializeField] private Weapon_Data defaultWeaponData;

    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("Bullet Detals")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletImpactForce = 100;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 4;
    [SerializeField] private List<Weapon> weaponSlots;

    [SerializeField] private GameObject weaponPickupPrefab;

    [Header("Weapon UI")]
    [SerializeField] private Weapon_UI weaponUI;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvent();

        Invoke(nameof(EquipStartingWeapon), 0.1f);
    }

    private void Update()
    {
        if(isShooting) Shoot();
    }

    #region Slot Managment - Pickup/Equip/Drop Weapon

    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);

        EquipWeapon(0);
    }

    // 装备武器
    private void EquipWeapon(int i)
    {
        if(i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine; //TODO:本来是没有的

        player.weaponVisuals.PlayWeaponEquipAnimation();
    }

    // 丢弃武器
    private void DropWeapon()
    {
        if (HasOnlyOoneWeapon()) return;

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    // 在角色当前位置生成一个被丢弃武器的Gameobjct
    private void CreateWeaponOnTheGround()
    {
        GameObject droppedWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab, transform);
        droppedWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    // 拾取武器
    public void PickupWeapon(Weapon newWeapon)
    {
        if(WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletInMagzine;
            return;
        }

        // 判断：当持有武器数量达到上限，
        if (weaponSlots.Count >= maxSlots && newWeapon.weaponType != currentWeapon.weaponType)
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);

            player.weaponVisuals.SwitchOffWeaponModels();
            weaponSlots[weaponIndex] = newWeapon;

            CreateWeaponOnTheGround();
            EquipWeapon(weaponIndex);

            return;
        }

        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwichOnBackupWeaponWeaponModel();
    }

    // 武器准备就绪
    public void SetWeaponReady(bool ready)
    {
        weaponReady = ready;

        /* 初始化武器UI */
        weaponUI.InitialWeaponUI(currentWeapon.weaponType, currentWeapon.bulletInMagzine, currentWeapon.totalReserveAmmo);

        /* 播放音效 */
        if(ready) player.sound.weaponReady.Play();
    }

    public bool WeaponReady() => weaponReady;
    #endregion 

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false); // 设置射击锁：在多发射击时不能再次射击

        for(int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if(i >= currentWeapon.bulletsPerShot) SetWeaponReady(true); // 解锁
        }
    }

    // 射击
    private void Shoot()
    {
        /* 检查当前武器是否可以射击 */
        if (WeaponReady() == false) return;

        /* 检查当前武器子弹数量 */
        if (currentWeapon.CanShoot() == false) return;

        /* 触发射击动画 */
        player.weaponVisuals.PlayerFireAnimation();

        if (currentWeapon.shootType == ShootType.Single) isShooting = false;

        if(currentWeapon.BurstActivated() == true)
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
        TriggerEnemyDodge();
    }

    // 单发射击
    private void FireSingleBullet()
    {
        currentWeapon.bulletInMagzine--;

        /* 更新UI */
        weaponUI.UpdateWeaponUI();

        /* 播放射击音效 */
        player.weaponVisuals.CurrentWeaponModle().fireSFX.Play();

        /* 从对象池中获取子弹GameObject */
        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, GunPoint());

        /* 子弹方向设置 */
        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());
        newBullet.transform.rotation = Quaternion.LookRotation(bulletDirection);

        /* 子弹数据设置 */
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(whatIsAlly, currentWeapon.gunDistance, bulletImpactForce);

        /* 子弹刚体运动设置 */
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    // 重新装弹
    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();

        player.weaponVisuals.CurrentWeaponModle().reloadSFX.Play();
    }

    // 计算子弹的飞行方向
    public Vector3 BulletDirection()
    {
        /* 使用物体aim的位置 - 枪口的位置，即可得出飞行方向vec3 */
        Transform aim = player.aim.Aim();

        Vector3 targetPoint = player.aim.GetHitPosition();
        Vector3 direction = (targetPoint - GunPoint().position).normalized;
        /*
        /* 如果未启用精确瞄准 
        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }
        */
        return direction;
    }

    public bool HasOnlyOoneWeapon() => weaponSlots.Count <= 1;
    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach(Weapon weapon in weaponSlots)
        {
            if(weapon.weaponType == weaponType) return weapon;
        }

        return null;
    }
    public Weapon CurrentWeapon() => currentWeapon;
    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModle().gunPoint;

    // 触发敌人闪避
    private void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if(Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();

            if(enemy_Melee != null)
            {
                enemy_Melee.ActivateDogeRoll();
            }
        }
    }

    #region GameInputEvent
    // 输入事件
    private void AssignInputEvent()
    {
        PlayerControls controls = player.controls;

        /* 绑定射击控制 */
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        /* 绑定武器切换控制 */
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);

        /* 绑定武器丢弃控制 */
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        /* 绑定武器装弹控制 */
        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

        /* 绑定武器连发模式控制 */
        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();
    }
    #endregion
}