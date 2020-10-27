using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public AutoFlip book;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SwipeManager.IsSwipingLeft())
        {
            Debug.Log("swiped left ");
            book.FlipRightPage();

        }

        if (SwipeManager.IsSwipingRight())
        {
            Debug.Log("swiped right ");
            book.FlipLeftPage();

        }

        // OR

        if (SwipeManager.IsSwiping())
            {
                // do something
            }
        
    }
}
