using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHandlerChatScene : MonoBehaviour
{   
        
    public Toggle homeToggle;
    public Toggle labToggle;

    private string roomScene = "RoomScene";
    

    void Start()
    {
        homeToggle.onValueChanged.AddListener(delegate
        {
            chooseRoom();
        });

        labToggle.onValueChanged.AddListener(delegate
        {
            chooseRoom();
        });
        
        // DontDestroyOnLoad(transform.root.gameObject);
    }

    void chooseRoom()
    {
        if (homeToggle.isOn)
        {            
            roomScene = ("BookScene");            
            // Debug.Log("Current scene choice is: " + roomScene);            
        }

        if (labToggle.isOn)
        {
            roomScene = ("RoomScene");            
            // Debug.Log("Current scene choice is: " + roomScene);
        }
    }

    public void submitRoom()
    {
        if(roomScene == "BookScene")
        {
            Highscores.Instance.gameObject.GetComponent<Highscores>().RoomChosen(0, this);
        }
        else if(roomScene == "RoomScene")
        {
            Highscores.Instance.gameObject.GetComponent<Highscores>().RoomChosen(1, this);
        }        
    }

    public void loadRoomScene()
    {        
        SceneManager.LoadScene(roomScene);
    }    
}
