using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Node : MonoBehaviour
{

    public Transform cameraPosition;
    public List<Node> reachableNodes = new List<Node>();
    public Material grey, orange;
    Renderer marker;

    [HideInInspector]
    public Collider col;

    // Start is called before the first frame update
    void Awake()
    {
        col = GetComponent<Collider>();

    }
    private void Start()
    {
        GameManager.ins.camRig.basePos(cameraPosition);

    }

    private void OnMouseDown()
    {
        Arrive();
    }

    public virtual void Arrive()
    {
        //leave existing currentNode
        if(GameManager.ins.currentNode != null)
        {
            GameManager.ins.currentNode.Leave();
        }
       
        //set currentNode
        GameManager.ins.currentNode = this;

        //move the camera

        GameManager.ins.camRig.AlignTo(cameraPosition);
      

        //turn off own collider
        if (col != null)
        {
            col.enabled = false;
        }

        //turn on all reachable node`s colliders
        SetReachableNodes(true);      
    }

    public virtual void Leave()
    {
        //turn off all reachable node`s colliders
        SetReachableNodes(false);
    }

    public void SetReachableNodes(bool set)
    {
        foreach (Node node in reachableNodes)
        {
            if(node.transform.Find("Marker") != null)
            {
                marker = node.transform.Find("Marker").GetComponent<MeshRenderer>();
            }

            if (node.col != null )
            {
                if (node.GetComponent<Prerequisite>() && node.GetComponent<Prerequisite>().nodeAccess)
                {
                    if (node.GetComponent<Prerequisite>().Complete)
                    {
                        node.col.enabled = set;
                        changeMaterial(set, marker);
                    }
                }
                else
                {
                    node.col.enabled = set;
                    changeMaterial(set, marker);
                } 
            }

        }
    }

    void changeMaterial(bool set, Renderer marker)
    {
        if(marker != null)
        {
            if (set == true)
            {
                marker.material = orange;
            }
            else
            {
                marker.material = grey;
            }
        }
    }

}
