using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerController scriptPlayer;
    [SerializeField] Animator animPlayer;
    [SerializeField] Transform[] posSpawnMagic;
    [SerializeField] string[] animAttackName;
    [SerializeField] int indexMagic;
    Camera cam;

    [Header("Magic")]
    [SerializeField] GameObject[] attackPrefabs;
    [SerializeField] GameObject[] spearFX;
    [SerializeField] GameObject defenseMagic;

    [Header("AutoTarget")]
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] float radius;
    [Range(0, 360)] [SerializeField] float angle;

    Vector3 direction;
    Vector3 foward;
    [SerializeField] Transform enemy;
    Vector3 camPosFoward;
    Vector3 camPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    #region input
    public void OnAttack()
    {
        if (!scriptPlayer.canMove || scriptPlayer.defend) return;
        animPlayer.Play(animAttackName[indexMagic]);
        TargetFind();
        StartCoroutine(RotateToTarget());
    }

    public void OnDefend(bool value)
    {
        if (scriptPlayer.canMove)
        {
            defenseMagic.SetActive(value);
            animPlayer.SetBool("defend", value);
        }
    }

    public void OnSkillChange()
    {
        indexMagic++;
        if (indexMagic >= attackPrefabs.Length)
            indexMagic = 0;

        ChangeSpearFX();
        UIManager.instance.ChangeSpriteSkill(indexMagic);
    }

    void ChangeSpearFX()
    {
        spearFX[0].SetActive(!spearFX[0].activeInHierarchy);
        spearFX[1].SetActive(!spearFX[1].activeInHierarchy);
    }
    #endregion

    #region magic
    void SpawnInstantaneousMagic()
    {
        if (enemy == null) return;

        GameObject magic = Instantiate(attackPrefabs[indexMagic], posSpawnMagic[indexMagic].position, Quaternion.identity);
        magic.GetComponent<InstantaneousMagic>().SetData(enemy);
    }

    void SpawnProjectileMagic()
    {
        GameObject magic = Instantiate(attackPrefabs[indexMagic], posSpawnMagic[indexMagic].position, Quaternion.identity);
        magic.transform.forward = foward;
    }
    #endregion

    #region rotate
    void TargetFind()
    {
        camPos = cam.transform.position;
        camPos.y = 0;
        camPosFoward = cam.transform.forward;
        camPosFoward.y = 0;
        foward = camPosFoward;
        FieldOfView();
        if (enemy)
        {
            direction = (enemy.position - this.transform.position).normalized;
            direction.y = 0;
            foward = direction;
        }
    }

    void FieldOfView()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - camPos).normalized;

                if (Vector3.Angle(camPosFoward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(camPos, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        enemy = target;
                        break;
                    }
                }
                else
                {
                    enemy = null;
                }
            } 
        }
        else if (enemy)
            enemy = null;
    }
    #endregion

    #region coroutines
    IEnumerator RotateToTarget()
    {
        float duration = 0.3f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 rotation = Vector3.Slerp(transform.forward, foward, t / duration);
            transform.forward = rotation;
            yield return null;
        }
    }

    #endregion
}
