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
    [SerializeField] GameObject defenseMagic;
    [SerializeField] LayerMask ignoreLayer;

    [Header("RotationAttack")]
    [SerializeField] float rotationSpeed;

    Vector3 direction;
    Vector3 foward;
    Transform enemy;
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
        camPos = cam.transform.forward;
        camPos.y = 0;
        foward = camPos;

        if (Physics.SphereCast(posSpawnMagic[indexMagic].position, 3f, camPos, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                enemy = hit.transform;
                direction = (enemy.position - this.transform.position).normalized;
                foward = direction;
            }
            else
            {
                enemy = null;
            }
        }
        else { enemy = null; }
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
