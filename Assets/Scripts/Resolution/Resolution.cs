using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resolution : MonoBehaviour
{
    [SerializeField] ResolutionMenu type;
    [SerializeField] TMP_Dropdown dropDown;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] string labelText;

    ResolutionHandler resHandler;

    void Start()
    {
        label.text = labelText;
        resHandler = GameManager.Instance.resHandler;

        dropDown.onValueChanged.AddListener(delegate { PlaySound(); });

        if (type == ResolutionMenu.Mode)
            dropDown.onValueChanged.AddListener(delegate { resHandler.SetResolutionMode(dropDown.value); });
        else
            dropDown.onValueChanged.AddListener(delegate { resHandler.SetResolutionSize(dropDown.value, dropDown.options); });
    }

    void PopulateWithAvailableSizes()
    {
        if (resHandler == null)
            resHandler = GameManager.Instance.resHandler;
        dropDown.ClearOptions();
        List<string> resolutionOptions = resHandler.GetAvailableResolutions();
        dropDown.AddOptions(resolutionOptions);
    }

    public void InitializeLastSelected(int index)
    {
        if (type == ResolutionMenu.Size)
            PopulateWithAvailableSizes();

        if (index == -1 && type == ResolutionMenu.Size)
        {
            // detect resolution and pick respective options value
            dropDown.value = GetOptionsIndex(Screen.width, Screen.height);
        } else
            dropDown.value = index;
    }

    int GetOptionsIndex(int width, int height)
    {
        int count = 0;
        foreach (TMP_Dropdown.OptionData opt in dropDown.options)
        {
            Vector2 resolution = ResolutionHandler.ParseResolution(opt.text);
            if (width == resolution.x && height == resolution.y)
                return count;
            count++;
        }

        return 0;
    }

    void PlaySound()
    {
        GameManager.Instance.soundControl.uiSounds.Click();
    }

    public TMP_Dropdown GetDropDown()
    {
        return dropDown;
    }

    private void OnEnable()
    {
        if (type == ResolutionMenu.Size)
        {
            int tempSize = dropDown.value;
            GameManager.Instance.resHandler.currentSize = tempSize;
            GameManager.Instance.resHandler.lastSize = tempSize;
        } else if (type == ResolutionMenu.Mode)
        {
            int tempMode = dropDown.value;
            GameManager.Instance.resHandler.currentMode = tempMode;
            GameManager.Instance.resHandler.lastMode = tempMode;
        }
    }

}

public enum ResolutionMenu
{
    Mode,
    Size
}
