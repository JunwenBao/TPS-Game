using NUnit.Framework;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    AutoRifle,
    Shotgun,
    Sniper
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public int bulletInMagzine;  // �����ӵ�����
    public int magzineCapacity;  // ��������
    public int totalReserveAmmo; // ���ӵ�����

    [UnityEngine.Range(1, 5)]
    public float reloadSpeed = 1;
    [UnityEngine.Range(1, 2)]
    public float equipmentSpeed = 1;

    [Space]
    public float fireRate = 1; // ���٣�n/s

    private float lastShootTime;

    public bool CanShoot()
    {
        if(HaveEnoughBullets() && ReadyToFire())
        {
            bulletInMagzine--;
            return true;
        }

        return false;
    }

    private bool ReadyToFire()
    {
        /* �������� */
        if(Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    #region Reload Methods
    // �жϣ������Ƿ����㹻��ĵ�ҩ
    private bool HaveEnoughBullets() => bulletInMagzine > 0;

    // �жϣ�ʣ���ӵ������Ƿ����װ��
    public bool CanReload()
    {
        if (bulletInMagzine == magzineCapacity) return false;

        if (totalReserveAmmo > 0) return true;

        return false;
    }

    // װ��
    public void RefillBullets()
    {
        totalReserveAmmo += bulletInMagzine;

        int bulletsToReload = magzineCapacity;

        if(bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletInMagzine = bulletsToReload;

        if(totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
    #endregion
}