using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Prop))]
public abstract class Interactable : MonoBehaviour
{
    
    void Start()
    {
        this.enabled = false;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + name);
    }
}