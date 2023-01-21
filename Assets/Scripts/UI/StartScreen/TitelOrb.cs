using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitelOrb : MonoBehaviour
{
    MeshRenderer mr;

    [SerializeField] GameObject hoverEffect;
    [SerializeField] GameObject selectionEffect;

    [SerializeField] ParticleSystem hoverPs;
    [SerializeField] ParticleSystem selectPs;

    bool swapping = false;
    Vector3 destination;

    private void Awake()
    {
        GameManager.onMaterialsInitializedDelegate += SetMaterial;
        hoverEffect.SetActive(false);
        selectionEffect.SetActive(false);
    }

    private void Start()
    {
        //GameManager.onMaterialsInitializedDelegate += SetMaterial;
        mr = GetComponent<MeshRenderer>();
        SetRandomRotation();
    }

    private void Update()
    {
        if (swapping)
            LerpToDestination();
    }

    void SetMaterial()
    {
        int rndIndex = Random.Range(0, 2);
        if (mr == null)
            mr = GetComponent<MeshRenderer>();
        mr.material = GameManager.Instance.gameElements.nodeRuntimeMaterials[rndIndex];
    }

    void SetRandomRotation()
    {
        Quaternion rotSphere = transform.rotation;

        int random = Random.Range(0, 3);

        if (random > 1)
        {
            rotSphere.x = 0;
            rotSphere.z = 0;
            rotSphere.y = Mathf.Clamp(Random.rotation.y, 0, 180);
        }
        else
        {
            rotSphere.x = 0;
            rotSphere.z = Random.rotation.z;
            rotSphere.y = 0;
        }
        transform.rotation = rotSphere;
    }

    private void OnMouseEnter()
    {
        SelectedDecorationOrb.hovering = this;
        hoverEffect.SetActive(true);
        //hoverPs.Play();
    }

    private void OnMouseExit()
    {
        SelectedDecorationOrb.hovering = null;
        hoverEffect.SetActive(false);
        //hoverPs.Stop();
    }

    private void OnMouseOver()
    {
        SelectedDecorationOrb.hovering = this;
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedDecorationOrb.selected == null)
                Select();
            else if (SelectedDecorationOrb.selected == this)
            {
                Deselect();
            } else
            {
                Vector3 selectedPosition = SelectedDecorationOrb.selected.transform.position;
                SelectedDecorationOrb.selected.Swap(transform.position);
                Swap(selectedPosition);
                SoundControl.onSwapDelegate();
            }
        }
    }

    private void OnEnable()
    {
        SetMaterial();
    }

    private void OnDisable()
    {
        hoverEffect.SetActive(false);
        selectionEffect.SetActive(false);
    }

    void Select()
    {
        selectionEffect.SetActive(true);
        SelectedDecorationOrb.selected = this;
    }

    public void Deselect()
    {
        selectionEffect.SetActive(false);
        if (SelectedDecorationOrb.selected == this)
            SelectedDecorationOrb.selected = null;
    }

    void Swap(Vector3 dest)
    {
        Deselect();
        destination = dest;
        swapping = true;
    }

    void LerpToDestination()
    {
        if (transform.position != destination)
            transform.position = Vector3.Lerp(transform.position, destination, GameManager.Instance.swapSpeed * Time.deltaTime);
        else
            swapping = false;
    }
}
