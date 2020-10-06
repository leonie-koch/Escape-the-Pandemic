using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MoveReactor : StateReactor
{
    public Vector3 active;
    public Vector3 inactive;

    

    protected override void Awake()
    {
        base.Awake();
        React();
    }

    public override void React()
    {
        if (switcher.state)
        {
            transform.localPosition = active;
        }
        else if(switcher.state == false)
        {
            transform.localPosition = inactive;
        }
    }
}
