using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] Animator animEnemy;
    [HideInInspector] public HealthPlayer playerScriptHealth;
    [SerializeField] HealthEnemy enemyScriptHealth;

    [Header("FieldOfView")]
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    public bool canSeePlayer;

    [Header("Patrol")]
    [SerializeField] PatrolControl scriptPatrol;
    [SerializeField] float rotateChaseSpeed;
    [SerializeField] int indexEnemy;

    [Header("Fight")]
    [SerializeField] float timeToCast;
    [SerializeField] float weaponRange;
    [SerializeField] GameObject[] attackPrefabs;
    [SerializeField] Transform[] posSpawnMagic;
    [SerializeField] string[] animName;

    Vector3 directionToTarget;
    Vector3 lastPointPlayer;
    Vector3 previousPosition;

    float remainingDistance;
    float curSpeed;
    bool getPoint;
    bool canPatrol;
    bool win;

    Transform point;

    private void Awake()
    {
        playerScriptHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthPlayer>();
    }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
        canPatrol = true;
        GetPoint();
    }

    void Update()
    {
        if (!win) CheckPlayerAlive();
        else return;

        if (!enemyScriptHealth.die)
        {
            LookTarget();
            CurrentSpeed();
            Behaviour();
        }
        else if (navMesh.enabled)
        {
            StopAllCoroutines();
            navMesh.enabled = false;
            StartCoroutine(Disappear());
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
        if (enemyScriptHealth.wasAttacked) FindAndChase();
        else if (!canSeePlayer && canPatrol) Patrol();
        else ChasePlayer();
    }

    void LookTarget()
    {
        if (!canSeePlayer && !enemyScriptHealth.wasAttacked) return;

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
            navMesh.SetDestination(lastPointPlayer);
            navMesh.stoppingDistance = 0;
        }
        else
        {
            navMesh.speed = 4;
            canPatrol = true;
            enemyScriptHealth.wasAttacked = false;
        }
    }
    
    void FindAndChase()
    {
        navMesh.speed = 6;
        navMesh.stoppingDistance = weaponRange;
        navMesh.SetDestination(playerScriptHealth.transform.position);
    }

    void CheckPlayerAlive()
    {
        if (playerScriptHealth.die && navMesh.enabled)
        {
            StopAllCoroutines();
            navMesh.isStopped = true;
            animEnemy.SetTrigger("win");
            win = true;
        }
    }

    #endregion

    #region patrol
    void Patrol()
    {
        if (remainingDistance != Mathf.Infinity && navMesh.pathStatus == NavMeshPathStatus.PathComplete && remainingDistance == 0 && !getPoint)
        {
            getPoint = true;
            StartCoroutine(WaitToMove());
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
                lastPointPlayer = playerScriptHealth.transform.position;

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
            StartCoroutine(CastSkill());
    }

    IEnumerator CastSkill()
    {
        navMesh.isStopped = true;
        yield return new WaitForSeconds(timeToCast);
        int index = Random.Range(0, attackPrefabs.Length);
        animEnemy.Play(animName[index]);
        yield return new WaitForSeconds(1f);
        navMesh.isStopped = false;
    }

    void Magic01()
    {
        GameObject magic = Instantiate(attackPrefabs[0], posSpawnMagic[0].position, Quaternion.identity);
        magic.transform.forward = transform.forward;
    }

    void Magic02()
    {
        GameObject magic = Instantiate(attackPrefabs[1], posSpawnMagic[0].position, Quaternion.identity);
        magic.GetComponent<InstantaneousMagic>().SetData(playerScriptHealth);
    }
    #endregion

    #region visual
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3);
        float timeToDisappear = 0f;
        while (timeToDisappear < 4)
        {
            timeToDisappear += Time.deltaTime/2;
            float lerpValue = timeToDisappear;
            Vector3 pos = transform.position;
            pos.y -= lerpValue;
            transform.position = pos;
            yield return null;
        }

        Destroy(this.gameObject);
    }
    #endregion
}
