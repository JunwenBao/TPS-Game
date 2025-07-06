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
    public ShootType shootType;

    public int bulletsPerShot {  get; private set; }

    private float defaultFireRate;
    public float fireRate = 1; // ���٣�n/s
    private float lastShootTime;

    public bool burstActive;
    private bool burstAvailble;

    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay {  get; private set; }

    [Header("Magzine Details")]
    public int bulletInMagzine;  // �����ӵ�����
    public int magzineCapacity;  // ��������
    public int totalReserveAmmo; // ���ӵ�����

    public float reloadSpeed    { get; private set; }
    public float equipmentSpeed { get; private set; }
    public float gunDistance    { get; private set; }
    public float cameraDistance { get; private set; }

    [Header("Spread")]
    private float baseSpread;                 // ����spreadֵ
    private float maximumSpread = 3;          // ���spreadֵ
    private float currentSpread = 2;         // ��ǰspreadֵ

    private float spreadIncreaseRate = 0.15f; //spreadֵ����

    private float lastSpreadUpdateTime = 1;  // ������spreadֵ��ʱ�䣨�������ø�ֵ��
    private float spreadCooldown;            // ����spreadֵ����ʱ��

    public Weapon_Data weaponData {  get; private set; }

    public Weapon(Weapon_Data weaponData)
    {
        bulletInMagzine = weaponData.bulletInMagzine;
        magzineCapacity = weaponData.magzineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;

        fireRate = weaponData.fireRate;
        weaponType = weaponData.weaponType;

        bulletsPerShot = weaponData.bulletsPerShot;
        shootType = weaponData.shootType;

        burstAvailble = weaponData.burstAvailble;
        burstActive = weaponData.burstActive;
        burstBulletsPerShot = weaponData.burstBulletsPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;

        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maxSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;

        reloadSpeed = weaponData.reloadSpeed;
        equipmentSpeed = weaponData.equipmentSpeed;
        gunDistance = weaponData.gunDistance;
        cameraDistance = weaponData.cameraDistance;

        defaultFireRate = fireRate;
        
        this.weaponData = weaponData;
    }

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
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstBulletsPerShot;
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