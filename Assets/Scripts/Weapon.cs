using NUnit.Framework;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    AutoRifle,
    Shotgun,
    Sniper
}

public enum ShootType
{
    Single,
    Auto
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting Specifics")]
    public ShootType shootType;
    public int bulletsPerShot;
    public float defaultFireRate;
    public float fireRate = 1; // ���٣�n/s
    private float lastShootTime;

    [Header("Burst Fire")]
    public bool burstAvailble;
    public bool burstActive;

    public int burstFireRate;
    public float burstModeFireRate;
    public float burstFireDelay = 0.1f;

    [Header("Magzine Details")]
    public int bulletInMagzine;  // �����ӵ�����
    public int magzineCapacity;  // ��������
    public int totalReserveAmmo; // ���ӵ�����

    [UnityEngine.Range(1, 5)]
    public float reloadSpeed = 1f;
    [UnityEngine.Range(1, 2)]
    public float equipmentSpeed = 1f;
    [UnityEngine.Range(2, 20)]
    public float gunDistance = 4f;

    [Header("Spread")]
    public float baseSpread;                 // ����spreadֵ
    public float maximumSpread = 3;          // ���spreadֵ
    private float currentSpread = 2;         // ��ǰspreadֵ

    public float spreadIncreaseRate = 0.15f; //spreadֵ����

    private float lastSpreadUpdateTime = 1;  // ������spreadֵ��ʱ�䣨�������ø�ֵ��
    private float spreadCooldown;            // ����spreadֵ����ʱ��

    #region Spread Methods
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if(Time.time > lastSpreadUpdateTime + spreadCooldown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            IncreaseSpread();
        }

        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }
    #endregion

    #region Burst Methods

    public bool BurstActivated()
    {
        if(weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    // �޸�Burst״̬
    public void ToggleBurst()
    {
        if (burstAvailble == false) return;

        burstActive = !burstActive;

        if(burstActive)
        {
            bulletsPerShot = burstFireRate;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion

    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();
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