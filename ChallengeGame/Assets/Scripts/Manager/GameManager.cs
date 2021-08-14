using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Quests")]
    public int itensQuest;
    public bool questCompleted;
    [SerializeField] Animator[] animDoor;
    [SerializeField] int numberOfEnemys;
    public int enemyDefeat;

    [Header("Player")]
    [SerializeField] Animator animPlayer;
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

    public void EnemyDefeat()
    {
        enemyDefeat++;

        if(enemyDefeat == numberOfEnemys)
        {
            stopActionsPlayer = true;
            animPlayer.Play("Victory");
            UIManager.instance.textFinalQuest.color = Color.green;
            StartCoroutine(WaitToOpenMenu());
        }
    }

    IEnumerator WaitToOpenMenu()
    {
        yield return new WaitForSeconds(4);
        UIManager.instance.AjustMenuDeath();
        UIManager.instance.OpenCloseMenu();
    }

    #region button
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
