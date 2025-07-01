using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private Player player;
    private Animator animator;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private Rig rig;
    private bool shouldIncrease_RigWeight;

    [Header("Left Hand IK")]
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIK_Target;
    private bool shouldIncrease_LeftHandIKWeight;

    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    // 获取武器模型：主要得到GunPoint和LeftHandPoint
    public WeaponModel CurrentWeaponModle()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        for(int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
            {
                weaponModel = weaponModels[i];
            }
        }

        return weaponModel;
    }

    public void PlayerFireAnimation() => animator.SetTrigger("Fire");

    // 播放装弹动画
    public void PlayReloadAnimation()
    {
        float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;

        animator.SetFloat("ReloadSpeed", reloadSpeed);
        animator.SetTrigger("Reload");

        ReduceRigWeight();
    }

    // 切换指定武器
    public void SwitchOnCurrentWeaponModel()
    {
        int animationLayerIndex = (int)CurrentWeaponModle().holdType;

        /* 首先关闭所有武器 */
        SwitchOffWeaponModels();

        /* 显示第二武器 */
        SwitchOffBakcupWeaponModel();
        if(player.weapon.HasOnlyOoneWeapon() == false)
        {
            SwichOnBackupWeaponWeaponModel();
        }

        /* 触发拿取武器动画 */
        SwitchAnimationLayer(animationLayerIndex);
        CurrentWeaponModle().gameObject.SetActive(true);
        AttachLeftHand();
    }

    private void SwitchOffBakcupWeaponModel()
    {
        foreach(BackupWeaponModel backupModel in backupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
        }
    }

    public void SwichOnBackupWeaponWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach(BackupWeaponModel backupModel in backupWeaponModels)
        {
            if(backupModel.weaponType == weaponType)
            {
                backupModel.gameObject.SetActive(true);
            }
        }
    }

    // 关闭所有武器
    public void SwitchOffWeaponModels()
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    // 切换不同武器的动画层
    private void SwitchAnimationLayer(int layerIndex)
    {
        for(int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    // 控制拿取武器动画
    public void PlayWeaponEquipAnimation()
    {
        EquipType equipType = CurrentWeaponModle().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;

        leftHandIK.weight = 0;
        ReduceRigWeight();
        animator.SetTrigger("EquipWeapon");
        animator.SetFloat("EquipType", (float)equipType);
        animator.SetFloat("EquipSpeed", equipmentSpeed);
    }

    #region Animation Rigging Method

    // 切换武器时，绑定左手在不同武器的位置
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModle().holdPoint;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1) shouldIncrease_LeftHandIKWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1) shouldIncrease_RigWeight = false;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    public void MaximizeRigWeight() => shouldIncrease_RigWeight = true;
    public void MaximizeLeftHandWeight() => shouldIncrease_LeftHandIKWeight = true;

    #endregion
}