using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform[] gunTransforms;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private Transform currentGun;

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private bool rigShouldBeIncreased;

    [Header("Left Hand IK")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHand_Target;
    [SerializeField] private float leftHandIK_IncreaseStep;
    private bool shouldIncreaseLeftHandWeight;
    private Rig rig;

    private bool busyGrabbingWeapon;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();

        SwitchOn(pistol);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && busyGrabbingWeapon == false)
        {
            animator.SetTrigger("Reload");
            PauseRig();
        }

        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandWeight)
        {
            leftHandIK.weight += leftHandIK_IncreaseStep * Time.deltaTime;

            if (leftHandIK.weight >= 1) shouldIncreaseLeftHandWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;

            if (rig.weight >= 1) rigShouldBeIncreased = false;
        }
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    public void ReturnRigWeightToOne() => rigShouldBeIncreased = true;
    public void ReturnWeightToLeftHandIK() => shouldIncreaseLeftHandWeight = true;

    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(sniper);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for(int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHand_Target.localPosition = targetTransform.localPosition;
        leftHand_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for(int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    // ¿ØÖÆÄÃÈ¡ÎäÆ÷¶¯»­
    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        PauseRig();
        animator.SetFloat("WeaponGrabType", (float)grabType);
        animator.SetTrigger("WeaponGrab");
        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        busyGrabbingWeapon = busy;
        animator.SetBool("BusyGrabbingWeapon", busyGrabbingWeapon);
    }
}

public enum GrabType { SideGrab, BackGrab };