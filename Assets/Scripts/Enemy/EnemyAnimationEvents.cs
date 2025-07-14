using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger()    => enemy.AnimationTrigger();
    public void StartManualMovement() => enemy.ActiveManualMovement(true);
    public void StopManualMovement()  => enemy.ActiveManualMovement(false);
    public void StartManualRotation() => enemy.ActiveManualRotation(true);
    public void StopManualRotation()  => enemy.ActiveManualRotation(false);
    public void AbilityEvent() => enemy.AbilityTrigger();
    public void EnableIK() => enemy.visuals.EnableIK(true, true, 1.5f);

    public void EnableWeaponModel()
    {
        enemy.visuals.EnableWeaponModel(true);
        enemy.visuals.EnableSeconderyWeaponModel(false);
    }
}