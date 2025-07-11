using UnityEngine;

public enum Enemy_Range_WeaponHoldType { Common, LowHold, HighHold };

public class Enemy_Range_WeaponModel : MonoBehaviour
{
    public Transform gunPoint;

    public Enemy_RangeWeaponType weaponType;
    public Enemy_Range_WeaponHoldType weaponHoldType;

    public Transform leftHandTarget;
    public Transform leftElbowTarget;
}
