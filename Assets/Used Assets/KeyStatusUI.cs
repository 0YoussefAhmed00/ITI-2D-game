using UnityEngine;
using UnityEngine.UI;

public class KeyStatusUI : MonoBehaviour
{
    public Image keyImage;
    public Sprite normalKeySprite;
    public Sprite blueKeySprite;

    public void ShowNoKey()
    {
        keyImage.enabled = false;
    }

    public void ShowNormalKey()
    {
        keyImage.enabled = true;
        keyImage.sprite = normalKeySprite;
    }

    public void ShowBlueKey()
    {
        keyImage.enabled = true;
        keyImage.sprite = blueKeySprite;
    }
}