using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private const float TIME_LIMIT = 1800;
    private float finishTime;
    private float timeLeft;
    public Text timerText;
    

    void Start(){
        finishTime = Time.fixedTime + TIME_LIMIT;
        StartCoundownTimer();
    }

    void StartCoundownTimer()
    {
        print ("entered StartCoundownTimer");
        if (timerText != null)
        {            
            timerText.text = "30:00";
        }
    }
 
    void FixedUpdate()
    {
        timeLeft = finishTime - Time.fixedTime;

        if (timerText != null)
        {                      
            timerText.text = GetTimeAsString(timeLeft);
        }

        if(timeLeft <= 0)
        {
            Highscores.Instance.gameObject.GetComponent<Highscores>().GameLost();
            SceneManager.LoadScene("LooseScene");
        }
    }

    public int GetFinishTime()
    {
        return (int) (TIME_LIMIT - timeLeft);
    }

    public static string GetTimeAsString(float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        return minutes + ":" + seconds;
    }
}
