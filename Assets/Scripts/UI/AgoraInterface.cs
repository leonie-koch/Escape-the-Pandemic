using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

public class AgoraInterface : MonoBehaviour
{    
    private static string appId = "fda76a63c95b421b828c369eb95c2c33";
    public IRtcEngine mRtcEngine;
    public uint mRemotePeer;
    private GameObject canvas;
    private GameObject videoSurface;
    public void loadEngine()
    {
        // Debug.Log("initialize engine");

        if (mRtcEngine != null)
        {
            Debug.Log("Engine already exits. Please unload it first!");
            return;
        }
        //init RTC engine
        mRtcEngine = IRtcEngine.getEngine(appId);
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG);

    }
    public void joinChannel(string channelName)
    {
        // Debug.Log("Joining Channel:" + channelName);
        if (mRtcEngine == null)
        {
            Debug.Log("Engine needs to be initialized before joining a channel");
            return;
        }
        //set callbacks
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        //enable video
        mRtcEngine.EnableVideo();

        //allow camera output callback
        mRtcEngine.EnableVideoObserver();

        //join the channel
        mRtcEngine.JoinChannel(channelName, null, 0);

    }
    public void leaveChannel()
    {
        Debug.Log("leaving channel");
        if (mRtcEngine == null)
        {
            Debug.Log("Engine needs to be initialized before leaving a channel");
            return;
        }
        //leave channel
        mRtcEngine.LeaveChannel();
        //remove video observer
        mRtcEngine.DisableVideoObserver();
    }
    public void unloadEngine()
    {
        Debug.Log("Unload Agora Engine!");
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }

    //engine callbacks
    private void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        // Debug.Log("Succesfully joinded channel " + channelName + " with id: " + channelName);

        Highscores.Instance.gameObject.GetComponent<Highscores>().AddChannelName(channelName);
    }
    private void OnUserJoined(uint uid, int elapsed)
    {
        // Debug.Log("New User has joined the channel");
        videoSurface.SetActive(true);
        videoSurface.name = uid.ToString();
        VideoSurface o = videoSurface.GetComponent<VideoSurface>();
        o.SetForUser(uid);
        o.SetEnable(true);
        mRemotePeer = uid;
    }
    private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("User has left the channel");
        videoSurface.SetActive(false);
    }

    public void OnChatSceneLoaded()
    {
        canvas = GameObject.Find("Canvas");
        videoSurface = GameObject.Find("VideoChat");
        videoSurface.SetActive(false);
       
    }
}
