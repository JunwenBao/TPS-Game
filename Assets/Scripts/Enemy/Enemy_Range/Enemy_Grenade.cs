using System.Collections.Generic;
using UnityEngine;

public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMultiplier = 1f;
    private Rigidbody rb;
    private float timer;
    private float impactPower;

    private LayerMask allyLayerMask;
    private bool canExplode = true;

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && canExplode) Explode();
    }

    private void Explode()
    {
        canExplode = false;

        PlayExplosionFX();

        HashSet<GameObject> uniqueEntities = new HashSet<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider hit in colliders)
        {
            if (IsTargetValid(hit) == false) continue;

            /* 添加根Gameobject，防止爆炸伤害，对多个部位的Collider造成重复伤害 */
            GameObject rootEntity = hit.transform.root.gameObject;
            if (uniqueEntities.Add(rootEntity) == false) continue;
            ApplyDamageTo(hit);
            ApplyPhysicalForceTo(hit);
        }
    }

    // 对目标造成物理力
    private void ApplyPhysicalForceTo(Collider hit)
    {
        Rigidbody rb = hit.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier, ForceMode.Impulse);
        }
    }

    // 对目标造成伤害
    private static void ApplyDamageTo(Collider hit)
    {
        IDamagable damagable = hit.GetComponent<IDamagable>();
        damagable?.TakeDamage();
    }

    // 播放爆炸特效
    private void PlayExplosionFX()
    {
        GameObject newFx = ObjectPool.Instance.GetObject(explosionFx, transform);
        ObjectPool.Instance.ReturnObject(newFx, 1);
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }

    // 设置手雷
    public void SetupGrenade(LayerMask allyLayerMask, Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        canExplode = true;

        this.allyLayerMask = allyLayerMask;
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
    }

    private bool IsTargetValid(Collider collider)
    {
        if (GameManager.Instance.firendlyFire) return true;

        if ((allyLayerMask.value & (1 << collider.gameObject.layer)) > 0) return false;

        return true;
    }

    // 计算手雷飞行速度
    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);

        Vector3 velocityXZ = directionXZ / timeToTarget;

        float arcHeight = 2f; 

        float velocityY =
            (direction.y + arcHeight - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2f) / timeToTarget;

        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;

        return launchVelocity;
    }

}