using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField] private GameObject bulletImpactFX;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    private LayerMask allyLayerMask;

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisabledBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    // 设置子弹属性
    public void BulletSetup(LayerMask allyLayerMask,float flyDistance = 100, float impactForce = 100)
    {
        this.impactForce = impactForce;
        this.allyLayerMask = allyLayerMask;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.Clear();
        trailRenderer.time = 0.25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + 0.5f;
    }

    // 将子弹返回到对象池
    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0) ReturnBulletToPool();
    }

    // 子弹消失
    protected void DisabledBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    // 子弹轨迹
    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    // 子弹撞击
    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"碰撞到：{collision.gameObject.name}");
        /* 检测友伤 */
        if(FriendlyFire() == false)
        {
            if((allyLayerMask.value & (1 << collision.gameObject.layer)) > 0)
            {
                ReturnBulletToPool(10);
                return;
            }
        }

        CreateImpactFX();
        ReturnBulletToPool();

        /* 如果子弹碰撞的对象有接口IDamagable，则造成伤害 */
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        damagable?.TakeDamage();

        ApplyBulletImpactToEnemy(collision);
    }

    private void ApplyBulletImpactToEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;

            enemy.BulletImpact(force, collision.contacts[0].point, hitRigidbody);
        }
    }

    protected void ReturnBulletToPool(float delay = 0) => ObjectPool.Instance.ReturnObject(gameObject, delay);

    // 制造子弹碰撞特效
    protected void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX, transform);
        ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
    }

    private bool FriendlyFire() => GameManager.Instance.firendlyFire;
}