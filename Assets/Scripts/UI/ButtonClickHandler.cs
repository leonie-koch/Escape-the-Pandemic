using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if(UNITY_2018_3_OR_NEWER)
using UnityEngine.Android;
#endif

public class ButtonClickHandler : MonoBehaviour
{
    static AgoraInterface app = null;
    static GameObject video;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject hintPanel;

    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_2018_3_OR_NEWER)
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone)) 
        { }
        else{
            Permission.RequestUserPermission(Permission.Microphone);
        }
        if (Permission.HasUserAuthorizedPermission(Permission.Camera)) { }
        else
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnButtonClick()
    {
        Debug.Log("Button Clicked" + name);
        if (name.CompareTo("JoinButton") == 0)
        {
            OnJoinButtonClicked();
        }
        else if (name.CompareTo("LeaveButton") == 0)
        {
            OnLeaveButtonClicked();
        }
        else if (name.CompareTo("BackToStart") == 0)
        {
            BackToStart();
        }
        else if (name.CompareTo("BackToConnect") == 0)
        {
            BackToConnect();
        }else if (name.CompareTo("InMenuButton") == 0)
        {
            OpenMenu();
        }
        else if (name.CompareTo("ResumeButton") == 0)
        {
            Resume();
        }
    }
    private void OnJoinButtonClicked()
    {
        Debug.Log("join button clicked");
        GameObject go = GameObject.Find("ChannelName");
        InputField input = go.GetComponent<InputField>();

        //init AgoraEngine
        if (ReferenceEquals(app, null))
        {
            app = new AgoraInterface();
            app.loadEngine();

        }
        app.joinChannel(input.text);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        // SceneManager.LoadScene("ChatScene", LoadSceneMode.Single);
        SceneManager.LoadScene("ChooseRoomScene", LoadSceneMode.Single);


    }
    private void OnLeaveButtonClicked()
    {
        Debug.Log("leave clicked");
        if (!ReferenceEquals(app, null))
        {
            Debug.Log("leaving..");
            app.leaveChannel();
            app.unloadEngine();
            app = null;
            SceneManager.LoadScene("ConnectScene", LoadSceneMode.Single);
        }
    }
    public void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // if (scene.name.CompareTo("ChatScene") == 0)
        if (scene.name.CompareTo("ChooseRoomScene") == 0)
        {
            video = GameObject.Find("VideoChat");

            if (!ReferenceEquals(app, null))
            {
                app.OnChatSceneLoaded();
            }
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }
    }

    public void OnVideoChange(bool val)
    {
        Debug.Log("video change");
        video.SetActive(val);
    }

    void OnApplicationQuit() //leave Channel
    {
        OnLeaveButtonClicked();

    }
    

    public void BackToStart()
    {
        Debug.Log("back button clicked");
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void BackToConnect()
    {
        Debug.Log("back button clicked");
        OnLeaveButtonClicked();
        Debug.Log("after leaving");
        Destroy(GameObject.Find("Video"));
    }

    public void OpenMenu()
    {
        // Debug.Log("menu button clicked");
        // SceneManager.LoadScene("InMenuScene", LoadSceneMode.Single);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        // Debug.Log("menu button clicked");
        // SceneManager.LoadScene("InMenuScene", LoadSceneMode.Single);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        hintPanel.SetActive(false);
    }
}
