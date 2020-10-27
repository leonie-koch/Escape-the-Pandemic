using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateReactor : MonoBehaviour
{
  
    public Switcher switcher;

    protected virtual void Awake()
    {
        switcher.Change += React;
    }

    public virtual void React()
    {
        Debug.Log(name + "`s state is " + switcher.state);
    }

}
