public class Player_Health : HealthController
{
    private Player player;

    //TODO������ɫ���ˣ��������������ý�ɫ+��������ƶ�
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