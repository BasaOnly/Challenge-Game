using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Quests")]
    public int itensQuest;
    public bool questCompleted;
    [SerializeField] Animator[] animDoor;

    [Header("Player")]
    public bool stopActionsPlayer;
    public bool unlockMagic;

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
        UIManager.instance.itensQuest.text = string.Format("{0}/4", itensQuest);
        UIManager.instance.keyText.enabled = false;

        switch (itensQuest)
        {
            case 1:
                OpenDoor(0);
                break;
            case 3:
                OpenDoor(1);
                break;
            case 4:
                QuestCompleted();
                OpenDoor(2);
                break;
        }
    }

    void OpenDoor(int indexDoor)
    {
        animDoor[indexDoor].Play("OpenDoor");
    }

    void QuestCompleted()
    {
        UIManager.instance.itensQuest.color = Color.green;
        questCompleted = true;
    }
}
