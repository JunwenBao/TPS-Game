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

    // װ������
    private void EquipWeapon(int i)
    {
        if(i >= weaponSlots.Count) return;

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        currentWeapon.bulletInMagzine = weaponSlots[i].bulletInMagzine; //TODO:������û�е�

        player.weaponVisuals.PlayWeaponEquipAnimation();
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
        GameObject droppedWeapon = ObjectPool.Instance.GetObject(weaponPickupPrefab, transform);
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

    // ����׼������
    public void SetWeaponReady(bool ready)
    {
        weaponReady = ready;

        /* ��ʼ������UI */
        weaponUI.InitialWeaponUI(currentWeapon.weaponType, currentWeapon.bulletInMagzine, currentWeapon.totalReserveAmmo);

        /* ������Ч */
        if(ready) player.sound.weaponReady.Play();
    }

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

    // ���
    private void Shoot()
    {
        /* ��鵱ǰ�����Ƿ������� */
        if (WeaponReady() == false) return;

        /* ��鵱ǰ�����ӵ����� */
        if (currentWeapon.CanShoot() == false) return;

        /* ����������� */
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

    // �������
    private void FireSingleBullet()
    {
        currentWeapon.bulletInMagzine--;

        /* ����UI */
        weaponUI.UpdateWeaponUI();

        /* ���������Ч */
        player.weaponVisuals.CurrentWeaponModle().fireSFX.Play();

        /* �Ӷ�����л�ȡ�ӵ�GameObject */
        GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, GunPoint());

        /* �ӵ��������� */
        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());
        newBullet.transform.rotation = Quaternion.LookRotation(bulletDirection);

        /* �ӵ��������� */
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(whatIsAlly, currentWeapon.gunDistance, bulletImpactForce);

        /* �ӵ������˶����� */
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }

    // ����װ��
    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayReloadAnimation();

        player.weaponVisuals.CurrentWeaponModle().reloadSFX.Play();
    }

    // �����ӵ��ķ��з���
    public Vector3 BulletDirection()
    {
        /* ʹ������aim��λ�� - ǹ�ڵ�λ�ã����ɵó����з���vec3 */
        Transform aim = player.aim.Aim();

        Vector3 targetPoint = player.aim.GetHitPosition();
        Vector3 direction = (targetPoint - GunPoint().position).normalized;
        /*
        /* ���δ���þ�ȷ��׼ 
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

    // ������������
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
    // �����¼�
    private void AssignInputEvent()
    {
        PlayerControls controls = player.controls;

        /* ��������� */
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        /* �������л����� */
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);

        /* �������������� */
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();

        /* ������װ������ */
        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };

        /* ����������ģʽ���� */
        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();
    }
    #endregion
}