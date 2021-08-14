using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Components")]
    [SerializeField] Image imgSkill;
    [SerializeField] Sprite[] spritesSkill;
    public Text keyText;

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
        if (string.Equals("LS", keyName)) keyText.text = string.Format("Press [ButtonNorth]");
        else keyText.text = string.Format("Press [E]");
    }

}
