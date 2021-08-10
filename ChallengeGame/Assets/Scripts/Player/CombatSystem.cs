using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerController scriptPlayer;
    [SerializeField] Animator animPlayer;
    [SerializeField] Transform posSpawnMagic;

    [Header("Magic")]
    [SerializeField] GameObject[] magicPrefabs;

    public void OnAttack()
    {
        animPlayer.SetTrigger("attack");
    }

    void SpawnMagic()
    {
        GameObject magic = Instantiate(magicPrefabs[0], posSpawnMagic.position, Quaternion.identity);
        magic.transform.forward = this.transform.forward;
    }
}
