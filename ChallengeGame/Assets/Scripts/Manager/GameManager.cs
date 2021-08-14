using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("QUests")]
    public int itensQuest;

    [Header("Player")]
    public bool stopActionsPlayer;

    [Header("Interaction")]
    public Interaction interaction;

    private void Awake() => Instance();

    void Instance()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void AddQuestItem()
    {
        itensQuest++;
       
        if(itensQuest > 4)
        {
            //OpenDoor
        }
    }
}
