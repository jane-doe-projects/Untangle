using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] public Node predecessor;
    [SerializeField] public Node successor;
    [SerializeField] NodeBody child;
    [SerializeField] ColliderLine colliderLine;
    [SerializeField] GameObject lineBody;

    [SerializeField] public Collider2D nodeCollider;

    public delegate void OnSwap();
    public static event OnSwap onSwapDelegate;

    public NodeSet nodeset;

    Vector3 start;

    Vector3 destination;

    Node currentSwappingTarget;
    public bool swapping = false;
    public bool clickableInSwap = false;

    private void Start()
    {
        start = transform.position;
    }

    private void Update()
    {
        if (swapping)
        {
            LerpToDestination();
            UpdateNodeLines();
        }

    }

    /*
    void InitSwap(Vector3 dest)
    {
        destination = dest;
        swapping = true;
        SoundControl.onSwapDelegate();
    } */

    void InitSwap(GameObject swapTarget)
    {
        Node targetNode = swapTarget.GetComponent<Node>();
        destination = targetNode.start;
        if (targetNode.currentSwappingTarget != null)
        {
            if (targetNode.currentSwappingTarget != this)
            {
                Debug.Log("this is an issue");
                //destination = targetNode.currentSwappingTarget.start;
                //destination = targetNode.currentSwappingTarget.start;
                destination = targetNode.destination;
                targetNode.start = start;
            }
            else
            {
                Debug.Log("this is other");
                // swap start end destination of each
               /* Vector3 temp = start;
                start = destination;
                destination = temp; */
            }
        }
        else
            Debug.Log("FINNNNNNNNNEEEEEEEEEEEEEEEEEEEEEE");

        targetNode.currentSwappingTarget = this;
        swapping = true;
        SoundControl.onSwapDelegate();

        /*
        if (swapping)
        {
            StartCoroutine(DelaySwap(swapTarget));
        } else
        {
            Node targetNode = swapTarget.GetComponent<Node>();
            destination = targetNode.start;
            swapping = true;
            SoundControl.onSwapDelegate();
        } */
    }

    void LerpToDestination()
    {
        if (transform.position != destination)
        {
            transform.position = Vector3.Lerp(transform.position, destination, GameManager.Instance.swapSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) < 0.3)
                clickableInSwap = true;
        }
        else
        {
            currentSwappingTarget = null;
            swapping = false;
            start = transform.position;
            //start = destination;
            clickableInSwap = false;
            GameManager.Instance.currentSession.CheckCrossingState();
        }
    }

    void Swapper(GameObject origin, GameObject target)
    {
        Node ogNode = origin.GetComponent<Node>();
        Node tarNode = target.GetComponent<Node>();

        if (ogNode.swapping && tarNode.swapping)
        {
            //Debug.Log("double SWAPPING");
            Vector3 tempTarNodeDestination = tarNode.destination;
            tarNode.destination = ogNode.destination;
            ogNode.destination = tempTarNodeDestination;

            //Debug.Log(ogNode.destination + " " + tarNode.destination); // second was zero when only target swapping is checked

        } else
        {
            Vector3 tempTarNodeDestination = tarNode.destination;
            if (ogNode.swapping)
            {
                //Debug.Log("OG Swapping");
                tarNode.destination = ogNode.destination;
            }
            else
            {
                //Debug.Log("ognoswap");
                tarNode.destination = ogNode.start;
            }

            if (tarNode.swapping)
            {
                //Debug.Log("TAR Swapping");
                //ogNode.destination = tarNode.destination;
                ogNode.destination = tempTarNodeDestination;
                //Debug.Log(ogNode.destination + " " + tarNode.destination);
            }
            else
            {
                //Debug.Log("tarnoswap");
                ogNode.destination = tarNode.start;
            }
        }
        //Debug.Log("======");
        ogNode.swapping = true;
        tarNode.swapping = true;

        swapping = true;
        SoundControl.onSwapDelegate();
    }

    public void SelectNode()
    {
        if (NodeSwitch.selected != null && NodeSwitch.selected != this.gameObject)
        {
            GameManager.Instance.currentSession.IncreaseMoveCount();
            // init swap of node positions
            //InitSwap(NodeSwitch.selected.transform.position);
            /*
            InitSwap(NodeSwitch.selected); // new way of doing it
            NodeSwitch.selected.GetComponent<Node>().InitSwap(transform.gameObject); */
            Swapper(NodeSwitch.selected, transform.gameObject);

            NodeSwitch.selected.GetComponent<Node>().child.nodeSelected.SetActive(false);

            child.nodeSelected.SetActive(false);

            // reset selected
            NodeSwitch.selected = null;

            onSwapDelegate?.Invoke();

        }
        else
        {
            NodeSwitch.selected = this.gameObject;
            child.nodeSelected.SetActive(true);
        }
    }

    public void DeselectNode()
    {
        NodeSwitch.selected.GetComponent<Node>().child.nodeSelected.SetActive(false);
        NodeSwitch.selected = null;
    }

    void UpdateNodeLines()
    {
        // update lines to predecessor and successor
        predecessor.colliderLine.UpdateLine(predecessor.transform.position, this.transform.position);
        colliderLine.UpdateLine(this.transform.position, successor.transform.position);
    }

    public ColliderLine GetLine()
    {
        return colliderLine;
    }

    public void SetNodeVisual()
    {
        child.SetIcon();
    }

    public void DeactivateToSolved()
    {
        child.Solved();
    }

}