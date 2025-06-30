using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private const float REFERENCE_BULLET_SPEED = 20;

    private Player player;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Weapon secondWeapon;

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

        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    // �����¼�
    private void AssignInputEvent()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
    }

    // װ������
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    // ��������
    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1) return;

        weaponSlots.Remove(currentWeapon);

        currentWeapon = weaponSlots[0];
    }

    // ʰȡ����
    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots) return;
        
        weaponSlots.Add(newWeapon);
    }

    private void Shoot()
    {
        /* �����ӵ����� */
        if(currentWeapon.ammo <= 0)
        {
            return;
        }
        currentWeapon.ammo--;

        /* �����ӵ�GameObject */
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

        /* �����ӵ����з��� */
        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

        return direction;
    }

    public Transform GunPoint() => gunPoint;
}