using UnityEngine;

public enum EquipType { SideEquipAnimation, BackEquipAnimation };
public enum HoldType { CommonHold = 1, LowHold = 2, HighHold = 3 };

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public EquipType equipAnimationType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;

    [Header("Audio")]
    public AudioSource fireSFX;
    public AudioSource reloadSFX;
}