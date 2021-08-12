using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public Transform playerRef;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] Enemy enemyScript;


    [Header("FieldOfView")]    
    public float radius;
    [Range(0,360)] public float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [HideInInspector] public bool canSeePlayer;
  
    [Header("Patrol")]
    [SerializeField] PatrolControl scriptPatrol;
    [SerializeField] float rotateChaseSpeed;
    [SerializeField] int indexEnemy;

    Vector3 directionToTarget;
    Vector3 lastPointPlayer;
    float remainingDistance;
    bool getPoint;
    bool canPatrol;
    Coroutine waitRotine, viewRotine;
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
        if (!enemyScript.die)
        {
            Behaviour();
        }
        else if(navMesh.enabled)
        {
            StopCoroutine(waitRotine);
            StopCoroutine(viewRotine);
            navMesh.enabled = false;
        }
    }

    #region behaviour
    void Behaviour()
    {
        remainingDistance = navMesh.remainingDistance;
        if (!canSeePlayer && canPatrol) Patrol();
        else ChasePlayer();
    }
    #endregion

    #region patrol
    void Patrol()
    {
        PathComplete();
    }

    void PathComplete()
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
        
        while(true)
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

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }

            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    void ChasePlayer()
    {
        if (canSeePlayer)
        {
            canPatrol = false;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateChaseSpeed * Time.deltaTime);
            navMesh.stoppingDistance = 15;
            navMesh.SetDestination(lastPointPlayer);
        }
        else if (!canSeePlayer && remainingDistance > 0)
        {
            navMesh.stoppingDistance = 0;
            navMesh.SetDestination(lastPointPlayer);
        }
        else
        {
            canPatrol = true;
        }
    }


    #endregion

}
