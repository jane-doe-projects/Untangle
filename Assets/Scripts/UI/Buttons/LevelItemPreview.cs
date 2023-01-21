using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelItemPreview : BasePreviewItem, IPointerClickHandler
{
    public Level correspondingLevel;
    public LevelSet correspondingSet;
    public int indexInSet;
    public SessionType type;
    public GroupItemPreview correspondingGroup;
    public bool isEditable = false;
    [SerializeField] Button deleteBtn;
    public Difficulty diff;

    public PreviewHandler previewHandler;


    private void Start()
    {
        deleteBtn.gameObject.SetActive(false);
        deleteBtn.onClick.AddListener(RemoveLevel);
    }

    public override void OnClickAction()
    {
        // load level to screen and close all windows
        GameManager.Instance.currentSession.StartNewLevel(type, correspondingSet, indexInSet);
        GameManager.Instance.soundControl.PlayNew();
        GameManager.Instance.windowControl.CloseAll();
    }

    public void RemoveLevel()
    {
        // TODO add are you sure window

        // remove corresponding level and score from disk
        GameManager.Instance.serManager.RemoveSingleFavorite(correspondingLevel, diff);

        // remove it for corresponding set of display group too
        correspondingGroup.set.levels.Remove(correspondingLevel);

        // update level count on level display
        correspondingGroup.targetDisplay.UpdateLevelProgress(correspondingSet);



        if (!correspondingGroup.targetDisplay.HasMoreDisplayedItems())
            correspondingGroup.targetDisplay.DisplayLevelNotification(true);

        // destroy gameobject
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEditable)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ToggleUtility();
            }
        }
    }

    void ToggleUtility()
    {
        btn.enabled = !btn.enabled;
        deleteBtn.gameObject.SetActive(!deleteBtn.gameObject.activeSelf);
    }

    public static int GetIndexInSet(Level lvl, LevelSet set)
    {
        int index = -1;

        foreach (Level level in set.levels)
        {
            index++;
            if (level == lvl)
                return index;
        }

        return index;
    }

    public void InitPreview()
    {
        previewHandler = GetComponent<PreviewHandler>();
        previewHandler.item = this;
        previewHandler.InitState();
    }
}
