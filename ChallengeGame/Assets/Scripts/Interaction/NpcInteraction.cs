using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcInteraction : Interaction
{
    [SerializeField] GameObject panel;
    public override void CanInteraction()
    {
        GameManager.instance.stopActionsPlayer = !GameManager.instance.stopActionsPlayer;
        panel.SetActive(!panel.activeInHierarchy);
        base.CanInteraction();
    }
}
