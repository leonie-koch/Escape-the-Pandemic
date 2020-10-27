using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class RotateReactor : StateReactor
{
    protected override void Awake()
    {
        base.Awake();
        React();
    }

    public override void React()
    {
        if (switcher.state)
        {
            transform.localRotation = Quaternion.Euler(transform.rotation.x, -85, transform.rotation.z);
        }
        else
        {
            transform.rotation = transform.rotation;
        }
    }
}
