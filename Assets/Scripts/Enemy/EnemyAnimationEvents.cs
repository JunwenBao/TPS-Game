using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Melee enemyMelee;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyMelee = GetComponentInParent<Enemy_Melee>();
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
    /*
    public void BossJumpImpact()
    {
        enemyBoss?.JumpImpact();
    }
    */

    // 开始检查近战攻击
    public void BeginMeleeAttackCheck()
    {
        enemyMelee?.EnableAttackCheck(true);
    }
    // 结束检查近战攻击
    public void EndMeleeAttackCheck()
    {
        enemyMelee?.EnableAttackCheck(false);
    }
}