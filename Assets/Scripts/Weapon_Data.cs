using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;

    [Header("Regular Shot")]
    public WeaponType weaponType;
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate;

    [Header("Burst Shot")]
    public bool burstAvailble;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = 0.1f;

    [Header("Weapon Spread")]
    public float baseSpread;
    public float maxSpread;
    public float spreadIncreaseRate = 0.15f;

    [Header("Weapon Generics")]
    [Range(1, 3)]
    public float reloadSpeed;
    [Range(1, 3)]
    public float equipmentSpeed;
    [Range(4, 8)]
    public float gunDistance = 4f;
    [Range(4, 8)]
    public float cameraDistance = 1f;
}