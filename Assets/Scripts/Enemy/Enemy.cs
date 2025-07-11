using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 20;

    [Header("Idle Data")]
    public float idleTime;
    public float aggresionRange;

    [Header("Move Data")]
    public float walkSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    private bool manualMovement;
    private bool manualRotation;

    [SerializeField] protected Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition;
    private int currentPatrolIndex;

    public bool inBattleMode {  get; private set; }

    public Transform player {  get; private set; }

    public Animator animator {  get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine {  get; private set; }
    public Enemy_Visuals visuals { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        visuals = GetComponent<Enemy_Visuals>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    protected virtual void Update()
    {
        /* Battle State */
        if (ShouldEnterBattleMode()) EnterBattleMode();
    }

    // 判断是否要进入Battle State
    protected bool ShouldEnterBattleMode()
    {
        /* 根据玩家距离判断 */
        bool inAggressionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;

        if (inAggressionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    public bool PlayerInAggresionRange() => Vector3.Distance(transform.position, player.position) < aggresionRange;

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngels = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
    }

    // 被击中
    public virtual void GetHit()
    {
        EnterBattleMode();

        healthPoints--;
    }

    public virtual void DeathImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        StartCoroutine(DeathImpactCourutine(force, hitPoint, rb));
    }

    private IEnumerator DeathImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);

        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }

    public void ActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public void ActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualMovementActive() => manualMovement;
    public bool ManualRotationActive() => manualRotation;
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }
}