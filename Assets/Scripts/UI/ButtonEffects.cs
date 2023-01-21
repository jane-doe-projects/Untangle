using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button btn;
    GameObject effect;
    [SerializeField] bool maskable;

    public bool isSelected = false;
    void Start()
    {
        if (effect == null)
            InitEffect();
    }

    void OnClickEffect()
    {
        // do nothing for now
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        effect.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            effect.SetActive(false);
    }

    float GetScale()
    {
        float scale = 0;
        GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup != null)
            scale = gridLayoutGroup.cellSize.x / 4 / 100;
        else
        {
            RectTransform rt = GetComponent<RectTransform>();
            float width = rt.rect.width;
            scale = width / 4 / 100;
        }
        return scale;
    }

    void InitEffect()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickEffect);

        // add particle hover effect

        if (maskable)
            effect = Instantiate(GameManager.Instance.effectsManager.hoverVortexMaskable, transform);
        else
            effect = Instantiate(GameManager.Instance.effectsManager.hoverVortex, transform);

        GridLayoutGroup gridLayoutGroup = transform.parent.GetComponent<GridLayoutGroup>();

        float scale = GetScale();
        effect.transform.localScale = new Vector3(scale, scale, scale);
        effect.SetActive(false);
    }


    public void ShowAsActive()
    {
        effect.SetActive(true);
        isSelected = true;
    }

    public void DisableActive()
    {
        effect.SetActive(false);
        isSelected = false;
    }
}



