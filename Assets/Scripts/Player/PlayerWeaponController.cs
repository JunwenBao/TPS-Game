using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PlayerWeaponController : MonoBehaviour
{
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

    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);

        EquipWeapon(0);
    }

    // װ������
    private void EquipWeapon(int i)
    {
        if(i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine; //TODO:������û�е�

        player.weaponVisuals.PlayWeaponEquipAnimation();

        CameraManager.Instance.ChangeCameraDistance(currentWeapon.cameraDistance);
    }

    // ��������
    private void DropWeapon()
    {
        if (HasOnlyOoneWeapon()) return;

        CreateWeaponOnTheGround();

        weaponSlots.Remove(currentWeapon);
        EquipWeapon(0);
    }

    // �ڽ�ɫ��ǰλ������һ��������������Gameobjct
    private void CreateWeaponOnTheGround()
    {
        GameObject droppedWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab);
        droppedWeapon.GetComponent<Pickup_Weapon>()?.SetupPickupWeapon(currentWeapon, transform);
    }

    // ʰȡ����
    public void PickupWeapon(Weapon newWeapon)
    {
        if(WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.bulletInMagzine;
            return;
        }

        // �жϣ����������������ﵽ���ޣ�
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

    //
    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;
    #endregion 

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false); // ������������ڶ෢���ʱ�����ٴ����

        for(int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if(i >= currentWeapon.bulletsPerShot) SetWeaponReady(true); // ����
        }
    }

    private void Shoot()
    {
        if (WeaponReady() == false) return;

        /* ��鵱ǰ�����ӵ����� */
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

    // �������
    private void FireSingleBullet()
    {
        currentWeapon.bulletInMagzine--;

        /* �Ӷ�����л�ȡ�ӵ�GameObject */
        //GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab);
        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BuletSetup(currentWeapon.gunDistance, bulletImpactForce);

        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    // ����װ��
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

        /* �����ӵ����з��� */
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
    // �����¼�
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