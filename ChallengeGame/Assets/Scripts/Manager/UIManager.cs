using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Components Player")]
    [SerializeField] GameObject containerSkill;
    [SerializeField] Image imgSkill;
    [SerializeField] Sprite[] spritesSkill;
    [SerializeField] Image HPFill;

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

    public void ChangeSpriteSkill(int index)
    {
        imgSkill.sprite = spritesSkill[index];
    }

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

    public void UnlockMagics()
    {
        titleText.text = string.Format("You unlocked two spells, press {0} to switch between them!", switchMagic);
        itensQuest.text = "";
        imgItemQuest.enabled = false;
        panelImgSkills.SetActive(true);
        containerSkill.SetActive(true);
        GameManager.instance.unlockMagic = true;
    }

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
}
