using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Image imgSkill;
    [SerializeField] Sprite[] spritesSkill;

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

}
