using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;

    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("Bullet Detals")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvent();

        Invoke("EquipStartingWeapon", 0.1f);
    }

    private void Update()
    {
        if(isShooting) Shoot();
    }

    #region Slot Managment - Pickup/Equip/Drop Weapon

    private void EquipStartingWeapon() => EquipWeapon(0);

    // 装备武器
    private void EquipWeapon(int i)
    {
        if(i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine; //TODO:本来是没有的

        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.Instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }

    // 丢弃武器
    private void DropWeapon()
    {
        if (HasOnlyOoneWeapon()) return;

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    // 拾取武器
    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots) return;
        
        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwichOnBackupWeaponWeaponModel();
    }

    //
    public void SetWeaponReady(bool ready) => weaponReady = ready;
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

    private void Shoot()
    {
        if (WeaponReady() == false) return;

        /* 检查当前武器子弹数量 */
        if (currentWeapon.CanShoot() == false) return;

        player.weaponVisuals.PlayerFireAnimation();

        if (currentWeapon.shootType == ShootType.Single) isShooting = false;

        if(currentWeapon.BurstActivated() == true)
        {
            StartCoroutine(BurstFire());
            return;
        }

        FireSingleBullet();
    }

    // 单发射击
    private void FireSingleBullet()
    {
        currentWeapon.bulletInMagzine--;

        /* 从对象池中获取子弹GameObject */
        //GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        GameObject newBullet = ObjectPool.Instance.GetBullet();
        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BuletSetup(currentWeapon.gunDistance);

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    // 重新装弹
    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if(player.aim.CanAimPrecisly() == false && player.aim.Target() == null) direction.y = 0;

        /* 控制子弹飞行方向 */
        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

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

    #region GameInputEvent
    // 输入事件
    private void AssignInputEvent()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();
    }
    #endregion
}