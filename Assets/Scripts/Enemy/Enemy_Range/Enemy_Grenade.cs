using UnityEngine;

public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMultiplier = 1f;
    private Rigidbody rb;
    private float timer;
    private float impactPower;

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0) Explode();
    }

    private void Explode()
    {
        GameObject newFx = ObjectPool.Instance.GetObject(explosionFx);

        ObjectPool.Instance.ReturnObject(newFx, 1);
        ObjectPool.Instance.ReturnObject(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }

    // 设置手雷
    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
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