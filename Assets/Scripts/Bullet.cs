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

    // �����ӵ�����
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

    // ���ӵ����ص������
    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0) ReturnBulletToPool();
    }

    // �ӵ���ʧ
    protected void DisabledBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    // �ӵ��켣
    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    // �ӵ�ײ��
    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"��ײ����{collision.gameObject.name}");
        /* ������� */
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

        /* ����ӵ���ײ�Ķ����нӿ�IDamagable��������˺� */
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

    // �����ӵ���ײ��Ч
    protected void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX, transform);
        ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
    }

    private bool FriendlyFire() => GameManager.Instance.firendlyFire;
}