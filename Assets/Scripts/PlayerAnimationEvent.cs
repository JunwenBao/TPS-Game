using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver()
    {
        visualController.ReturnRigWeightToOne();
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeightToOne();
        visualController.ReturnWeightToLeftHandIK();
    }

    public void WeaponGrabIsOver()
    {
        visualController.SetBusyGrabbingWeaponTo(false);
    }
}