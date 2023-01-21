using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour
{
    Button btn;
    TextMeshProUGUI btnText;
    [SerializeField] bool showText = false;

    [SerializeField] string buttonText;
    [SerializeField] Sprite sprite;
    [SerializeField] bool isMainMenuButton;
    [SerializeField] bool noImage = false;
    [SerializeField] Image buttonImg;
    [SerializeField] Image fullImage;

    Image img;

    Sprite pauseSprite;
    Sprite playSprite;

    void Start()
    {
        pauseSprite = GameManager.Instance.gameElements.pauseSprite;
        playSprite = GameManager.Instance.gameElements.playSprite;

        DelegateHandler.onBackgroundChangeDelegate += SetAspectColor;

        btn = GetComponent<Button>();
        btnText = btn.GetComponentInChildren<TextMeshProUGUI>();

        if (showText)
            btnText.text = buttonText;
        else
            btnText.enabled = false;
        
        img = GetComponent<Image>();
        SetSprite();
        
    }

    void SetSprite()
    {
        if (noImage)
            return;
        if (isMainMenuButton)
            buttonImg.sprite = sprite;
        else
        {
            fullImage.gameObject.SetActive(true);
            fullImage.sprite = sprite;
        }

        //SetAspectColor();
    }

    public void SetCustomSprite(CustomSprite sprite)
    {
        if (!noImage && !isMainMenuButton)
        {
            switch (sprite)
            {
                case CustomSprite.Pause:
                    fullImage.sprite = pauseSprite;
                    break;
                case CustomSprite.Play:
                    fullImage.sprite = playSprite;
                    break;
                default:
                    break;
            }
        }
    }

    void SetAspectColor()
    {
        if (!img)
            img = GetComponent<Image>();
        img.color = GameManager.Instance.currentTheme.nodeColor1;
    }

    private void OnDestroy()
    {
        DelegateHandler.onBackgroundChangeDelegate -= SetAspectColor;
    }

}

public enum CustomSprite
{
    Pause,
    Play
}
