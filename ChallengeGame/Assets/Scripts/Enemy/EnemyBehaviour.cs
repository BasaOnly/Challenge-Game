using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] Animator animEnemy;
    [HideInInspector] public Transform playerRef;
    [SerializeField] Health scriptHealth;

    [Header("FieldOfView")]
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [HideInInspector] public bool canSeePlayer;

    [Header("Patrol")]
    [SerializeField] PatrolControl scriptPatrol;
    [SerializeField] float rotateChaseSpeed;
    [SerializeField] int indexEnemy;

    [Header("Fight")]
    [SerializeField] float timeToCast;
    [SerializeField] float weaponRange;

    Vector3 directionToTarget;
    Vector3 lastPointPlayer;
    Vector3 previousPosition;

    float remainingDistance;
    float curSpeed;
    bool getPoint;
    bool canPatrol;

    Coroutine waitRotine, viewRotine, castSkill;
    Transform point;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        viewRotine = StartCoroutine(FOVRoutine());
        canPatrol = true;
        GetPoint();
    }

    void Update()
    {
        if (!scriptHealth.die)
        {
            LookTarget();
            CurrentSpeed();
            Behaviour();
        }
        else if (navMesh.enabled)
        {
            StopCoroutine(waitRotine);
            StopCoroutine(viewRotine);
            StopCoroutine(castSkill);
            navMesh.enabled = false;
        }
    }

    #region animator
    void CurrentSpeed()
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        animEnemy.SetFloat("speed", curSpeed);
        previousPosition = transform.position;
    }

    #endregion

    #region behaviour
    void Behaviour()
    {
        if (navMesh.isStopped) return;
        remainingDistance = navMesh.remainingDistance;
        Attack();
        if (!canSeePlayer && canPatrol) Patrol();
        else ChasePlayer();
    }

    void LookTarget()
    {
        if (!canSeePlayer) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateChaseSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        if (canSeePlayer)
        {
            canPatrol = false;
            navMesh.speed = 6;
            navMesh.stoppingDistance = weaponRange;
            navMesh.SetDestination(lastPointPlayer);
        }
        else if (!canSeePlayer && remainingDistance > 0)
        {
            navMesh.stoppingDistance = 0;
        }
        else
        {
            navMesh.speed = 4;
            canPatrol = true;
        }

    }

    #endregion

    #region patrol
    void Patrol()
    {
        if (remainingDistance != Mathf.Infinity && navMesh.pathStatus == NavMeshPathStatus.PathComplete && remainingDistance == 0 && !getPoint)
        {
            getPoint = true;
            waitRotine = StartCoroutine(WaitToMove());
        }
    }

    IEnumerator WaitToMove()
    {
        float randomTime = Random.Range(4, 6);
        yield return new WaitForSeconds(randomTime);
        GetPoint();
    }

    void GetPoint()
    {
        getPoint = false;
        point = scriptPatrol.GetPoint(indexEnemy);
        navMesh.SetDestination(point.position);
    }
    #endregion

    #region fieldOfView
    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            if (canSeePlayer)
                lastPointPlayer = playerRef.transform.position;

            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                canSeePlayer = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    #endregion

    #region combat

    void Attack()
    {
        if (canSeePlayer && curSpeed < 0.15f)
            castSkill = StartCoroutine(CastSkill());
    }

    IEnumerator CastSkill()
    {
        navMesh.isStopped = true;
        yield return new WaitForSeconds(timeToCast);
        animEnemy.Play("attack01");
        yield return new WaitForSeconds(1f);
        navMesh.isStopped = false;
    }
    #endregion
}
