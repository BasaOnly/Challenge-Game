using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : Interaction
{
    public override void CanInteraction()
    {
        GameManager.instance.AddQuestItem();
        Destroy(this.gameObject);
    }
}
