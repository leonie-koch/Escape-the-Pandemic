using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    // for single or double tap
    private float touchDuration;
    private Touch touch;
    private bool singleTap = false;
    private bool doubleTap = false;


    // for dragging
    [SerializeField] private float sensitivity = 0.1f;
    private float movingAxisX;
    private float movingAxisY;

    
    // for zooming in and out
    Vector3 touchStart;
    [SerializeField] private float zoomOutMin = 1;
    [SerializeField] private float zoomOutMax = 8;
	    
    
    void Update() {

        // Thanks to: https://forum.unity.com/threads/single-tap-double-tap-script.83794/#post-2975311
        if(Input.touchCount > 0) // if there is any touch
        { 
            touchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);
    
            if(touch.phase == TouchPhase.Ended && touchDuration < 0.2f) // making sure it only check the touch once && it was a short touch/tap and not a dragging.
            {
                StartCoroutine("SingleOrDouble");
            }

            // Thanks to: https://www.youtube.com/watch?v=3_CX-KtsDic
            if(touch.phase == TouchPhase.Moved)
            {
                movingAxisX = touch.deltaPosition.x * sensitivity;
                movingAxisY = touch.deltaPosition.y * sensitivity;
            }
        }
        else
        {
            touchDuration = 0.0f;
            singleTap = false;
            doubleTap = false;
        }

        // Thanks to: https://pressstart.vip/tutorials/2018/07/12/44/pan--zoom.html
        // if(Input.GetMouseButtonDown(0)){
        //     touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // }
        // if(Input.touchCount == 2)
        // {
        //     Touch touchZero = Input.GetTouch(0);
        //     Touch touchOne = Input.GetTouch(1);

        //     Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        //     Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        //     float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        //     float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        //     float difference = currentMagnitude - prevMagnitude;

        //     Zoom(difference * 0.01f);
        // }
        // else if(Input.GetMouseButton(0))
        // {
        //     Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Camera.main.transform.position += direction;
        // }

        // Zoom(Input.GetAxis("Mouse ScrollWheel"));


        // SWIPING --> https://www.youtube.com/watch?v=5YeksVWebQ0
    }
    
    private IEnumerator SingleOrDouble(){
        yield return new WaitForSeconds(0.3f);
        if(touch.tapCount == 1)
        {
            singleTap = true;
            doubleTap = false;
        }

        else if(touch.tapCount == 2)
        {            
            StopCoroutine("SingleOrDouble"); // this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            singleTap = false;
            doubleTap = true;
        }

        else
        {
            singleTap = false;
            doubleTap = false;
        }
    }

    public bool IsSingleTap()
    {
        return singleTap;
    }

    public bool IsDoubleTap()
    {
        return doubleTap;
    }

    public float GetMovingAxisX()
    {
        return movingAxisX;
    }

    public float GetMovingAxisY()
    {
        return movingAxisY;
    }

    private void Zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

}
