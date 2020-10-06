using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : Interactable
{

    public bool state;

    //event setup
    public delegate void OnStateChange();
    public event OnStateChange Change;

    public override void Interact()
    {
        state = !state;
        if(Change != null)
        {
            Change();
        }
    }

}
