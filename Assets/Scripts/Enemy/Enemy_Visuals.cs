using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Enemy_MeleeWeaponType { OneHand, Throw }

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon Model")]
    [SerializeField] private Enemy_WeaponModel[] weaponModels;
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel {  get; private set; }

    [Header("Corruption Visuals")]
    [SerializeField] private GameObject[] corruptionCrystals;
    [SerializeField] private int corruptionAmount;

    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        CollectCorruptionCrystals();
    }

    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }

    // 设置角色随机外观
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    // 设置武器类型
    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;

    // 为角色设置随机武器
    private void SetupRandomWeapon()
    {
        foreach(var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);
        }

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach(var weaponModel in weaponModels)
        {
            if(weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.SetActive(true);
    }

    // 将角色的Texture设置为随机Texture
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }

    // 设置随机的额外外观
    private void SetupRandomCorruption()
    {
        List<int> availableIndex = new List<int>();

        for(int i = 0; i < corruptionCrystals.Length; i++)
        {
            availableIndex.Add(i);
            corruptionCrystals[i].SetActive(false);
        }

        for(int i = 0; i < corruptionAmount; i++)
        {
            if (availableIndex.Count == 0) break;

            int randomIndex = Random.Range(0, availableIndex.Count);
            int objectIndex = availableIndex[randomIndex];

            corruptionCrystals[objectIndex].SetActive(true);
            availableIndex.RemoveAt(randomIndex);
        }
    }

    // 获取所有的额外外观
    private void CollectCorruptionCrystals()
    {
        Enemy_CorruptionCrystal[] crystalComponents = GetComponentsInChildren<Enemy_CorruptionCrystal>(true);
        corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }
    }
}