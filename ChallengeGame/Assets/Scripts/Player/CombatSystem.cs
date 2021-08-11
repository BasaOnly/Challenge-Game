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

    [Header("Magic")]
    [SerializeField] GameObject[] magicPrefabs;

    public void OnAttack()
    {
        animPlayer.Play(animAttackName[indexMagic]);
    }

    public void OnSkillChange()
    {
        if (!animPlayer.GetBool("canMove")) return;

        indexMagic++;

        if (indexMagic >= magicPrefabs.Length)
            indexMagic = 0;
    }

    void SpawnMagic()
    {
        GameObject magic = Instantiate(magicPrefabs[indexMagic], posSpawnMagic[indexMagic].position, Quaternion.identity);
        magic.transform.forward = this.transform.forward;
    }
}
