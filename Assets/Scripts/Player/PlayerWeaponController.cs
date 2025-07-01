using NUnit.Framework;
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
        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine; //TODO:本来是没有的

        player.weaponVisuals.PlayWeaponEquipAnimation();
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

    private void Shoot()
    {
        if (WeaponReady() == false) return;

        /* 检查当前武器子弹数量 */
        if(currentWeapon.CanShoot() == false) return;

        if (currentWeapon.shootType == ShootType.Single) isShooting = false;

        /* 从对象池中获取子弹GameObject */
        //GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        GameObject newBullet = ObjectPool.Instance.GetBullet();
        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;

        player.weaponVisuals.PlayerFireAnimation();
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
    public Weapon CurrentWeapon() => currentWeapon;
    public Weapon BackupWeapon()
    {
        foreach(Weapon weapon in weaponSlots)
        {
            if(weapon != currentWeapon) return weapon;
        }

        return null;
    }
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

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }
    #endregion
}