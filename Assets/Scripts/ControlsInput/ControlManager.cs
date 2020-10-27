using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    private TouchControl touchControl;
    private DesktopControl desktopControl;
    private bool userIsOnPhone = false;
    private bool userIsOnDesktop = false;
    private bool action1 = false;
    private bool action2 = false;

    
    void Start()
    {

        if (SystemInfo.supportsGyroscope)
        {            
            touchControl = GetComponent<TouchControl>();
            touchControl.enabled = true;
            userIsOnPhone = true;

            GameObject.Find("Main Camera").GetComponent<GyroControl>().enabled = true;            
        }

        else
        {
            desktopControl = GetComponent<DesktopControl>();
            desktopControl.enabled = true;
            userIsOnDesktop = true;
        }
    }
    
    void Update()
    {
        if(userIsOnPhone)
        {
            action1 = touchControl.IsSingleTap();
            action2 = touchControl.IsDoubleTap();
        }

        if(userIsOnDesktop)
        {
            action1 = desktopControl.IsLeftClick();
            action2 = desktopControl.IsRightClick();
        }
    }

    public bool UseAction1()
    {
        return action1;
    }

    public bool UseAction2()
    {
        return action2;
    }

    public float UseDraggingX()
    {
        if(userIsOnPhone)
        {
            return touchControl.GetMovingAxisX();
        }

        if(userIsOnDesktop)
        {
            return desktopControl.GetAxisMouseX();
        }
        return 0;
    }

    public float UseDraggingY()
    {
        if(userIsOnPhone)
        {
            return touchControl.GetMovingAxisY();
        }

        if(userIsOnDesktop)
        {
            return desktopControl.GetAxisMouseY();
        }
        return 0;
    }
}