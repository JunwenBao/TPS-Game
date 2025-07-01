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

    public int bulletInMagzine;  // 弹夹子弹数量
    public int magzineCapacity;  // 弹夹数量
    public int totalReserveAmmo; // 总子弹数量

    [UnityEngine.Range(1, 5)]
    public float reloadSpeed = 1;
    [UnityEngine.Range(1, 2)]
    public float equipmentSpeed = 1;

    [Space]
    public float fireRate = 1; // 射速：n/s

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
        /* 计算射速 */
        if(Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    #region Reload Methods
    // 判断：武器是否有足够多的弹药
    private bool HaveEnoughBullets() => bulletInMagzine > 0;

    // 判断：剩余子弹数量是否可以装弹
    public bool CanReload()
    {
        if (bulletInMagzine == magzineCapacity) return false;

        if (totalReserveAmmo > 0) return true;

        return false;
    }

    // 装弹
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