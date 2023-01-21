using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFadeController : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.5f;

    // runtime materials
    //Material nodeMaterial;
    List<Material> nodeMaterials;
    Material crossedMaterial;
    Material uncrossedMaterial;

    string fadeValueNode = "_FadeInValue";
    string fadeValueLine = "_FadeInValue";

    bool fadeIn = false;
    bool fadeInLine = false;

    private void Update()
    {
        if (fadeIn)
            FadeNodeMaterial();
        if (fadeInLine)
            FadeLineMaterials();
    }

    void FadeNodeMaterial()
    {
        /*
        float val = nodeMaterial.GetFloat(fadeValueNode);
        if (val >= 1)
            fadeIn = false;
        val += Time.deltaTime * fadeSpeed;
        nodeMaterial.SetFloat(fadeValueNode, val); */


        foreach (Material mat in nodeMaterials)
        {
            float val = mat.GetFloat(fadeValueNode);
            if (val >= 1)
                fadeIn = false;
            val += Time.deltaTime * fadeSpeed;
            mat.SetFloat(fadeValueNode, val);
        }
    }

    void FadeLineMaterials()
    {
        float val = crossedMaterial.GetFloat(fadeValueLine);
        if (val >= 1)
            fadeInLine = false;
        val += Time.deltaTime * fadeSpeed / 2;
        uncrossedMaterial.SetFloat(fadeValueLine, val);
        crossedMaterial.SetFloat(fadeValueLine, val);

    }

    public void HideMaterials()
    {
        //nodeMaterial.SetFloat(fadeValueNode, -1);

        foreach (Material mat in nodeMaterials)
            mat.SetFloat(fadeValueNode, -1);

        crossedMaterial.SetFloat(fadeValueLine, -2);
        uncrossedMaterial.SetFloat(fadeValueLine, -2);

    }

    public void FadeInMaterials()
    {
        if (1 < nodeMaterials[0].GetFloat(fadeValueNode))
            return;
        HideMaterials();
        fadeIn = true;
        fadeInLine = true;
    }

    public void SetLineMaterials(Material uncrossed, Material crossed)
    {
        uncrossedMaterial = uncrossed;
        crossedMaterial = crossed;
    }

    /*
    public void SetNodeMaterial(Material node)
    {
        nodeMaterial = node;
    } */

    public void SetNodeMaterials(List<Material> list)
    {
        nodeMaterials = list;
    }
}
