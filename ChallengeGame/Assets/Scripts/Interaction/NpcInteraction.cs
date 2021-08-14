using System.Collections;
using UnityEngine;


public class NpcInteraction : Interaction
{
    [SerializeField] GameObject panel;
    [SerializeField] Animator animNpc;
    [SerializeField] Collider triggerCol;
    bool cantInteractable;
    public override void CanInteraction()
    {
        if (cantInteractable) return;

        if (GameManager.instance.questCompleted)
        {
            OpenClosePanel();
            StartCoroutine(QuestCompleted());
        }
        else
        {
            OpenClosePanel();
        }
    }

    IEnumerator QuestCompleted()
    {
        cantInteractable = true;
        triggerCol.enabled = false;
        UIManager.instance.keyText.enabled = false;
        yield return new WaitForSeconds(2);
        animNpc.Play("Npc_ForgeMagic");
        yield return new WaitForSeconds(4);
        UIManager.instance.UnlockMagics();
        yield return new WaitForSeconds(4);
        OpenClosePanel();
    }

    void OpenClosePanel()
    {
        GameManager.instance.stopActionsPlayer = !GameManager.instance.stopActionsPlayer;
        panel.SetActive(!panel.activeInHierarchy);
    }
}
