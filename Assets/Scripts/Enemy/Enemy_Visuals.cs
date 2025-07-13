using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum Enemy_MeleeWeaponType { OneHand, Throw, Unarmed }
public enum Enemy_RangeWeaponType { Pistol, Rifle, Shugun, Sniper }

public class Enemy_Visuals : MonoBehaviour
{
    public GameObject currentWeaponModel {  get; private set; }

    [Header("Corruption Visuals")]
    [SerializeField] private GameObject[] corruptions;
    [SerializeField] private int corruptionAmount;

    [Header("Color Visuals")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Rig References")]
    [SerializeField] private Transform leftHandIK;
    [SerializeField] private Transform leftElowIK;
    [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;
    [SerializeField] private MultiAimConstraint weaponAimConstraint;

    // 设置角色随机外观
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
        SetupRandomCorruption();
    }

    #region 为敌人设置随机武器

    // 为角色设置随机武器
    private void SetupRandomWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;

        if(thisEnemyIsMelee) currentWeaponModel = FindMeleeWeaponModel();
        if(thisEnemyIsRange) currentWeaponModel = FindRangeWeaponModel();

        currentWeaponModel.SetActive(true);
    }

    // 寻找所有的Melee类型武器：随机选择
    private GameObject FindMeleeWeaponModel()
    {
        Enemy_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
        Enemy_MeleeWeaponType weaponType = GetComponent<Enemy_Melee>().weaponType;

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                filteredWeaponModels.Add(weaponModel);
            }
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        return filteredWeaponModels[randomIndex].gameObject;
    }

    // 寻找所有的Range类型武器：根据敌人使用的武器类型选择
    private GameObject FindRangeWeaponModel()
    {
        Enemy_Range_WeaponModel[] weaponModels = GetComponentsInChildren<Enemy_Range_WeaponModel>(true);
        Enemy_RangeWeaponType weaponType = GetComponent<Enemy_Range>().weaponType;

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
            {
                SwitchAnimationLayer((int)weaponModel.weaponHoldType);
                SetupLeftHandIK(weaponModel.leftHandTarget, weaponModel.leftElbowTarget);
                return weaponModel.gameObject;
            }
        }

        return null;
    }

    #endregion

    #region 为敌人设置随机外观

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
        corruptions = CollectCorruptions();

        for(int i = 0; i < corruptions.Length; i++)
        {
            availableIndex.Add(i);
            corruptions[i].SetActive(false);
        }

        for(int i = 0; i < corruptionAmount; i++)
        {
            if (availableIndex.Count == 0) break;

            int randomIndex = Random.Range(0, availableIndex.Count);
            int objectIndex = availableIndex[randomIndex];

            corruptions[objectIndex].SetActive(true);
            availableIndex.RemoveAt(randomIndex);
        }
    }

    // 获取所有的额外外观
    private GameObject[] CollectCorruptions()
    {
        Enemy_Attach[] crystalComponents = GetComponentsInChildren<Enemy_Attach>(true);
        GameObject[] corruptionCrystals = new GameObject[crystalComponents.Length];

        for (int i = 0; i < crystalComponents.Length; i++)
        {
            corruptionCrystals[i] = crystalComponents[i].gameObject;
        }

        return corruptionCrystals;
    }

    #endregion

    // 武器攻击特效
    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }

    private void OverrideAnimatorControllerIfCan()
    {
        
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        Animator animator = GetComponentInChildren<Animator>();

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    public void EnableIK(bool enableLeftHand, bool enableAim)
    {
        leftHandIKConstraint.weight = enableLeftHand ? 1 : 0;
        weaponAimConstraint.weight = enableAim ? 1 : 0;
    }

    private void SetupLeftHandIK(Transform leftHandTarget, Transform LeftElowTarget)
    {
        leftHandIK.localPosition = leftHandTarget.localPosition;
        leftHandIK.localRotation = leftHandTarget.localRotation;

        leftElowIK.localPosition = LeftElowTarget.localPosition;
        leftElowIK.localRotation = LeftElowTarget.localRotation;
    }
}