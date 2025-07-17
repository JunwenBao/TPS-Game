using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;

    private Vector3 direction;
    private Transform player;
    private float flySpeed;
    private float rotationSpeed;
    private float timer = 1;

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        rotationSpeed = 1600;

        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }

    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        
        /* 让斧头在规定时间内飞向玩家 */
        timer -= Time.deltaTime;
        if (timer > 0) direction = player.position + Vector3.up - transform.position;

        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if (bullet != null || player != null)
        {
            GameObject newFx = ObjectPool.Instance.GetObject(impactFx, transform);

            ObjectPool.Instance.ReturnObject(gameObject);
            ObjectPool.Instance.ReturnObject(newFx, 1f);
        }
    }
}