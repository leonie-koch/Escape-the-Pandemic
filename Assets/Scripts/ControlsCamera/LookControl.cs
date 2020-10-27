using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRig))]
public class LookControl : MonoBehaviour
{
    [SerializeField] private float XSensitivity = 2f;
    [SerializeField] private float YSensitivity = 2f;
    [SerializeField] private bool clampVerticalRotation = true;
    [SerializeField] private float MinimumX = -90F;
    [SerializeField] private float MaximumX = 90F;
    [SerializeField] private bool smooth;
    [SerializeField] private float smoothTime = 5f;
    //public bool lockCursor = true;


    private Quaternion yAxis;
    private Quaternion xAxis;
    //private bool m_cursorIsLocked = true;
    private CameraRig rig;
    private ControlManager controller;
    

    private void Start()
    {
        rig = GetComponent<CameraRig>();
        controller = GameObject.Find("ControlManager").GetComponent<ControlManager>();
    }

    private void Update()
    {
        //no tap on the screen will be detected when the ivCanvas or obsCamera is active
        if(controller.UseAction1())
        {
            if (GameManager.ins.ivCanvas.gameObject.activeInHierarchy || GameManager.ins.obsCamera.gameObject.activeInHierarchy)
            {
                return;
            }

            yAxis = rig.y_axis.localRotation;
            xAxis = rig.x_axis.localRotation;
            LookRotation();
        }
    }
    

    public void LookRotation()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        yAxis *= Quaternion.Euler(0f, yRot, 0f);
        xAxis *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            xAxis = ClampRotationAroundXAxis(xAxis);

        if (smooth)
        {
            rig.y_axis.localRotation = Quaternion.Slerp(rig.y_axis.localRotation, yAxis,
                smoothTime * Time.deltaTime);
            rig.x_axis.localRotation = Quaternion.Slerp(rig.x_axis.localRotation, xAxis,
                smoothTime * Time.deltaTime);
        }
        else
        {
            rig.y_axis.localRotation = yAxis;
            rig.x_axis.localRotation = xAxis;
        }

        //UpdateCursorLock();
    }
    
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
