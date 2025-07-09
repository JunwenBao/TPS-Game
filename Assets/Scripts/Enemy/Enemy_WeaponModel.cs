using UnityEngine;

public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;

    [SerializeField] private GameObject[] trailEffects;

    public void EnableTrailEffect(bool enable)
    {
        foreach(var effect in trailEffects)
        {
            effect.SetActive(enable);
        }
    }
}