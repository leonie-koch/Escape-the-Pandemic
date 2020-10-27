using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopControl : MonoBehaviour
{
    [SerializeField] private float sensitivity = 3f; // for dragging
    private bool leftClick = false;
    private bool rightClick = false;

    
    void Update()
    {
        if(Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            leftClick = true;
            rightClick = false;
        }

        else if (Input.GetMouseButtonDown(1))
        {
            leftClick = false;
            rightClick = true;
        }
        
        else
        {
            leftClick = false;
            rightClick = false;
        }

    }

    public bool IsLeftClick()
    {
        return leftClick;
    }

    public bool IsRightClick()
    {
        return rightClick;
    }

    public float GetAxisMouseX()
    {
        return Input.GetAxis("Mouse X") * sensitivity;
    }

    public float GetAxisMouseY()
    {
        return Input.GetAxis("Mouse Y") * sensitivity;
    }

}
