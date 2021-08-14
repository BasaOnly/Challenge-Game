using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Components Player")]
    [SerializeField] GameObject containerSkill;
    [SerializeField] Image imgSkill;
    [SerializeField] Sprite[] spritesSkill;
    [SerializeField] Image HPFill;

    [Header("Menu")]
    [SerializeField] GameObject menu;
    [SerializeField] Button btnResume;
    [SerializeField] Button btnRestart;

    [Header("Canvas World")]
    [SerializeField] Canvas canvasWorld;
    [SerializeField] GameObject prefabHPEnemy;
    [SerializeField] List<Image> listFillHPEnemy = new List<Image>();

    [Header("Quest Final")]
    [SerializeField] GameObject panelQuestFinal;
    public Text textFinalQuest;

    [Header("Components NPC")]
    [SerializeField] Text titleText;
    [SerializeField] Image imgItemQuest;
    [SerializeField] GameObject panelImgSkills;
    public Text itensQuest;
    public Text keyText;
    string switchMagic;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #region input

    public void ChangeNameKey(string keyName)
    {
        if (string.Equals("LS", keyName)) 
        { 
            keyText.text = string.Format("Press [ButtonNorth]");
            switchMagic = string.Format("[ButtonEast]");
        }
        else 
        { 
            keyText.text = string.Format("Press [E]");
            switchMagic = string.Format("[Q]");
        }
    }

    public void OpenCloseMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
        GameManager.instance.stopActionsPlayer = menu.activeInHierarchy;
    }

    public void AjustMenuDeath()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnRestart.gameObject);
        btnResume.gameObject.SetActive(false);
        btnRestart.gameObject.SetActive(true);
    }
    #endregion

    #region magic
    public void ChangeSpriteSkill(int index)
    {
        imgSkill.sprite = spritesSkill[index];
    }


    public void UnlockMagics()
    {
        titleText.text = string.Format("You unlocked two spells, press {0} to switch between them!", switchMagic);
        itensQuest.text = "";
        imgItemQuest.enabled = false;
        panelImgSkills.SetActive(true);
        containerSkill.SetActive(true);
        panelQuestFinal.SetActive(true);
        GameManager.instance.unlockMagic = true;
    }
    #endregion

    #region playerHP
    public void SetValueHP(float life)
    {
        StartCoroutine(LerpValueHP(life));
    }

    IEnumerator LerpValueHP(float life)
    {
        float currentHP = HPFill.fillAmount;
        if (life < currentHP)
        {
            while (currentHP > life)
            {
                currentHP -= Time.deltaTime;
                HPFill.fillAmount = currentHP;
                yield return null;
            }
        }
        else
        {
            while (currentHP < life)
            {
                currentHP += Time.deltaTime;
                HPFill.fillAmount = currentHP;
                yield return null;
            }
        }
    }
    #endregion

    #region enemyHP
    public int InstantiateLifeBar()
    {
        GameObject bar = Instantiate(prefabHPEnemy, canvasWorld.transform);
        listFillHPEnemy.Add(bar.transform.GetChild(0).GetComponent<Image>());
        int indexBar = listFillHPEnemy.Count - 1;
        return indexBar;
    }

    public GameObject GetBar(int indexBar)
    {
        return listFillHPEnemy[indexBar].transform.gameObject;
    }

    public void SetHPEnemy(float life, int indexLerp)
    {
        StartCoroutine(LerpHPEnemy(life, indexLerp));
    }

    IEnumerator LerpHPEnemy(float life, int indexLerp)
    {
        float currentHP = listFillHPEnemy[indexLerp].fillAmount;
        while (currentHP > life)
        {
            currentHP -= Time.deltaTime;
            listFillHPEnemy[indexLerp].fillAmount = currentHP;
            yield return null;
        }
    }
    #endregion
}
