public class Player_Health : HealthController
{
    private Player player;

    //TODO：当角色死了，检查这个变量，让角色+相机不再移动
    public bool isDead {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override void ReduceHealth()
    {
        base.ReduceHealth();

        if (ShouldDie()) Die();
    }

    private void Die()
    {
        isDead = true;
        player.animator.enabled = false;
        player.ragdoll.RagdollActive(true);
    }
}