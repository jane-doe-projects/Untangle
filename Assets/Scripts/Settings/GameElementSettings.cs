using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElementSettings : MonoBehaviour
{
    public MaterialFadeController fadeController;

    public Sprite nodeSprite;
    public List<Material> nodeMaterialList;
    public Material nodeMaterial;
    public Material nodeMaterial2;
    //public Material nodeMaterialRuntime;
    public List<Material> nodeRuntimeMaterials;
    public Material lineMaterial;
    public GameObject lineEffect;

    [Header("Line Materials")]
    public Material crossedLineMaterial;
    public Material uncrossedLineMaterial;

    public Material crossedLineMaterialRuntime;
    public Material uncrossedLineMaterialRuntime;

    public Sprite groupSprite;
    public Sprite levelSprite;

    public float opacityValue = 0.5f;

    [Header("Blur Materials")]
    public Material blurMaterial;
    public Material uiBlurMatieral;
    public Material buttonBlurMaterial;

    [Header("Game Items")]
    public GameObject nodePrefab;
    public GameObject nodeSetPrefab;
    public GameObject nodeParent;

    [Header("Background")]
    public GameObject background;
    public Material backgroundMaterial;
    public Material backgroundMaterialRuntime;

    [Header("Node Preview")]
    public GameObject nodePreviewPrefab;
    public Material linePreviewMaterial;
    public GameObject singleSetPreviewPrefab;

    [Header("Favorites")]
    public List<WordList> wordLists;

    [Header("Sprites")]
    public Sprite playSprite;
    public Sprite pauseSprite;

    public void SetBackground(Material mat)
    {
        ParticleSystem ps = background.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;

        Theme current = GameManager.Instance.currentTheme;
        current.background = mat.GetColor("_Color1");
        current.particleBackground = mat.GetColor("_Color2");

        // set colors
        //psMain.startColor = current.aspectColor2;
        /*Color whiteWithOpacity = Color.white;
        whiteWithOpacity.a = 0.25f;
        psMain.startColor = whiteWithOpacity; */

        Color particleColorWithOpacity = current.particleBackground;
        particleColorWithOpacity.a = 0.25f;
        psMain.startColor = particleColorWithOpacity;
        Camera.main.backgroundColor = current.background;
    }

    public void InitLineMaterials()
    {
        // copy materials so they can be changed during runtime without changing the original material every time
        crossedLineMaterialRuntime = new Material(crossedLineMaterial);
        uncrossedLineMaterialRuntime = new Material(uncrossedLineMaterial);

        fadeController.SetLineMaterials(uncrossedLineMaterialRuntime, crossedLineMaterialRuntime);
    }

    public void InitNodeMaterial()
    {
        //nodeMaterialRuntime = new Material(nodeMaterial);
        //fadeController.SetNodeMaterial(nodeMaterialRuntime);
        nodeRuntimeMaterials = new List<Material>();
        foreach (Material mat in nodeMaterialList)
        {
            Material runtimeMat = new Material(mat);
            nodeRuntimeMaterials.Add(runtimeMat);
        }

        fadeController.SetNodeMaterials(nodeRuntimeMaterials);
    }

    public void InitBackgroundMaterial()
    {
        backgroundMaterialRuntime = new Material(backgroundMaterial);
        backgroundMaterialRuntime.SetColor("_Color1", GameManager.Instance.currentTheme.background);
        backgroundMaterialRuntime.SetColor("_Color2", GameManager.Instance.currentTheme.particleBackground);
    }

    public Material GetNodeRuntimeMaterial()
    {
        int index = Random.Range(0, nodeRuntimeMaterials.Count);
        return nodeRuntimeMaterials[index];
    }

}
