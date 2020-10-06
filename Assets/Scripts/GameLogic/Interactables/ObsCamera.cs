using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsCamera : MonoBehaviour
{
    [HideInInspector] public Transform model;
    [HideInInspector] public Transform rig;
    

    private Quaternion modelRot;
    private Quaternion rigRot;
    private ControlManager controller;

    private void Start()
    {        
        controller = GameObject.Find("ControlManager").GetComponent<ControlManager>();
    }

    private void Update()
    {
        if (controller.UseAction1())
        {            
            if (model == null)
            {
                return;
            }

            modelRot = model.rotation;
            rigRot = rig.rotation;
            ObjectRotation();
        }
    }

    public void ObjectRotation()
    {
        // ControllerTask: needs to be moved later and translated for touch devices
        float yRot = controller.UseDraggingX();
        float xRot = controller.UseDraggingY();

        modelRot *= Quaternion.Euler(0f, -yRot, 0f);
        rigRot *= Quaternion.Euler(xRot, 0f, 0f);
        
        rigRot = ClampRotationAroundXAxis(rigRot);

        model.rotation = modelRot;
        rig.rotation = rigRot;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -80, 80);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void Close()
    {
        Destroy(model.gameObject);
        rig.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}