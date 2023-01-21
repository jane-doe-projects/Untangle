using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] GameObject mouseEffectParent;
    GameObject effect;
    ParticleSystem effectPs;

    [SerializeField] Texture2D cursorTexture;
    [SerializeField] Vector2 hotspot;

    private void Awake()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
            ResetCursor();
        ResolutionHandler.resolutionChangeDelegate += ResetCursor;
    }

    private void Start()
    {
        // add mouse effect to mouse
        effect = Instantiate(GameManager.Instance.effectsManager.mouseEffect, mouseEffectParent.transform);
        effectPs = effect.GetComponent<ParticleSystem>();
        effectPs.Play();
    }
    // Update is called once per frame
    void Update()
    {
        mouseEffectParent.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void ResetCursor()
    {
        CursorMode mode = CursorMode.Auto;
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
            mode = CursorMode.ForceSoftware;
        Cursor.SetCursor(cursorTexture, hotspot, mode);
    }

    private void OnDestroy()
    {
        ResolutionHandler.resolutionChangeDelegate -= ResetCursor;
    }
}
