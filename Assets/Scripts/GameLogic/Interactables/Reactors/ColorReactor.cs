using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorReactor : StateReactor
{

    public Color active;
    public Color inactive;
    MeshRenderer mesh;

    protected override void Awake()
    {
        base.Awake();
        mesh = GetComponent<MeshRenderer>();
        React();
    }

    public override void React()
    {
        if (switcher.state)
        {
            mesh.material.color = active;
        }
        else
        {
            mesh.material.color = inactive;
        }
    }

}
