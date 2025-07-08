using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField] private GameObject bulletImpactFX;

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    private void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void BuletSetup(float flyDistance, float impactForce)
    {
        this.impactForce = impactForce;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = 0.25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + 0.5f;
    }

    private void Update()
    {
        FadeTrailIfNeeded();
        DisabledBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    private void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0) ReturnBulletToPool();
    }

    private void DisabledBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    private void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        EnemyShield shield = collision.gameObject.GetComponent<EnemyShield>();

        if(shield != null)
        {
            shield.ReduceDurability();
            return;
        }

        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;

            enemy.GetHit();
            enemy.HitImpact(force, collision.contacts[0].point, hitRigidbody);
        }

        CreateImpactFX(collision);
        ReturnBulletToPool();
    }

    private void ReturnBulletToPool() => ObjectPool.Instance.ReturnObject(gameObject);

    // 制造子弹碰撞特效
    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX);
            newImpactFX.transform.position = contact.point;

            ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
        }
    }
}