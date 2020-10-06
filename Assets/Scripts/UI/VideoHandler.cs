using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class VideoHandler : MonoBehaviour
{
    public GameObject video;
    public Toggle videoToggle;
   
    public Animator videoBox;

    void Start()
    {
        videoToggle.onValueChanged.AddListener(delegate
        {
            changeVideoVisibility();
        });

        DontDestroyOnLoad(transform.root.gameObject);
    }
    void changeVideoVisibility()
    {
        if (videoToggle.isOn)
        {
            videoBox.SetBool("isHidden", false);
        }
        else
            videoBox.SetBool("isHidden", true);
    }

  
}
