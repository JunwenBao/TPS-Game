using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;

    [SerializeField] private Weapon currentWeapon;

    [Header("Bullet Detals")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

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

    #region Slot Managment - Pickup/Equip/Drop Weapon

    private void EquipStartingWeapon() => EquipWeapon(0);

    // 装备武器
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine;

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
    #endregion 

    private void Shoot()
    {
        /* 检查当前武器子弹数量 */
        if(currentWeapon.CanShoot() == false) return;

        /* 生成子弹GameObject */
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;

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
    public Transform GunPoint() => gunPoint;

    #region GameInputEvent
    // 输入事件
    private void AssignInputEvent()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
            {
                player.weaponVisuals.PlayReloadAnimation();
            }
        };
    }
    #endregion
}