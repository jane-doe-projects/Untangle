using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBody : MonoBehaviour
{
    [SerializeField] GameObject nodeIcon;
    [SerializeField] public GameObject nodeEffect;
    [SerializeField] public GameObject nodeSelected;
    [SerializeField] public GameObject nodeBodyEffect;

    [SerializeField] GameObject solvedEffect;

    [Header("3D Spehere")]
    [SerializeField] GameObject sphere;

    public Node parent;

    void Start()
    {
        nodeEffect.SetActive(false);
        nodeSelected.SetActive(false);
        SetIcon();
    }


    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && (parent.clickableInSwap || !parent.swapping))
            parent.SelectNode();
        /*
        if (Input.GetMouseButtonDown(0) && !parent.swapping)
            parent.SelectNode();
        if (!parent.swapping)
            nodeEffect.SetActive(true) */
    }

    private void OnMouseEnter()
    {
        
        if (!parent.swapping || parent.clickableInSwap)
            nodeEffect.SetActive(true);
        /*if (!parent.swapping)
            nodeEffect.SetActive(true); */
    }

    private void OnMouseExit()
    {
        nodeEffect.SetActive(false);
    }

    public void SetIcon()
    {
        SpriteRenderer sr = nodeIcon.GetComponent<SpriteRenderer>();
        sr.sprite = GameManager.Instance.gameElements.nodeSprite;
        Material nodeMat = GameManager.Instance.gameElements.GetNodeRuntimeMaterial();
        if (nodeMat != null)
        {
            sr.material = nodeMat;
            sphere.GetComponent<Renderer>().material = nodeMat;
        }

        sr.color = GameManager.Instance.currentTheme.nodeColor1;

        // turn by random angle for randomized node effects/orientation
        Quaternion rot = sr.transform.rotation;
        rot.z = Random.Range(0, 180);
        sr.transform.rotation = rot;

        // do the same for the sphere
        Quaternion rotSphere = sphere.transform.rotation;
        //rotSphere = Random.rotation;

        int random = Random.Range(0, 3);

        if (random > 1)
        {
            rotSphere.x = 0;
            rotSphere.z = 0;
            rotSphere.y = Mathf.Clamp(Random.rotation.y, 0, 180);
        } else
        {
            rotSphere.x = 0;
            rotSphere.z = Random.rotation.z;
            rotSphere.y = 0;
        }
        sphere.transform.rotation = rotSphere;

        // z no clamp
        // x no rotation
        // y btw 0 and 180

    }

    public void SetEffectColors()
    {
        ParticleSystem ps = nodeBodyEffect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        
        var col = ps.colorOverLifetime;
        col.enabled = true;
    }

    public void Solved()
    {
        // deactivate collider so node is not swapable anylonger
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;

        // show solved visual
        solvedEffect = Instantiate(GameManager.Instance.effectsManager.nodeSolved, solvedEffect.transform);
        SkinUpdater updater = solvedEffect.AddComponent<SkinUpdater>();
        updater.IsParticleSystem(true);
        ParticleSystem ps = solvedEffect.GetComponent<ParticleSystem>();
        ps.Play();
    }
}
