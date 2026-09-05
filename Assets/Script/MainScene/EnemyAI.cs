using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Chase,
        ReadyToCharge,
        Charge
    }
    private Rigidbody rb;
    public Transform player;

    public float detectionRange = 10f;
    public float chargeRange = 2f;

    public float moveSpeed = 3f;
    public float chargeSpeed = 6f;

    public float readyToChargeDuration = 3f;
    public float chargeDuration = 1.5f;

    private State state = State.Idle;
    private float stateTime;
    private Vector3 chargeDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        Vector3 playerDir = player.position - transform.position;
        playerDir.y = 0;
        playerDir.Normalize();

        // 상태는 오로지 하나만 가지고 있어야 함
        switch (state)
        {
            case State.Idle:
                if (playerDistance <= detectionRange)
                {
                    state = State.Chase;
                }
                break;

            case State.Chase:
                rb.linearVelocity = playerDir * moveSpeed;
                if (playerDistance <= chargeRange)
                {
                    state = State.ReadyToCharge;
                    stateTime = 0f;
                }
                else if (playerDistance > detectionRange)
                {
                    state = State.Idle;
                    rb.linearVelocity = Vector3.zero;
                }
                break;

            case State.ReadyToCharge:
                rb.linearVelocity = Vector3.zero;
                stateTime += Time.deltaTime;
                if (stateTime >= readyToChargeDuration)
                {
                    // 돌진 방향은 여기서 고정
                    chargeDirection = playerDir;
                    state = State.Charge;
                    stateTime = 0f;
                }
                break;

            case State.Charge:
                rb.linearVelocity = chargeDirection * chargeSpeed;
                stateTime += Time.deltaTime;
                if (stateTime >= chargeDuration)
                {
                    state = State.Idle;
                    rb.linearVelocity = Vector3.zero;
                }
                break;
        }
    }
}
